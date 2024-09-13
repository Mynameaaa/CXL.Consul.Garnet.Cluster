using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RequetConsul.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RequestController : ControllerBase
{
    private readonly IConsulClient _consulClient;

    public RequestController(IConsulClient consulClient)
    {
        _consulClient = consulClient;
    }

    [HttpGet]
    public async Task<string> RequestConsul()
    {
        var addressPort = await GetServiceEndpointAsync("ASPNETCore-Service-12312-Name", "api");
        await Console.Out.WriteLineAsync("请求地址：" + addressPort);
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://" + addressPort);
            var response = await client.GetAsync("/api/Consul");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            await Console.Out.WriteLineAsync("响应内容：" + content);

            return content;
        }
    }

    private async Task<string> GetServiceEndpointAsync(string serviceName, string versionTag)
    {
        var services = await _consulClient.Health.Service(serviceName, versionTag, true);
        var instances = services.Response.Select(s => s.Service).ToList();

        if (!instances.Any())
            throw new Exception($"No services available for version: {versionTag}");

        // Load balancing strategy
        var random = new Random();
        var selectedInstance = instances[random.Next(instances.Count)];

        return $"{selectedInstance.Address}:{selectedInstance.Port}";
    }

}
