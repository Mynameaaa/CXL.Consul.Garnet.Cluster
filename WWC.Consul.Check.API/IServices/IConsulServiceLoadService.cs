using Consul;
using Microsoft.EntityFrameworkCore.Storage;
using WWC.Consul.Check.API.Db;
using WWC.Consul.Check.API.Model;

namespace WWC.Consul.Check.API.IServices;

public interface IConsulServiceLoadService
{

    /// <summary>
    /// 加载需要注册到 Consul 的服务
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    Task<(List<AgentServiceRegistration>, IDbContextTransaction)> LoadService(string nodeName, GarnetClusterDbContext _dbContext);

}
