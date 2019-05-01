﻿// <auto-generated />
using System;
using BubelSoft.Core.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BubelSoft.Core.Infrastructure.Migrations
{
    [DbContext(typeof(MainContext))]
    partial class MainContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BubelSoft.Core.Infrastructure.Database.Entities.Building", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConnectionString");

                    b.Property<bool?>("IsReady")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("BubelSoft.Core.Infrastructure.Database.Entities.BuildingCompany", b =>
                {
                    b.Property<int>("BuildingId");

                    b.Property<int>("CompanyId");

                    b.Property<int>("ContractType");

                    b.HasKey("BuildingId", "CompanyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("BuildingCompany");
                });

            modelBuilder.Entity("BubelSoft.Core.Infrastructure.Database.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City");

                    b.Property<string>("EMail");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Number");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PlaceNumber");

                    b.Property<string>("PostCode");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("BubelSoft.Core.Infrastructure.Database.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CompanyId");

                    b.Property<int>("CompanyRole");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BubelSoft.Core.Infrastructure.Database.Entities.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("BuildingId");

                    b.Property<int>("UserBuildingRole");

                    b.Property<int>("CompanyId");

                    b.HasKey("UserId", "BuildingId", "UserBuildingRole");

                    b.HasIndex("BuildingId", "CompanyId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("BubelSoft.Core.Infrastructure.Database.Entities.BuildingCompany", b =>
                {
                    b.HasOne("BubelSoft.Core.Infrastructure.Database.Entities.Building", "Building")
                        .WithMany("Companies")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BubelSoft.Core.Infrastructure.Database.Entities.Company", "Company")
                        .WithMany("Buildings")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BubelSoft.Core.Infrastructure.Database.Entities.User", b =>
                {
                    b.HasOne("BubelSoft.Core.Infrastructure.Database.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("BubelSoft.Core.Infrastructure.Database.Entities.UserRole", b =>
                {
                    b.HasOne("BubelSoft.Core.Infrastructure.Database.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BubelSoft.Core.Infrastructure.Database.Entities.BuildingCompany", "Building")
                        .WithMany("Users")
                        .HasForeignKey("BuildingId", "CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
