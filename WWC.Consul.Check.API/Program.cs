using WWC.Consul.Check.API.HostedService;
using WWC.Consul.Check.API.IServices;
using WWC.Consul.Check.API.Model;
using WWC.Consul.Check.API.Options;
using WWC.Consul.Check.API.Services;
using StackExchange.Redis;
using WWC.Consul.Check.API.Db;
using Microsoft.EntityFrameworkCore;
using Consul;
using WWC.Consul.Check.API.HttpHelper;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<LoadGarnetNodeHostedService>();
builder.Services.AddHostedService<CheckGarnetNodeHostedService>();
builder.Services.AddHostedService<GarnetNodeLevelReductionHostedService>();
builder.Services.AddTransient<IServiceStatusService, ServiceStatusService>();
builder.Services.AddTransient(typeof(IGarnetNodeMatchService), typeof(GarnetNodeMatchService));
builder.Services.AddTransient<IGarnetFailoverSerivce, GarnetFailoverSerivce>();
builder.Services.AddTransient<IConsulServiceLoadService, ConsulServiceLoadService>();
builder.Services.AddTransient<IClusterNodeHttpService, ClusterNodeHttpService>();

builder.Services.AddDbContext<GarnetClusterDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ClusterConnection")).LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));

builder.Services.Configure<ConsulNodeNameOptions>(builder.Configuration.GetSection("ConsulNodeNameOptions"));

GarnetNodeConstant.AllNodes = builder.Configuration.GetSection("GarnetClusterNodes").Get<string[]>() ?? throw new Exception("Redis 连接字符串不存在");

builder.Services.AddSingleton<Func<string, IConnectionMultiplexer>>((addressPort) =>
{
    var options = ConfigurationOptions.Parse(addressPort);
    options.AllowAdmin = true;
    return ConnectionMultiplexer.Connect(options);
});

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var garnetNodes = builder.Configuration.GetSection("GarnetClusterNodes").Get<List<string>>() ?? throw new Exception("Redis 连接字符串不存在");
    garnetNodes.Add("password=Password");
    var options = ConfigurationOptions.Parse(string.Join(',', garnetNodes));
    options.AllowAdmin = true;
    return ConnectionMultiplexer.Connect(options);
});

var consulOptions = new ConsulPathOptions();
builder.Configuration.GetSection("ConsulPathOptions").Bind(consulOptions);

var consulClusterOptions = new ConsulClusterPathOptions();
builder.Configuration.GetSection("ConsulClusterPathOptions").Bind(consulOptions);


builder.Services.AddHttpClient("ClusterNodesAPI", client =>
{
    client.BaseAddress = new Uri(consulOptions.ClusterNodesAPI ?? "http://localhost:5193/api/");
});

//builder.Services.AddHttpClient("Node", client =>
//{
//    client.BaseAddress = new Uri(consulOptions.NodeRootPath ?? "http://127.0.0.1:8500/v1/catalog/node/");
//});

//builder.Services.AddHttpClient("Service", client =>
//{
//    client.BaseAddress = new Uri(consulOptions.ServiceRootPath ?? "http://127.0.0.1:8500/v1/health/service/");
//});

//builder.Services.AddHttpClient("RegisterService", client =>
//{
//    client.BaseAddress = new Uri(consulOptions.RegisterServicePath ?? "http://localhost:8500/v1/agent/service/register");
//});

builder.Services.AddSingleton<IConsulClient>(p =>
{
    var addresses = new List<string>
    {
        consulClusterOptions.Leader ?? "http://127.0.0.1:8500"
    };

    if (consulClusterOptions.Follow != null && consulClusterOptions.Follow.Any())
    {
        addresses.AddRange(consulClusterOptions.Follow);
    }
    else
    {
        // 添加备用地址
        addresses.Add("http://127.0.0.1:8510");
        addresses.Add("http://127.0.0.1:8520");
    }

    var consulClient = new ConsulClient(config =>
    {
        config.Address = new Uri(addresses.First());
        config.Datacenter = "dc1"; // 需要根据实际情况调整
        // 其他配置...
    });

    // 配置连接到多个地址
    foreach (var address in addresses.Skip(1))
    {
        consulClient.Agent.Services().Wait(); // 尝试连接到备用地址
    }

    return consulClient;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
