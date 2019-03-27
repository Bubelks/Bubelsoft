﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BubelSoft.Building.Infrastructure.Migrations
{
    [DbContext(typeof(BuildingContext))]
    [Migration("20180116203505_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BuildingContext.Entities.Estimation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("EstimationId");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("SpecNumber");

                    b.Property<string>("Unit");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("BuildingContext.Entities.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int?>("EstimationId");

                    b.Property<DateTime>("ReportDate");

                    b.Property<int?>("ReportId");

                    b.Property<int>("ReporterId");

                    b.Property<int>("WorkersCount");

                    b.HasKey("Id");

                    b.HasIndex("EstimationId");

                    b.HasIndex("ReportId");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("BuildingContext.Entities.Report", b =>
                {
                    b.HasOne("BuildingContext.Entities.Estimation")
                        .WithMany("Reports")
                        .HasForeignKey("EstimationId");

                    b.HasOne("BuildingContext.Entities.Report")
                        .WithMany("Quantities")
                        .HasForeignKey("ReportId");
                });
#pragma warning restore 612, 618
        }
    }
}
