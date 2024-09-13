using Newtonsoft.Json;

public class ConsulServiceTaggedAddresses
{
    public string lan { get; set; }
    public string lan_ipv4 { get; set; }
    public string wan { get; set; }
    public string wan_ipv4 { get; set; }
}

public class ConsulServiceNode
{
    public string ID { get; set; }
    public string Node { get; set; }
    public string Address { get; set; }
    public string Datacenter { get; set; }
    public ConsulServiceTaggedAddresses TaggedAddresses { get; set; }
    public Dictionary<string, string> Meta { get; set; }
    public int CreateIndex { get; set; }
    public int ModifyIndex { get; set; }
}

public class ConsulServiceWeights
{
    public int Passing { get; set; }
    public int Warning { get; set; }
}

public class ConsulServiceMeta
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

public class ConsulServiceProxy
{
    public string Mode { get; set; }
    public object MeshGateway { get; set; }
    public object Expose { get; set; }
}

public class ConsulServiceService
{
    public string ID { get; set; }
    public string Service { get; set; }
    public List<string> Tags { get; set; }
    public string Address { get; set; }
    public ConsulServiceMeta Meta { get; set; }
    public int Port { get; set; }
    public ConsulServiceWeights Weights { get; set; }
    public bool EnableTagOverride { get; set; }
    public ConsulServiceProxy Proxy { get; set; }
    public object Connect { get; set; }
    public string PeerName { get; set; }
    public int CreateIndex { get; set; }
    public int ModifyIndex { get; set; }
}

public class ConsulServiceCheck
{
    public string Node { get; set; }
    public string CheckID { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
    public string Output { get; set; }
    public string ServiceID { get; set; }
    public string ServiceName { get; set; }
    public List<string> ServiceTags { get; set; }
    public string Type { get; set; }
    public string Interval { get; set; }
    public string Timeout { get; set; }
    public int ExposedPort { get; set; }
    public Dictionary<string, string> Definition { get; set; }
    public int CreateIndex { get; set; }
    public int ModifyIndex { get; set; }
}

public class ConsulServiceConsulNodeInfo
{
    public ConsulServiceNode Node { get; set; }
    public ConsulServiceService Service { get; set; }
    public List<ConsulServiceCheck> Checks { get; set; }
}