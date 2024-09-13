using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WWC.Consul.Check.API.Db.Entities;

namespace WWC.Consul.Check.API.Db;

public class GarnetClusterDbContext : DbContext
{
    /// <summary>
    /// 集群
    /// </summary>
    public virtual DbSet<RedisCluster> RedisClusters { get; set; }

    /// <summary>
    /// 集群指令
    /// </summary>
    public virtual DbSet<RedisClusterCommand> RedisClusterCommands { get; set; }

    /// <summary>
    /// 集群节点
    /// </summary>
    public virtual DbSet<RedisClusterNode> RedisClusterNodes { get; set; }

    public GarnetClusterDbContext(DbContextOptions<GarnetClusterDbContext> options) : base (options) 
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

}
