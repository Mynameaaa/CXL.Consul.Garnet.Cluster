namespace WWC.Consul.Check.API.Model.Enum;

public enum ClusterCommandState
{
    /// <summary>
    /// 未执行
    /// </summary>
    Unexecuted = 1,

    /// <summary>
    /// 已执行
    /// </summary>
    Executed = 10,

    /// <summary>
    /// 执行失败
    /// </summary>
    ExecutionFail = 20,
}
