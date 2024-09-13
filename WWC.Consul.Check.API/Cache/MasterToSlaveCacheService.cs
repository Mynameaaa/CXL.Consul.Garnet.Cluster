using WWC.Consul.Check.API.Model;
using WWC.Consul.Check.API.Model.Enum;

namespace WWC.Consul.Check.API.Cache;

public class MasterToSlaveCacheService
{
    private static List<ClusterCommandModel> MasterToSlaveCache = new List<ClusterCommandModel>();

    private static LoggerFactory loggerFactory = new LoggerFactory();

    public static QueueTaskResult SetQueueValue(ClusterCommandModel model)
    {
        try
        {
            if (model == null || !model.IsTask)
            {
                return QueueTaskResult.TypeError;
            }

            var any = MasterToSlaveCache?.Any(a => a.ClusterID == model.ClusterID && a.Command == model.Command);

            if (any.HasValue && any.Value)
            {
                return QueueTaskResult.Exist;
            }

            MasterToSlaveCache?.Add(model);
            return QueueTaskResult.Added;
        }
        catch (Exception ex)
        {
            CreateLogger().Log(LogLevel.Error, $"缓存命令出现异常：{ex}");
            return QueueTaskResult.Fail;
        }
    }

    public static List<string>? GroupServiceName()
    {
        return MasterToSlaveCache?.GroupBy(p => p.ServiceName)?.Select(p => p.Key)?.ToList();
    }

    /// <summary>
    /// 消费命令行
    /// </summary>
    /// <param name="serviceName"></param>
    public static ClusterCommandModel? ConsumerCommand(string serviceName)
    {
        return MasterToSlaveCache?.FirstOrDefault(p => p.ServiceName == serviceName);
    }

    /// <summary>
    /// 消费命令行
    /// </summary>
    /// <param name="ServiceName"></param>
    public static List<ClusterCommandModel>? ConsumerCommand(List<string> serviceNameList)
    {
        return MasterToSlaveCache?.Where(p => serviceNameList.Contains(p.ServiceName)).ToList();
    }

    private static ILogger CreateLogger()
    {
        return loggerFactory.CreateLogger<MasterToSlaveCacheService>();
    }

}
