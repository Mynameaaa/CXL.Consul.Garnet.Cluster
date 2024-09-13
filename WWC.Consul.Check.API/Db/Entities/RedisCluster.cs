using System.ComponentModel.DataAnnotations;

namespace WWC.Consul.Check.API.Db.Entities;

public class RedisCluster
{

    /// <summary>
    /// 主键自增
    /// </summary>
    [Key]
    public long ID { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

}
