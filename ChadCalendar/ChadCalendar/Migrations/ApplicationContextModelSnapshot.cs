﻿// <auto-generated />
using System;
using ChadCalendar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChadCalendar.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("ChadCalendar.Models.Duty", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Accessed")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Frequency")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Multiplier")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NRepetitions")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Duties");
                });

            modelBuilder.Entity("ChadCalendar.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<int>("RemindEveryNDays")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeZone")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WorkingHoursFrom")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WorkingHoursTo")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChadCalendar.Models.Event", b =>
                {
                    b.HasBaseType("ChadCalendar.Models.Duty");

                    b.Property<DateTime>("FinishesAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("RemindNMinutesBefore")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartsAt")
                        .HasColumnType("TEXT");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("ChadCalendar.Models.Project", b =>
                {
                    b.HasBaseType("ChadCalendar.Models.Duty");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IconNumber")
                        .HasColumnType("INTEGER");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ChadCalendar.Models.Task", b =>
                {
                    b.HasBaseType("ChadCalendar.Models.Duty");

                    b.Property<bool?>("AllowedToDistribute")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("HoursTakes")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("IsCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MaxPerDay")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PredecessorFK")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SuccessorFK")
                        .HasColumnType("INTEGER");

                    b.HasIndex("PredecessorFK")
                        .IsUnique();

                    b.HasIndex("ProjectId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("ChadCalendar.Models.Duty", b =>
                {
                    b.HasOne("ChadCalendar.Models.User", "User")
                        .WithMany("Duties")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChadCalendar.Models.Event", b =>
                {
                    b.HasOne("ChadCalendar.Models.Duty", null)
                        .WithOne()
                        .HasForeignKey("ChadCalendar.Models.Event", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ChadCalendar.Models.Project", b =>
                {
                    b.HasOne("ChadCalendar.Models.Duty", null)
                        .WithOne()
                        .HasForeignKey("ChadCalendar.Models.Project", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ChadCalendar.Models.Task", b =>
                {
                    b.HasOne("ChadCalendar.Models.Duty", null)
                        .WithOne()
                        .HasForeignKey("ChadCalendar.Models.Task", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChadCalendar.Models.Task", "Successor")
                        .WithOne("Predecessor")
                        .HasForeignKey("ChadCalendar.Models.Task", "PredecessorFK");

                    b.HasOne("ChadCalendar.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.Navigation("Project");

                    b.Navigation("Successor");
                });

            modelBuilder.Entity("ChadCalendar.Models.User", b =>
                {
                    b.Navigation("Duties");
                });

            modelBuilder.Entity("ChadCalendar.Models.Task", b =>
                {
                    b.Navigation("Predecessor");
                });
#pragma warning restore 612, 618
        }
    }
}
