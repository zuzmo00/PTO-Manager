using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PTO_Manager.Entities;

namespace PTO_Manager.Context;

public class AppDbContext :DbContext
{
    public DbSet<Szemelyek> Szemelyek { get; set; }
    public DbSet<Reszleg> Reszleg { get; set; }
    public DbSet<FennmaradoNapok> FennmaradoNapok { get; set; }
    public DbSet<Kerelmek> Kerelmek { get; set; }
    public DbSet<Ugyintezok> Ugyintezok { get; set; }
    public DbSet<KulonlegesNapok> KulonlegesNapok { get; set; }
    public DbSet<Log> Log { get; set; }
    public DbSet<Preferenciak> Preferenciak { get; set; }


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
       modelBuilder.Entity<Ugyintezok>()
            .HasOne(u => u.Szemely)
            .WithMany(s => s.Ugyintezo)
            .HasForeignKey(u => u.SzemelyId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}