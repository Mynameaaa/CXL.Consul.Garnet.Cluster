using Consul;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using WWC.Consul.Check.API.Db.Entities;
using WWC.Consul.Check.API.Model;
using WWC.Consul.Check.API.Options;

namespace WWC.Consul.Check.API.HttpHelper;

public class ClusterNodeHttpService : IClusterNodeHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ClusterNodeHttpService> _logger;
    private readonly ConsulNodeNameOptions _consulNodeNameOptions; 

    public ClusterNodeHttpService(IHttpClientFactory httpClientFactory
        , ILogger<ClusterNodeHttpService> logger
        , IOptions<ConsulNodeNameOptions> _options)
    {
        _consulNodeNameOptions = _options.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// 远程获取节点信息
    /// </summary>
    /// <returns></returns>
    public async Task<List<RedisClusterNode>> QueryClusterNodesByNodeName()
    {
        var client = _httpClientFactory.CreateClient("ClusterNodesAPI");

        try
        {
            client.DefaultRequestHeaders.Add("HostName", "GarnetCluster");
            client.DefaultRequestHeaders.Add("HostSecret", "BQpSRQM88hC8yNBjb7EhFkihwEpmcDeh");

            var response = await client.GetAsync($"GarnetConnected/QueryClusterNodesByNodeName?nodeName={_consulNodeNameOptions.NodeName}");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var jsonData = JsonConvert.DeserializeObject<JsonClusterResult<RedisClusterNode>>(responseContent);

            if (jsonData == null)
            {
                throw new Exception("Http 异常");
            }

            if (!jsonData.Success)
            {
                throw new Exception("未获取到任何服务");
            }

            return jsonData.Result;
        }
        catch (Exception ex)
        {
            _logger.LogError("远程获取服务出现异常：【{0}】", ex.ToString());
            throw;
        }

    }

    /// <summary>
    /// 获取集群节点服务信息
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    public async Task<List<ClusterService>> QueryClusterNodeServiceIPPortByNodeName()
    {
        var client = _httpClientFactory.CreateClient("ClusterNodesAPI");

        try
        {
            client.DefaultRequestHeaders.Add("HostName", "GarnetCluster");
            client.DefaultRequestHeaders.Add("HostSecret", "BQpSRQM88hC8yNBjb7EhFkihwEpmcDeh");

            var response = await client.GetAsync($"GarnetConnected/QueryClusterNodeServiceIPPortByNodeName?nodeName={_consulNodeNameOptions.NodeName}");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var jsonData = JsonConvert.DeserializeObject<JsonClusterResult<ClusterService>>(responseContent);

            if (jsonData == null)
            {
                throw new Exception("Http 异常");
            }

            return jsonData.Result;
        }
        catch (Exception ex)
        {
            _logger.LogError("远程获取服务出现异常：【{0}】", ex.ToString());
            throw;
        }
    }

}
