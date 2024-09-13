using System.ComponentModel.DataAnnotations;
using WWC.Consul.Check.API.Model.Enum;

namespace WWC.Consul.Check.API.Db.Entities;

public class RedisClusterNode
{

    /// <summary>
    /// 主键自增
    /// </summary>
    [Key]
    public long CID { get; set; }

    /// <summary>
    /// 集群节点编号
    /// </summary>
    public string ClusterNodeID { get; set; }

    /// <summary>
    /// 监听地址
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// 监听端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 访问间隔 (秒)
    /// </summary>
    public int Interval { get; set; }

    /// <summary>
    /// 超时时间 (秒)
    /// </summary>
    public int TimeOut { get; set; }

    /// <summary>
    /// 协议标签
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// 主节点编号
    /// </summary>
    public string? MasterID { get; set; }

    /// <summary>
    /// 所属 Consul 节点名称
    /// </summary>
    public string ConsulNodeName { get; set; }

    /// <summary>
    /// Consul 服务名称
    /// </summary>
    public string ConsulServieName { get; set; }

    /// <summary>
    /// 是否主节点
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// 通信端口
    /// </summary>
    public int ChannelPort { get; set; }

    /// <summary>
    /// 是否已注册
    /// </summary>
    public bool IsRegister { get; set; }

    ///// <summary>
    ///// 节点状态
    ///// </summary>
    //public ClusterNodeState NodeState { get; set; }

}
