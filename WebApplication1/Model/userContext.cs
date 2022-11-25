using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApplication1.Model;

public class userContext : DbContext
    {
        public userContext(DbContextOptions  options): base(options) {}
     public DbSet < user > user{ get; set;}
   public DbSet<comment> comment { get; set; }
   public  DbSet<article> article { get; set; }
    public DbSet<follower> follower { get; set; }
    public DbSet<favorite> favorite { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
    {
        if (!optionsbuilder.IsConfigured)
        {
            optionsbuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=user;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
          

        }
        base.OnConfiguring(optionsbuilder); 
    }
   
}
