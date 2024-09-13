
using Consul;
using Newtonsoft.Json;
using StackExchange.Redis;
using WWC.Consul.Check.API.Cache;
using WWC.Consul.Check.API.IServices;
using WWC.Consul.Check.API.Model.Enum;

namespace WWC.Consul.Check.API.HostedService;

public class GarnetNodeLevelReductionHostedService : IHostedService
{
    private readonly ILogger<CheckGarnetNodeHostedService> _logger;
    private Timer _timer = default(Timer);
    private bool _isStartLevelReduction = false;
    private readonly IGarnetFailoverSerivce _garnetFailoverSerivce;
    private readonly IConsulClient _consulClient;

    public GarnetNodeLevelReductionHostedService(ILogger<CheckGarnetNodeHostedService> logger
        , IGarnetFailoverSerivce garnetFailoverSerivce
        , IConsulClient consulClient)
    {
        _logger = logger;
        _garnetFailoverSerivce = garnetFailoverSerivce;
        _consulClient = consulClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(5000);
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
    }

    private async void DoWork(object state)
    {
        //while (CheckGarnetNodeHostedService.locks)
        //{
        //    Thread.Sleep(2000);
        //}
        await ProcessResponseAsync();
    }

    public async Task ProcessResponseAsync()
    {
        if (_isStartLevelReduction)
            return;

        try
        {
            _isStartLevelReduction = true;

            var errorService = MasterToSlaveCacheService.GroupServiceName();

            if (errorService == null || !errorService.Any())
                return;

            var errorServiceResult = new List<string>();

            foreach (var serviceName in errorService)
            {
                //获取服务的健康状态
                var healthResponse = (await _consulClient.Health.Service(serviceName)).Response;

                if (healthResponse == null)
                {
                    continue;
                }

                //只要有一个正常
                foreach (var health in healthResponse)
                {
                    if (health.Checks.Any(wh => wh.Status == HealthStatus.Passing && errorService.Contains(wh.ServiceName)))
                        errorServiceResult.Add(serviceName);
                }
            }

            var masterToSlave = MasterToSlaveCacheService.ConsumerCommand(errorServiceResult);

            if (masterToSlave == null || !masterToSlave.Any())
            {
                _logger.LogError($"本应该需要降级的节点无法降级，服务名称：【{errorServiceResult}】");
                return;
            }

            await _garnetFailoverSerivce.ExcuteCommandAsync(masterToSlave);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError("降级承载服务出现异常：【{0}】", ex.ToString());
            throw;
        }
        finally
        {
            _isStartLevelReduction = false;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
