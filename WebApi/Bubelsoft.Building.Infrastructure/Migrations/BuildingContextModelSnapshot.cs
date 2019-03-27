﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BubelSoft.Building.Infrastructure.Migrations
{
    [DbContext(typeof(BuildingContext))]
    partial class BuildingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Bubelsoft.Building.Infrastructure.Entities.Estimation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<int>("CompanyId");

                    b.Property<string>("Description");

                    b.Property<string>("EstimationId");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("SpecNumber");

                    b.Property<string>("Unit");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("Id");

                    b.ToTable("Estimations");
                });

            modelBuilder.Entity("Bubelsoft.Building.Infrastructure.Entities.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("ReportDate");

                    b.Property<int>("ReporterId");

                    b.Property<int>("WorkersCount");

                    b.HasKey("Id");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Bubelsoft.Building.Infrastructure.Entities.ReportQuantity", b =>
                {
                    b.Property<int>("ReportId");

                    b.Property<int>("EstimationId");

                    b.Property<decimal>("Quantity");

                    b.HasKey("ReportId", "EstimationId");

                    b.HasIndex("EstimationId");

                    b.ToTable("ReportQuantity");
                });

            modelBuilder.Entity("Bubelsoft.Building.Infrastructure.Entities.ReportQuantity", b =>
                {
                    b.HasOne("Bubelsoft.Building.Infrastructure.Entities.Estimation", "Estimation")
                        .WithMany("Quantities")
                        .HasForeignKey("EstimationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Bubelsoft.Building.Infrastructure.Entities.Report")
                        .WithMany("Quantities")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
