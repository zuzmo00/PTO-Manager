﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PTO_Manager.Entities;

namespace PTO_Manager.Context;

public class AppDbContext :DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Department> Department { get; set; }
    public DbSet<RemainingDay> Remaining { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Admin> Administrators { get; set; }
    public DbSet<SpecialDays> SpecialDays { get; set; }
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
       modelBuilder.Entity<Admin>()
            .HasOne(u => u.Szemely)
            .WithMany(s => s.Ugyintezo)
            .HasForeignKey(u => u.SzemelyId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}