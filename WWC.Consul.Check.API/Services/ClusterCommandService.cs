using StackExchange.Redis;
using WWC.Consul.Check.API.IServices;
using WWC.Consul.Check.API.Model;

namespace WWC.Consul.Check.API.Services;

public class ClusterCommandService : IClusterCommandService
{
    private readonly ILogger<GarnetFailoverSerivce> _logger;
    //private readonly IConnectionMultiplexer _connectionMultiplexer;
    //public ClusterCommandService(ILogger<GarnetFailoverSerivce> logger
    //    , IConnectionMultiplexer connectionMultiplexer)
    //{
    //    _logger = logger;
    //    _connectionMultiplexer = connectionMultiplexer;
    //}

    ///// <summary>
    ///// 提交故障转移指令
    ///// </summary>
    ///// <param name="clusterModel"></param>
    ///// <returns></returns>
    //public async Task<bool> ExcuteCommandAsync(List<ClusterCommandModel> clusterModels)
    //{
    //    try
    //    {
    //        _logger.LogWarning("开始执行故障转移指令，时间：【{0}】", DateTime.Now.ToString("yyyyMMddHHmmss_ffff"));

    //        foreach (var clusterModel in clusterModels)
    //        {
    //            var server = _connectionMultiplexer.GetServer($"{clusterModel.Address}:{clusterModel.Port}");
    //            string[] commands = clusterModel.Command.Split(' ');
    //            var result = await server.ExecuteAsync(commands[0], commands[1], commands[2]);
    //        }

    //        _logger.LogInformation("执行故障转移指令完毕，时间：【{0}】", DateTime.Now.ToString("yyyyMMddHHmmss_ffff"));
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError("执行故障转移指令时候出现异常：【{0}】", ex.ToString());
    //        return false;
    //    }
    //}
}
