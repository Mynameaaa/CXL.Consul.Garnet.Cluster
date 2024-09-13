using Consul;
using Newtonsoft.Json;
using System.Net.Http;
using WWC.Consul.Check.API.Cache;
using WWC.Consul.Check.API.IServices;

namespace WWC.Consul.Check.API.Services;

public class ServiceStatusService : IServiceStatusService
{
    private readonly ILogger<ServiceStatusService> _logger;
    private readonly IConsulClient _consulClient;
    public ServiceStatusService(ILogger<ServiceStatusService> logger
        , IConsulClient consulClient)
    {
        _logger = logger;
        _consulClient = consulClient;
    }

    /// <summary>
    /// 获取异常服务信息
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, AgentService>> GetExceptionServiceAsync(Dictionary<string, AgentService> services)
    {
        try
        {
            Dictionary<string, AgentService> result = new Dictionary<string, AgentService>();
            services = services.Where(p => DbConsulCacheService.GetServiceNames().Contains(p.Key)).ToDictionary();

            foreach (var service in services)
            {
                //获取服务的健康状态
                var healthResponse = (await _consulClient.Health.Service(service.Key)).Response;

                if (healthResponse == null)
                {
                    continue;
                }

                bool isContinue = false;
                //只要有一个正常
                foreach (var health in healthResponse)
                {
                    if (health.Checks.Any(wh => wh.Status == HealthStatus.Passing && services.Keys.Contains(wh.ServiceName)))
                    {
                        isContinue = true;
                        continue;
                    }
                }

                if (isContinue) continue;

                //异常状态添加
                result.Add(service.Key, service.Value);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogInformation("请求服务信息出现异常：【{0}】", ex.ToString());
            throw;
        }
    }

}
