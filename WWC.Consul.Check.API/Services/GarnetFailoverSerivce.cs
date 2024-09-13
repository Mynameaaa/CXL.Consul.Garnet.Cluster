using StackExchange.Redis;
using WWC.Consul.Check.API.IServices;
using WWC.Consul.Check.API.Model;
using WWC.Consul.Check.API.Model.Enum;

namespace WWC.Consul.Check.API.Services;

public class GarnetFailoverSerivce : IGarnetFailoverSerivce
{
    private readonly ILogger<GarnetFailoverSerivce> _logger;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    public GarnetFailoverSerivce(ILogger<GarnetFailoverSerivce> logger
        , IConnectionMultiplexer connectionMultiplexer)
    {
        _logger = logger;
        _connectionMultiplexer = connectionMultiplexer;
    }

    /// <summary>
    /// 提交故障转移指令
    /// </summary>
    /// <param name="clusterModel"></param>
    /// <returns></returns>
    public async Task<bool> ExcuteCommandAsync(List<ClusterCommandModel> clusterModels)
    {
        _logger.LogWarning("开始执行故障转移指令，时间：【{0}】", DateTime.Now.ToString("yyyyMMddHHmmss_ffff"));

        foreach (var clusterModel in clusterModels)
        {
            try
            {
                var server = _connectionMultiplexer.GetServer($"{clusterModel.Address}:{clusterModel.Port}");
                foreach (var command in clusterModel.Commands)
                {
                    var result = await server.ExecuteAsync(command.Command, command.Params.ToArray());
                    clusterModel.CommandState = ClusterCommandState.Executed;
                }
            }
            catch (Exception ex)
            {
                clusterModel.CommandState = ClusterCommandState.ExecutionFail;
                _logger.LogError("执行故障转移指令时候出现异常：【{0}】，异常服务：【{1}】", ex.ToString(), clusterModel.ServiceName);
                return false;
            }
        }

        _logger.LogInformation("执行故障转移指令完毕，时间：【{0}】", DateTime.Now.ToString("yyyyMMddHHmmss_ffff"));
        return true;

    }

}
