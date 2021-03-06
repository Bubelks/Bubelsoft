﻿using BubelSoft.Core.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BubelSoft.Core.Infrastructure.Database
{
    public class MainContext: DbContext
    {
        public MainContext(DbContextOptions options): base(options)
        { }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<BuildingCompany>()
                .HasKey(bc => new {bc.BuildingId, bc.CompanyId});

            modelBuilder.Entity<BuildingCompany>()
                .HasOne(bc => bc.Building)
                .WithMany(b => b.Companies)
                .HasForeignKey(bc => bc.BuildingId);

            modelBuilder.Entity<BuildingCompany>()
                .HasOne(bc => bc.Company)
                .WithMany(b => b.Buildings)
                .HasForeignKey(bc => bc.CompanyId);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur=> new {ur.UserId, ur.BuildingId, ur.UserBuildingRole});

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Building)
                .WithMany(b => b.Users)
                .HasForeignKey(ur => new {ur.BuildingId, ur.CompanyId})
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Building>()
                .Property(b => b.IsReady)
                .HasDefaultValue(false);
        }
    }
}