using Microsoft.EntityFrameworkCore;
using ClassLibrary.Data.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<RecentFile> RecentFiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlServer("Data Source=DESKTOP-6GKM0QJ;Initial Catalog=DotNetLab3;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RecentFile>().
            Property(e => e.EditingDate).
            HasDefaultValue(DateTime.Now);
    }   
}
