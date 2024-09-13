using WWC.Consul.Check.API.Model;

namespace WWC.Consul.Check.API.IServices;

public interface IGarnetFailoverSerivce
{
    
    /// <summary>
    /// 提交故障转移指令
    /// </summary>
    /// <param name="clusterModel"></param>
    /// <returns></returns>
    Task<bool> ExcuteCommandAsync(List<ClusterCommandModel> clusterModels);

}
