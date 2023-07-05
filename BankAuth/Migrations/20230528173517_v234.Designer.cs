﻿// <auto-generated />
using BankAuth.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankAuth.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230528173517_v234")]
    partial class v234
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BankAuth.Models.CustomerAccountInfo", b =>
                {
                    b.Property<int>("CustomerAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerAccountId"));

                    b.Property<string>("AadharNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Addresss")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DOB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OccupationAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OccupationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PanNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkExp")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerAccountId");

                    b.ToTable("customer_accountinfo", (string)null);
                });

            modelBuilder.Entity("BankAuth.Models.CustomerAccountInfoo", b =>
                {
                    b.Property<int>("CustomeredId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomeredId"));

                    b.Property<string>("AadharNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccountNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Addresss")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DOB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmpType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OccupationAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OccupationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PanNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkExp")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomeredId");

                    b.ToTable("customer_accountinfoo", (string)null);
                });

            modelBuilder.Entity("BankAuth.Models.UserReg", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("AccountNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AuthToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("user_cred", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
