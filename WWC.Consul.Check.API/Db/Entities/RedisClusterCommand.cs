using System.ComponentModel.DataAnnotations;
using WWC.Consul.Check.API.Model.Enum;

namespace WWC.Consul.Check.API.Db.Entities;

public class RedisClusterCommand
{
    /// <summary>
    /// 编号
    /// </summary>
    [Key]
    public long ID { get; set; }

    /// <summary>
    /// 集群指令
    /// </summary>
    [StringLength(50)]
    [Required]
    public string Command { get; set; }

    /// <summary>
    /// Consul 服务名称
    /// </summary>
    [StringLength(50)]
    [Required]
    public string ConsulSerivceName { get; set; }

    /// <summary>
    /// 监听地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 监听端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 指令状态
    /// </summary>
    public ClusterCommandState CommandState { get; set; }

    /// <summary>
    /// 指令类型
    /// </summary>
    public ClusterCommandType CommandType { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 执行时间
    /// </summary>
    public DateTime? ExecuteTime { get; set; }

    /// <summary>
    /// 执行结果
    /// </summary>
    [StringLength(500)]
    [Required]
    public string ExecuteResult { get; set; }

}
