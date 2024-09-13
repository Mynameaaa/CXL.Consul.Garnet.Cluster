using Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var consulServers = new[]
{
    "http://127.0.0.1:8500",
    "http://127.0.0.1:8510",
    "http://127.0.0.1:8520"
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

            // ≤‚ ‘¡¨Ω”
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
