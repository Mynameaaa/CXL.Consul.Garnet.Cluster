using Consul;
using Microsoft.Extensions.Options;
using WWC.Consul.Check.API.Db;
using WWC.Consul.Check.API.IServices;
using WWC.Consul.Check.API.Options;

namespace WWC.Consul.Check.API.HostedService;

public class LoadGarnetNodeHostedService : IHostedService
{
    private readonly IConsulServiceLoadService _consulServiceLoadService;
    private readonly ConsulNodeNameOptions _nodeInfo;
    private readonly ILogger<LoadGarnetNodeHostedService> _logger;
    private readonly IConsulClient _consulClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public LoadGarnetNodeHostedService(IConsulServiceLoadService consulServiceLoadService
        , IOptionsMonitor<ConsulNodeNameOptions> optionsMonitor
        , ILogger<LoadGarnetNodeHostedService> logger
        , IConsulClient consulClient
        , IServiceScopeFactory serviceScopeFactory)
    {
        _consulServiceLoadService = consulServiceLoadService;
        _nodeInfo = optionsMonitor.CurrentValue;
        _logger = logger;
        _consulClient = consulClient;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var _dbContext = scope.ServiceProvider.GetRequiredService<GarnetClusterDbContext>();

        var (regsiterServiceList, transaction) = await _consulServiceLoadService.LoadService(_nodeInfo.NodeName, _dbContext);

        if (regsiterServiceList == null || !regsiterServiceList.Any())
        {
            return;
        }
        try
        {
            foreach (var item in regsiterServiceList)
            {
                await _consulClient.Agent.ServiceRegister(item);
            }

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError("加载 Consul 服务出现异常：【{0}】", ex.ToString());
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
