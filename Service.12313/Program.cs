using Consul;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var consulServers = new[]
{
    "http://192.168.1.152:8500",
    "http://192.168.1.152:8510",
    "http://192.168.1.152:8520"
};

builder.Services.AddSingleton<IConsulClient>(factory =>
{
    ConsulClient consulClient = null;

    foreach (var server in consulServers)
    {
        try
        {
            consulClient = new ConsulClient(config =>
            {
                config.Address = new Uri(server);
            });

            // 测试连接
            var services = consulClient.Agent.Services().Result;
            if (services != null)
            {
                Console.WriteLine($"Successfully connected to Consul server at {server}");
                break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to connect to Consul server at {server}: {ex.Message}");
            consulClient = null;
        }
    }

    if (consulClient == null)
    {
        throw new Exception("Unable to connect to any Consul servers.");
    }
    return consulClient;
});

// 添加健康检查服务
builder.Services.AddHealthChecks();

var app = builder.Build();

//Consul 健康检查
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true, // 对所有健康检查启用此端点
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                exception = e.Value.Exception?.Message,
                duration = e.Value.Duration.ToString()
            })
        });
        await context.Response.WriteAsync(result);
    }
});

var lifetime = app.Lifetime;
var consulClient = app.Services.GetRequiredService<IConsulClient>();

lifetime.ApplicationStarted.Register(() =>
{
    // 获取应用的地址和端口
    var address = app.Urls.First(); // 默认使用第一个地址
    var uri = new Uri(address);
    
    var registration = new AgentServiceRegistration()
    {
        ID = "ASPNETCore-Service-12313-ID", // 服务实例唯一 ID
        Name = "ASPNETCore-Service-12313-Name",                 // 服务名称
        Address = uri.Host,                 // 服务 IP 地址
        Port = uri.Port,                    // 服务端口
        Tags = new[] { "TestAPIService", "api" },// 服务标签
        Check = new AgentServiceCheck
        {
            HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}/health", // 健康检查的 HTTP 地址
            Interval = TimeSpan.FromSeconds(10),                   // 健康检查的间隔时间
            Timeout = TimeSpan.FromSeconds(5),                     // 健康检查超时时间
            DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1) // 在健康检查失败后取消注册服务的时间
        }
    };

    consulClient.Agent.ServiceRegister(registration).Wait();
});

lifetime.ApplicationStopping.Register(() =>
{
    // 在应用停止时注销服务
    consulClient.Agent.ServiceDeregister("ASPNETCore-Service-12313-ID").Wait();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
