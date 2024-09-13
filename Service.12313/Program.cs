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

            // ��������
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

// ��ӽ���������
builder.Services.AddHealthChecks();

var app = builder.Build();

//Consul �������
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true, // �����н���������ô˶˵�
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
    // ��ȡӦ�õĵ�ַ�Ͷ˿�
    var address = app.Urls.First(); // Ĭ��ʹ�õ�һ����ַ
    var uri = new Uri(address);
    
    var registration = new AgentServiceRegistration()
    {
        ID = "ASPNETCore-Service-12313-ID", // ����ʵ��Ψһ ID
        Name = "ASPNETCore-Service-12313-Name",                 // ��������
        Address = uri.Host,                 // ���� IP ��ַ
        Port = uri.Port,                    // ����˿�
        Tags = new[] { "TestAPIService", "api" },// �����ǩ
        Check = new AgentServiceCheck
        {
            HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}/health", // �������� HTTP ��ַ
            Interval = TimeSpan.FromSeconds(10),                   // �������ļ��ʱ��
            Timeout = TimeSpan.FromSeconds(5),                     // ������鳬ʱʱ��
            DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1) // �ڽ������ʧ�ܺ�ȡ��ע������ʱ��
        }
    };

    consulClient.Agent.ServiceRegister(registration).Wait();
});

lifetime.ApplicationStopping.Register(() =>
{
    // ��Ӧ��ֹͣʱע������
    consulClient.Agent.ServiceDeregister("ASPNETCore-Service-12313-ID").Wait();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
