using Microsoft.EntityFrameworkCore;

namespace PTO_Manager.Context;

public class AppDbContext :DbContext
{
    //public DbSet<User> Users { get; set; }
   

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       /*
        *  modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        */

    }
}