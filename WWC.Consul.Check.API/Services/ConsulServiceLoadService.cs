using Consul;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using WWC.Consul.Check.API.Cache;
using WWC.Consul.Check.API.Db;
using WWC.Consul.Check.API.Db.Entities;
using WWC.Consul.Check.API.HttpHelper;
using WWC.Consul.Check.API.IServices;
using WWC.Consul.Check.API.Model;

namespace WWC.Consul.Check.API.Services;

public class ConsulServiceLoadService : IConsulServiceLoadService
{
    private readonly ILogger<ConsulServiceLoadService> _logger;
    private readonly IClusterNodeHttpService _clusterNodeHttpService;

    public ConsulServiceLoadService(ILogger<ConsulServiceLoadService> logger, IClusterNodeHttpService clusterNodeHttpService)
    {
        _logger = logger;
        _clusterNodeHttpService = clusterNodeHttpService;
    }

    /// <summary>
    /// 加载需要注册到 Consul 的服务
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    public async Task<(List<AgentServiceRegistration>, IDbContextTransaction)> LoadService(string nodeName, GarnetClusterDbContext _dbContext)
    {
        IDbContextTransaction transaction = null;
        try
        {
            //var allConnecteds = await _dbContext.RedisClusterNodes
            //    .Where(p => p.ConsulNodeName.Equals(nodeName))
            //    .Select(p => new
            //    {
            //        serviceName = p.ConsulServieName,
            //        addressPort = $"{p.IP}:{p.Port}"
            //    })
            //    .ToListAsync();
            
            var allConnecteds = await _clusterNodeHttpService.QueryClusterNodeServiceIPPortByNodeName();

            DbConsulCacheService.CacheServiceName(allConnecteds.Select(p => p.ServiceName).ToList());
            DbConsulCacheService.CacheAddressPort(allConnecteds.Select(p => p.AddressPort).ToList());

            //var clusterNodes = await (from m in _dbContext.RedisClusters
            //                          where m.Name.Equals(nodeName)
            //                          join n in _dbContext.RedisClusterNodes on m.Name equals n.ConsulNodeName
            //                          where !n.IsRegister
            //                          select n).ToListAsync();

            var clusterNodes = await _clusterNodeHttpService.QueryClusterNodesByNodeName();

            if (!clusterNodes.Any())
            {
                return (null, null);
            }

            transaction = await _dbContext.Database.BeginTransactionAsync();

            foreach (var item in clusterNodes)
            {
                item.IsRegister = true;
            }
            _dbContext.UpdateRange(clusterNodes);

            string serviceNamePrefix = "garnet-";

            return (clusterNodes.Select(sl => new AgentServiceRegistration()
            {
                ID = serviceNamePrefix + sl.Port,
                Name = serviceNamePrefix + sl.Port,
                Address = sl.IP,
                Port = sl.Port,
                Tags = sl.Tags.Split(','),
                Check = new AgentServiceCheck()
                {
                    TCP = $"{sl.IP}:{sl.Port}",
                    Interval = TimeSpan.FromSeconds(sl.Interval),
                    Timeout = TimeSpan.FromSeconds(sl.TimeOut)
                },
            }).ToList(), transaction);
        }
        catch (Exception ex)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
            _logger.LogError("获取 Consul 服务出现异常：【{0}】", ex.ToString());
            throw;
        }
    }

}
