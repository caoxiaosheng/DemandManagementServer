﻿// <auto-generated />
using DemandManagementServer.DAL;
using DemandManagementServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DemandManagementServer.Migrations
{
    [DbContext(typeof(DemandDbContext))]
    [Migration("20180204160643_releaseData")]
    partial class releaseData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DemandManagementServer.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("CustomerPriority");

                    b.Property<int>("CustomerState");

                    b.Property<int>("CustomerType");

                    b.Property<string>("Name");

                    b.Property<string>("Remarks");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("DemandManagementServer.Models.Demand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("CustomerId");

                    b.Property<string>("DemandCode");

                    b.Property<string>("DemandDetail");

                    b.Property<int>("DemandPhase");

                    b.Property<int>("DemandType");

                    b.Property<string>("Remarks");

                    b.Property<int?>("SoftwareVersionId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("SoftwareVersionId");

                    b.HasIndex("UserId");

                    b.ToTable("Demands");
                });

            modelBuilder.Entity("DemandManagementServer.Models.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Icon");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Remarks");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("DemandManagementServer.Models.OperationRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTime");

                    b.Property<int>("DemandId");

                    b.Property<int>("OperationType");

                    b.Property<string>("RecordDetail");

                    b.HasKey("Id");

                    b.HasIndex("DemandId");

                    b.ToTable("OperationRecords");
                });

            modelBuilder.Entity("DemandManagementServer.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("Name");

                    b.Property<string>("Remarks");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DemandManagementServer.Models.RoleMenu", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("MenuId");

                    b.HasKey("RoleId", "MenuId");

                    b.HasIndex("MenuId");

                    b.ToTable("RoleMenus");
                });

            modelBuilder.Entity("DemandManagementServer.Models.SoftwareVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ExpectedEndDate");

                    b.Property<DateTime>("ExpectedReleaseDate");

                    b.Property<DateTime>("ExpectedStartDate");

                    b.Property<int>("IsDeleted");

                    b.Property<DateTime?>("ReleaseDate");

                    b.Property<string>("Remarks");

                    b.Property<string>("VersionName");

                    b.Property<int>("VersionProgress");

                    b.HasKey("Id");

                    b.HasIndex("VersionName")
                        .IsUnique()
                        .HasFilter("[VersionName] IS NOT NULL");

                    b.ToTable("SoftwareVersions");
                });

            modelBuilder.Entity("DemandManagementServer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("Email");

                    b.Property<int>("IsDeleted");

                    b.Property<DateTime>("LastLoginTime");

                    b.Property<string>("MobileNumber");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Remarks");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DemandManagementServer.Models.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("DemandManagementServer.Models.Demand", b =>
                {
                    b.HasOne("DemandManagementServer.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DemandManagementServer.Models.SoftwareVersion", "SoftwareVersion")
                        .WithMany()
                        .HasForeignKey("SoftwareVersionId");

                    b.HasOne("DemandManagementServer.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DemandManagementServer.Models.OperationRecord", b =>
                {
                    b.HasOne("DemandManagementServer.Models.Demand", "Demand")
                        .WithMany("OperationRecords")
                        .HasForeignKey("DemandId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DemandManagementServer.Models.RoleMenu", b =>
                {
                    b.HasOne("DemandManagementServer.Models.Menu", "Menu")
                        .WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DemandManagementServer.Models.Role", "Role")
                        .WithMany("RoleMenus")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DemandManagementServer.Models.UserRole", b =>
                {
                    b.HasOne("DemandManagementServer.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DemandManagementServer.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
