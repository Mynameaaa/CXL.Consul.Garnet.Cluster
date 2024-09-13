
using Consul;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using WWC.Consul.Check.API.Cache;
using WWC.Consul.Check.API.IServices;
using WWC.Consul.Check.API.Options;
using WWC.Consul.Check.API.Services;

namespace WWC.Consul.Check.API.HostedService;

public class CheckGarnetNodeHostedService : IHostedService, IDisposable
{
    private readonly ILogger<CheckGarnetNodeHostedService> _logger;
    private Timer _timer = default(Timer);
    private readonly IServiceStatusService _serviceStatusService;
    private bool _isStartFailover = false;
    private readonly IGarnetNodeMatchService _garnetNodeMatchService;
    private readonly IGarnetFailoverSerivce _garnetFailoverSerivce;
    private readonly IConsulClient _consulClient;
    private readonly ConsulNodeNameOptions _consulNodeNameOptions; 
    //public static bool locks = true;

    public CheckGarnetNodeHostedService(ILogger<CheckGarnetNodeHostedService> logger
        , IServiceStatusService serviceStatusService
        , IGarnetNodeMatchService garnetNodeMatchService
        , IGarnetFailoverSerivce garnetFailoverSerivce
        , IConsulClient consulClient
        , IOptions<ConsulNodeNameOptions> _optionsMonitor)
    {
        _consulNodeNameOptions = _optionsMonitor.Value;
        _logger = logger;
        _serviceStatusService = serviceStatusService;
        _garnetNodeMatchService = garnetNodeMatchService;
        _garnetFailoverSerivce = garnetFailoverSerivce;
        _consulClient = consulClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(3000);
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));
    }

    private async void DoWork(object state)
    {
        await ProcessResponseAsync();
    }

    public async Task ProcessResponseAsync()
    {
        if (_isStartFailover)
            return;

        try
        {
            var node = await _consulClient.Catalog.Node(_consulNodeNameOptions.NodeName);

            if (node.Response == null)
            {
                _logger.LogError("未在 Consul 获取节点信息");
                return;
            }

            var exceptionServiceList = await _serviceStatusService.GetExceptionServiceAsync(node.Response.Services);

            if (!exceptionServiceList.Any())
            {
                return;
            }

            _logger.LogWarning("【garnet 集群出现了故障节点，故障数量：{0}】", exceptionServiceList.Count);

            try
            {
                _logger.LogWarning("【开始进行故障转移操作，时间：{0}】", DateTime.Now.ToString("yyyyMMddHHmmss_ffff"));
                //故障转移操作
                _isStartFailover = true;

                #region 从节点转换为主节点操作

                var garnetNodes = await _garnetNodeMatchService.MatchGarnetNodesAsync(exceptionServiceList);
                var followCommands = garnetNodes.Where(p => !p.IsTask).ToList();
                if (followCommands != null && followCommands.Any())
                {
                    var command = await _garnetFailoverSerivce.ExcuteCommandAsync(followCommands);
                }

                #endregion

                #region 主节点转换为从节点指令保存

                foreach (var item in garnetNodes.Where(p => p.IsTask))
                {
                    var result = MasterToSlaveCacheService.SetQueueValue(item);
                    switch (result)
                    {
                        case Model.Enum.QueueTaskResult.Added:
                            _logger.LogWarning("【集群指令缓存成功】");
                            break;
                        case Model.Enum.QueueTaskResult.Exist:
                            _logger.LogWarning("【缓存已包含的集群指令】");
                            break;
                        case Model.Enum.QueueTaskResult.TypeError:
                            _logger.LogError("【集群指令缓存类型异常】");
                            break;
                        case Model.Enum.QueueTaskResult.Fail:
                            _logger.LogError("【集群指令缓存失败】");
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                _logger.LogWarning("【故障转移操作执行结束，时间：{0}】", DateTime.Now.ToString("yyyyMMddHHmmss_ffff"));
            }
            catch (Exception ex)
            {
                _logger.LogError("故障转移操作过程中出现异常，异常信息：【{0}】", ex.ToString());
                return;
            }
            finally
            {
                _isStartFailover = false;
                //locks = false;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError("获取 Garnet 节点信息异常，异常信息：【{0}】", ex.ToString());
            return;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
