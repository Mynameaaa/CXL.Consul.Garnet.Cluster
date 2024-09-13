namespace WWC.Consul.Check.API.Model;

public class JsonClusterResult<T>
{
    public bool Success { get; set; }

    public int Code { get; set; }

    public string Message { get; set; }

    public List<T> Result { get; set; }

}
