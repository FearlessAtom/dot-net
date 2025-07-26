using ClassLibrary.Data.Models;
using CLassLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<ATM> ATMs { get; set; }
    public DbSet<Company> Companies { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-6GKM0QJ;Initial Catalog=DotNetLab1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
}