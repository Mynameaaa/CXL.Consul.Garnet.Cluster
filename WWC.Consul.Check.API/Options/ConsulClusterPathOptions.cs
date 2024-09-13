namespace WWC.Consul.Check.API.Options;

public class ConsulClusterPathOptions
{

    /// <summary>
    /// 主节点
    /// </summary>
    public string Leader { get; set; }

    /// <summary>
    /// 备用节点
    /// </summary>
    public List<string> Follow { get; set; }

}
