﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WebApi.Database;
using WebApi.Database.Entities;
using WebApi.Domain.Models;

namespace WebApi.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20180117202945_connString")]
    partial class connString
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApi.Database.Entities.Building", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConnectionString");

                    b.Property<bool>("IsReady");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("WebApi.Database.Entities.BuildingCompany", b =>
                {
                    b.Property<int>("BuildingId");

                    b.Property<int>("CompanyId");

                    b.Property<int>("ContractType");

                    b.HasKey("BuildingId", "CompanyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("BuildingCompany");
                });

            modelBuilder.Entity("WebApi.Database.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("EMail");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Nip");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PlaceNumber");

                    b.Property<string>("PostCode");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("WebApi.Database.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CompanyId");

                    b.Property<int>("CompanyRole");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApi.Database.Entities.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("BuildingId");

                    b.Property<int>("UserBuildingRole");

                    b.Property<int>("CompanyId");

                    b.HasKey("UserId", "BuildingId", "UserBuildingRole");

                    b.HasIndex("BuildingId", "CompanyId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("WebApi.Database.Entities.BuildingCompany", b =>
                {
                    b.HasOne("WebApi.Database.Entities.Building", "Building")
                        .WithMany("Companies")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Database.Entities.Company", "Company")
                        .WithMany("Buildings")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApi.Database.Entities.User", b =>
                {
                    b.HasOne("WebApi.Database.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("WebApi.Database.Entities.UserRole", b =>
                {
                    b.HasOne("WebApi.Database.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WebApi.Database.Entities.BuildingCompany", "Building")
                        .WithMany("Users")
                        .HasForeignKey("BuildingId", "CompanyId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
