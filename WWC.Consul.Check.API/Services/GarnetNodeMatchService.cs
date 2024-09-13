using Consul;
using StackExchange.Redis;
using System.Reflection;
using WWC.Consul.Check.API.Cache;
using WWC.Consul.Check.API.Extensions;
using WWC.Consul.Check.API.IServices;
using WWC.Consul.Check.API.Model;
using WWC.Consul.Check.API.Model.Enum;

namespace WWC.Consul.Check.API.Services;

public class GarnetNodeMatchService : IGarnetNodeMatchService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<GarnetNodeMatchService> _logger;
    private readonly Func<string, IConnectionMultiplexer> _okConnectionMultiplexerFunc;

    public GarnetNodeMatchService(IConnectionMultiplexer connectionMultiplexer
        , ILogger<GarnetNodeMatchService> logger
        , Func<string, IConnectionMultiplexer> okConnectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
        _okConnectionMultiplexerFunc = okConnectionMultiplexer;
    }

    /// <summary>
    /// 获取集群节点信息
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public async Task<List<ClusterCommandModel>> MatchGarnetNodesAsync(Dictionary<string, AgentService> nodes)
    {
        try
        {
            var downAddressPorts = nodes.Select(sl => $"{sl.Value.Address}:{sl.Value.Port}");
            var okNodeAddressPort = DbConsulCacheService.GetAddressPort().FirstOrDefault(p => !downAddressPorts.Contains(p));

            if (okNodeAddressPort == null)
            {
                throw new Exception("集群不存在状态为 connected 的节点");
            }

            var okNode = _okConnectionMultiplexerFunc.Invoke(okNodeAddressPort);
            var server = okNode.GetServer(okNodeAddressPort);
            var allNodes = (await server.ClusterNodesAsync())?.Nodes;

            if (allNodes == null)
            {
                throw new Exception("集群不存在节点信息");
            }
            var result = new List<ClusterCommandModel>(nodes.Count);

            foreach (var node in allNodes)
            {
                var endPoint = node.EndPoint;
                if (endPoint == null)
                {
                    continue;
                }

                var portValue = Convert.ToInt32(endPoint.GetFieldValue("_port", BindingFlags.Instance | BindingFlags.NonPublic));
                var addressValue = endPoint.GetFieldValue("_address", BindingFlags.Instance | BindingFlags.NonPublic)?.ToString();

                if (portValue == 0 || string.IsNullOrWhiteSpace(addressValue))
                {
                    throw new Exception("不正确节点的连接信息");
                }

                if (!downAddressPorts.Contains(addressValue + ":" + portValue))
                {
                    continue;
                }

                var isPrimary = !node.IsReplica;

                if (isPrimary)
                {
                    //未添加策略
                    var follow = allNodes.Where(wh => wh.ParentNodeId == node.NodeId && !wh.IsFail).FirstOrDefault();
                    if (follow == null && (node.Slots == null || node.Slots.Count <= 0))
                    {
                        _logger.LogError($"集群节点编号为：【{node.NodeId}】 的节点，子节点已经成功完成故障转移，本身未转为子节点");
                        continue;
                    }

                    var followAddress = follow.EndPoint?.GetFieldValue("_address", BindingFlags.Instance | BindingFlags.NonPublic)?.ToString() ?? "";
                    var followPort = Convert.ToInt32(follow.EndPoint?.GetFieldValue("_port", BindingFlags.Instance | BindingFlags.NonPublic));

                    var masterService = nodes.FirstOrDefault(x => $"{x.Value.Address}:{x.Value.Port}" == $"{addressValue}:{portValue}");

                    //从节点替换主节点
                    result.Add(new ClusterCommandModel()
                    {
                        Address = followAddress,
                        Port = followPort,
                        ClusterID = follow.NodeId,
                        Command = new List<string> 
                        { 
                            GarnetNodeConstant._CLUSTERFAILOVERFORCE 
                        },
                        IsTask = false,
                        ServiceName = "garnet-" + followPort,
                        CommandState = ClusterCommandState.Unexecuted,
                    });

                    //主节点替换从节点
                    result.Add(new ClusterCommandModel()
                    {
                        Address = addressValue,
                        Port = portValue,
                        ClusterID = node.NodeId,
                        IsTask = true,
                        Command = new List<string>()
                        {
                            //GarnetNodeConstant._CLUSTERRESETHARD,
                            string.Format(GarnetNodeConstant._CLUSTERREPLICATE, follow.NodeId)
                        },
                        ServiceName = masterService.Key,
                        CommandState = ClusterCommandState.Unexecuted,
                    });
                }
                else
                {
                    _logger.LogWarning($"集群中节点编号为 【{node.NodeId}】 的从节点出现问题");
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError("匹配集群节点时出现异常，异常信息：【{0}】", ex.ToString());
            throw;
        }
    }
}