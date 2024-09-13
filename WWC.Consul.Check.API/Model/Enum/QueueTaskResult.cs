namespace WWC.Consul.Check.API.Model.Enum;

public enum QueueTaskResult
{
    /// <summary>
    /// 成功添加
    /// </summary>
    Added = 1,

    /// <summary>
    /// 已经存在
    /// </summary>
    Exist = 10,

    /// <summary>
    /// 异常的类型
    /// </summary>
    TypeError = 20,

    /// <summary>
    /// 失败
    /// </summary>
    Fail = 30,
}
