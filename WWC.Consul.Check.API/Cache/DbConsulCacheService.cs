namespace WWC.Consul.Check.API.Cache;

public class DbConsulCacheService
{
    private static List<string> ServiceNameList { get; set; } = new List<string>();

    public static List<string> ServiceAddressPort { get; set; } = new List<string>();

    public static void CacheServiceName(List<string> serviceNames)
    {
        if (serviceNames == null || !serviceNames.Any())
        {
            return;
        }
        ServiceNameList = serviceNames;
    }

    public static List<string> GetServiceNames()
    {
        return ServiceNameList;
    }

    public static void CacheAddressPort(List<string> serviceNames)
    {
        if (serviceNames == null || !serviceNames.Any())
        {
            return;
        }
        ServiceAddressPort = serviceNames;
    }

    public static List<string> GetAddressPort()
    {
        return ServiceAddressPort;
    }

}
