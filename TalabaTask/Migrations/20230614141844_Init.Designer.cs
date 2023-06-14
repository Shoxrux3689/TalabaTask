﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TalabaTask.Context;

#nullable disable

namespace TalabaTask.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230614141844_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TalabaTask.Entities.Gradiate", b =>
                {
                    b.Property<long>("StudentId")
                        .HasColumnType("bigint");

                    b.Property<long>("ScienceId")
                        .HasColumnType("bigint");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.HasKey("StudentId", "ScienceId");

                    b.HasIndex("ScienceId");

                    b.ToTable("Gradiates");
                });

            modelBuilder.Entity("TalabaTask.Entities.Science", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TeacherId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("Sciences");
                });

            modelBuilder.Entity("TalabaTask.Entities.Student", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StudentRegNumber")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("TalabaTask.Entities.StudentSciences", b =>
                {
                    b.Property<long>("StudentId")
                        .HasColumnType("bigint");

                    b.Property<long>("ScienceId")
                        .HasColumnType("bigint");

                    b.HasKey("StudentId", "ScienceId");

                    b.HasIndex("ScienceId");

                    b.ToTable("StudentSciences");
                });

            modelBuilder.Entity("TalabaTask.Entities.Teacher", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("TalabaTask.Entities.Gradiate", b =>
                {
                    b.HasOne("TalabaTask.Entities.Science", "Science")
                        .WithMany("Gradiates")
                        .HasForeignKey("ScienceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TalabaTask.Entities.Student", "Student")
                        .WithMany("Gradiates")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Science");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("TalabaTask.Entities.Science", b =>
                {
                    b.HasOne("TalabaTask.Entities.Teacher", "Teacher")
                        .WithMany("Sciences")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("TalabaTask.Entities.StudentSciences", b =>
                {
                    b.HasOne("TalabaTask.Entities.Science", "Science")
                        .WithMany("StudentSciences")
                        .HasForeignKey("ScienceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TalabaTask.Entities.Student", "Student")
                        .WithMany("StudentSciences")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Science");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("TalabaTask.Entities.Science", b =>
                {
                    b.Navigation("Gradiates");

                    b.Navigation("StudentSciences");
                });

            modelBuilder.Entity("TalabaTask.Entities.Student", b =>
                {
                    b.Navigation("Gradiates");

                    b.Navigation("StudentSciences");
                });

            modelBuilder.Entity("TalabaTask.Entities.Teacher", b =>
                {
                    b.Navigation("Sciences");
                });
#pragma warning restore 612, 618
        }
    }
}
