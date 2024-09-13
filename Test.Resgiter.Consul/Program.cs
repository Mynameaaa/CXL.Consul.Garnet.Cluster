using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConsulClient>(factory =>
{
    var addresses = new List<string>
    {
        "http://127.0.0.1:8500",
        "http://127.0.0.1:8510",
        "http://127.0.0.1:8520"
    };

    var consulClient = new ConsulClient(config =>
    {
        config.Address = new Uri(addresses.First());
        config.Datacenter = "dc1";
    });

    foreach (var address in addresses.Skip(1))
    {
        consulClient.Agent.Services().Wait();
    }

    return consulClient;
});

var app = builder.Build();

var consulClient = app.Services.GetRequiredService<IConsulClient>();


app.Run();
