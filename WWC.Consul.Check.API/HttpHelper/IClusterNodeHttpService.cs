using WWC.Consul.Check.API.Db.Entities;
using WWC.Consul.Check.API.Model;

namespace WWC.Consul.Check.API.HttpHelper;

public interface IClusterNodeHttpService
{

    /// <summary>
    /// 远程获取节点信息
    /// </summary>
    /// <returns></returns>
    Task<List<RedisClusterNode>> QueryClusterNodesByNodeName();
    
    /// <summary>
    /// 获取集群节点服务信息
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    Task<List<ClusterService>> QueryClusterNodeServiceIPPortByNodeName();

}
