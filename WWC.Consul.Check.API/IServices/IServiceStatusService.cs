using Consul;

namespace WWC.Consul.Check.API.IServices;

public interface IServiceStatusService
{

    /// <summary>
    /// 获取异常服务信息
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    Task<Dictionary<string, AgentService>> GetExceptionServiceAsync(Dictionary<string, AgentService> services);

}
