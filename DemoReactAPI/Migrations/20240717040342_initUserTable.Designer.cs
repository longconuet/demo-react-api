﻿// <auto-generated />
using System;
using DemoReactAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemoReactAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240717040342_initUserTable")]
    partial class initUserTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DemoReactAPI.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("06d7fdfa-f9cc-450f-a58b-9ce24892a6b4"),
                            CreatedAt = new DateTime(2024, 7, 17, 11, 3, 40, 626, DateTimeKind.Local).AddTicks(4315),
                            CreatedBy = new Guid("00000000-0000-0000-0000-000000000000"),
                            Email = "nice231096@gmail.com",
                            FullName = "Nguyen Thanh Long",
                            Password = "$2a$13$Ppz2bresYRDzQ1DmVwOQp.mBMsOUZVivWCA6lGPy2S.pUoaU.618O",
                            Phone = "0348523140",
                            Role = 0,
                            Status = 1,
                            Username = "admin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
