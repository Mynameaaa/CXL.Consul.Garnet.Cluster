using Newtonsoft.Json;
using WWC.Consul.Check.API.Model;

public class ConsulNodeModel
{
    [JsonProperty("Node")]
    public ConsulNode Node { get; set; }

    [JsonProperty("Services")]
    public Dictionary<string, ConsulNodeService> Services { get; set; }
}

public class ConsulNode
{
    [JsonProperty("ID")]
    public string ID { get; set; }

    [JsonProperty("Node")]
    public string NodeName { get; set; }

    [JsonProperty("Address")]
    public string Address { get; set; }

    [JsonProperty("Datacenter")]
    public string Datacenter { get; set; }

    [JsonProperty("TaggedAddresses")]
    public Dictionary<string, string> TaggedAddresses { get; set; }

    [JsonProperty("Meta")]
    public Dictionary<string, string> Meta { get; set; }

    [JsonProperty("CreateIndex")]
    public int CreateIndex { get; set; }

    [JsonProperty("ModifyIndex")]
    public int ModifyIndex { get; set; }
}

public class ConsulNodeService
{
    public string ID { get; set; }
    public string ServiceName { get; set; }
    public List<string> Tags { get; set; }
    public string Address { get; set; }
    public ConsulNodeMeta Meta { get; set; }
    public int Port { get; set; }
    public ConsulNodeWeights Weights { get; set; }
    public bool EnableTagOverride { get; set; }
    public ConsulNodeProxy Proxy { get; set; }
    public ConsulNodeConnect Connect { get; set; }
    public string PeerName { get; set; }
    public int CreateIndex { get; set; }
    public int ModifyIndex { get; set; }
}

public class ConsulNodeMeta
{
    public string grpc_tls_port { get; set; }
    public string non_voter { get; set; }
    public string raft_version { get; set; }
    public string read_replica { get; set; }
    public string serf_protocol_current { get; set; }
    public string serf_protocol_max { get; set; }
    public string serf_protocol_min { get; set; }
    public string version { get; set; }
}

public class ConsulNodeWeights
{
    public int Passing { get; set; }
    public int Warning { get; set; }
}

public class ConsulNodeProxy
{
    public string Mode { get; set; }
    public ConsulNodeMeshGateway MeshGateway { get; set; }
    public ConsulNodeExpose Expose { get; set; }
}

public class ConsulNodeMeshGateway { }
public class ConsulNodeExpose { }
public class ConsulNodeConnect { }