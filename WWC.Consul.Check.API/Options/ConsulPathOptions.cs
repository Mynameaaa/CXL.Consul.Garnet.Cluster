namespace WWC.Consul.Check.API.Options;

public class ConsulPathOptions
{
    /// <summary>
    /// 获取 Consul 节点信息根路径
    /// </summary>
    public string NodeRootPath { get; set; }

    /// <summary>
    /// 获取 Consul 服务信息根路径
    /// </summary>
    public string ServiceRootPath { get; set; }

    /// <summary>
    /// 获取集群节点 API
    /// </summary>
    public string ClusterNodesAPI { get; set; }

}
