namespace WWC.Consul.Check.API.Model.Enum;

public enum ClusterCommandType
{
    /// <summary>
    /// 从到主
    /// </summary>
    SlaveToMaster = 1, 
    
    /// <summary>
    /// 主到从
    /// </summary>
    MasterToSlave = 2,

}
