using Consul;
using WWC.Consul.Check.API.Model;

namespace WWC.Consul.Check.API.IServices;

public interface IGarnetNodeMatchService
{

    /// <summary>
    /// 获取集群节点信息
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    Task<List<ClusterCommandModel>> MatchGarnetNodesAsync(Dictionary<string, AgentService> nodes);

}
