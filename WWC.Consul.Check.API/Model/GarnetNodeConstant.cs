namespace WWC.Consul.Check.API.Model;

public class GarnetNodeConstant
{
    public static string[] AllNodes { get; set; }

    /// <summary>
    /// 从替换主
    /// </summary>
    public static string _CLUSTERFAILOVERFORCE = "CLUSTER FAILOVER FORCE";

    /// <summary>
    /// 主降级从
    /// </summary>
    public static string _CLUSTERREPLICATE = "CLUSTER REPLICATE {0}"; 
    
    /// <summary>
    /// 设为空闲
    /// </summary>
    public static string _CLUSTERRESETHARD = "CLUSTER RESET HARD"; 

}
