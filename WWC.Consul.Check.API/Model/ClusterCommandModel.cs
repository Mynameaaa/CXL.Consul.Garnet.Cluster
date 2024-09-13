using WWC.Consul.Check.API.Model.Enum;

namespace WWC.Consul.Check.API.Model;

public class ClusterCommandModel
{
    /// <summary>
    /// Consul 中服务名称
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// 集群编号
    /// </summary
    public string ClusterID { get; set; }

    /// <summary>
    /// 是否等待启动后执行
    /// </summary>
    public bool IsTask { get; set; }

    /// <summary>
    /// 监听地址
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// 监听端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 执行指令
    /// </summary>
    public List<string> Command { get; set; }

    /// <summary>
    /// 命令
    /// </summary>
    public List<CommandDetailModel> Commands
    {
        get
        {
            if (Command == null)
            {
                return null;
            }
            var result = new List<CommandDetailModel>();
            foreach (var command in this.Command)
            {
                var commandAndParams = command?.Split(" ");
                if (commandAndParams == null)
                {
                    continue;
                }
                if (commandAndParams.Length > 1)
                {
                    result.Add(new CommandDetailModel()
                    {
                        Command = commandAndParams[0],
                        Params = commandAndParams.Skip(1)?.ToList(),
                    });
                }
                else
                {
                    result.Add(new CommandDetailModel()
                    {
                        Command = commandAndParams[0],
                    });
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 指令状态
    /// </summary>
    public ClusterCommandState CommandState { get; set; }

}
public class CommandDetailModel
{
    public string Command { get; set; }

    public List<string> Params { get; set; }

}

//public class ClusterTask
//{
//    /// <summary>
//    /// 集群编号
//    /// </summary
//    public string ClusterID { get; set; }

//    /// <summary>
//    /// 监听地址
//    /// </summary>
//    public string Address { get; set; }

//    /// <summary>
//    /// 监听端口
//    /// </summary>
//    public int Port { get; set; }

//    /// <summary>
//    /// 主节点编号
//    /// </summary>
//    public string NewMasterID { get; set; }

//}