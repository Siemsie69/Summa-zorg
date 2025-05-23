﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Zorgdossier.Databases;

#nullable disable

namespace Zorgdossier.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250220105336_Question")]
    partial class Question
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Zorgdossier.Models.BasicInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complaint")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("DossierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DossierId")
                        .IsUnique();

                    b.ToTable("BasicInformation");
                });

            modelBuilder.Entity("Zorgdossier.Models.ComplaintsSymptoms", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ComplaintsSymptomsSummary")
                        .IsRequired()
                        .HasMaxLength(720)
                        .HasColumnType("TEXT");

                    b.Property<int>("DossierId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DossierId")
                        .IsUnique();

                    b.ToTable("ComplaintsSymptoms");
                });

            modelBuilder.Entity("Zorgdossier.Models.ContactAdvice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Advice")
                        .IsRequired()
                        .HasMaxLength(720)
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactAdviceText")
                        .HasMaxLength(320)
                        .HasColumnType("TEXT");

                    b.Property<int>("DossierId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DossierId")
                        .IsUnique();

                    b.ToTable("ContactAdvice");
                });

            modelBuilder.Entity("Zorgdossier.Models.Dossier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("StudentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Dossier");
                });

            modelBuilder.Entity("Zorgdossier.Models.Organ", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DossierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Organs")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DossierId");

                    b.ToTable("Organ");
                });

            modelBuilder.Entity("Zorgdossier.Models.Phone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DossierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PhoneSummary")
                        .IsRequired()
                        .HasMaxLength(720)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DossierId")
                        .IsUnique();

                    b.ToTable("Phone");
                });

            modelBuilder.Entity("Zorgdossier.Models.Policy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DossierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PolicyChoice")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PolicyDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("TriageCriteria")
                        .HasMaxLength(320)
                        .HasColumnType("TEXT");

                    b.Property<string>("Urgency")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DossierId")
                        .IsUnique();

                    b.ToTable("Policy");
                });

            modelBuilder.Entity("Zorgdossier.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DossierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("QuestionSummary")
                        .IsRequired()
                        .HasMaxLength(720)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DossierId")
                        .IsUnique();

                    b.ToTable("Question");
                });

            modelBuilder.Entity("Zorgdossier.Models.Research", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DossierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ResearchSummary")
                        .IsRequired()
                        .HasMaxLength(720)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DossierId")
                        .IsUnique();

                    b.ToTable("Research");
                });

            modelBuilder.Entity("Zorgdossier.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("StudentNumber")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("Zorgdossier.Models.Treatment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DossierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TreatmentSummary")
                        .IsRequired()
                        .HasMaxLength(720)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DossierId")
                        .IsUnique();

                    b.ToTable("Treatment");
                });

            modelBuilder.Entity("Zorgdossier.Models.BasicInformation", b =>
                {
                    b.HasOne("Zorgdossier.Models.Dossier", "Dossier")
                        .WithOne("BasicInformation")
                        .HasForeignKey("Zorgdossier.Models.BasicInformation", "DossierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dossier");
                });

            modelBuilder.Entity("Zorgdossier.Models.ComplaintsSymptoms", b =>
                {
                    b.HasOne("Zorgdossier.Models.Dossier", "Dossier")
                        .WithOne("ComplaintsSymptoms")
                        .HasForeignKey("Zorgdossier.Models.ComplaintsSymptoms", "DossierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dossier");
                });

            modelBuilder.Entity("Zorgdossier.Models.ContactAdvice", b =>
                {
                    b.HasOne("Zorgdossier.Models.Dossier", "Dossier")
                        .WithOne("ContactAdvice")
                        .HasForeignKey("Zorgdossier.Models.ContactAdvice", "DossierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dossier");
                });

            modelBuilder.Entity("Zorgdossier.Models.Dossier", b =>
                {
                    b.HasOne("Zorgdossier.Models.Student", "Student")
                        .WithMany("Dossiers")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Zorgdossier.Models.Organ", b =>
                {
                    b.HasOne("Zorgdossier.Models.Dossier", "Dossier")
                        .WithMany()
                        .HasForeignKey("DossierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dossier");
                });

            modelBuilder.Entity("Zorgdossier.Models.Phone", b =>
                {
                    b.HasOne("Zorgdossier.Models.Dossier", "Dossier")
                        .WithOne("Phone")
                        .HasForeignKey("Zorgdossier.Models.Phone", "DossierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dossier");
                });

            modelBuilder.Entity("Zorgdossier.Models.Policy", b =>
                {
                    b.HasOne("Zorgdossier.Models.Dossier", "Dossier")
                        .WithOne("Policy")
                        .HasForeignKey("Zorgdossier.Models.Policy", "DossierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dossier");
                });

            modelBuilder.Entity("Zorgdossier.Models.Question", b =>
                {
                    b.HasOne("Zorgdossier.Models.Dossier", "Dossier")
                        .WithOne("Question")
                        .HasForeignKey("Zorgdossier.Models.Question", "DossierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dossier");
                });

            modelBuilder.Entity("Zorgdossier.Models.Research", b =>
                {
                    b.HasOne("Zorgdossier.Models.Dossier", "Dossier")
                        .WithOne("Research")
                        .HasForeignKey("Zorgdossier.Models.Research", "DossierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dossier");
                });

            modelBuilder.Entity("Zorgdossier.Models.Treatment", b =>
                {
                    b.HasOne("Zorgdossier.Models.Dossier", "Dossier")
                        .WithOne("Treatment")
                        .HasForeignKey("Zorgdossier.Models.Treatment", "DossierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dossier");
                });

            modelBuilder.Entity("Zorgdossier.Models.Dossier", b =>
                {
                    b.Navigation("BasicInformation")
                        .IsRequired();

                    b.Navigation("ComplaintsSymptoms")
                        .IsRequired();

                    b.Navigation("ContactAdvice")
                        .IsRequired();

                    b.Navigation("Phone")
                        .IsRequired();

                    b.Navigation("Policy")
                        .IsRequired();

                    b.Navigation("Question")
                        .IsRequired();

                    b.Navigation("Research")
                        .IsRequired();

                    b.Navigation("Treatment")
                        .IsRequired();
                });

            modelBuilder.Entity("Zorgdossier.Models.Student", b =>
                {
                    b.Navigation("Dossiers");
                });
#pragma warning restore 612, 618
        }
    }
}
