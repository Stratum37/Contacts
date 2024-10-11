﻿// <auto-generated />
using System;
using Contacts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Contacts.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241011062749_ADDTBLS")]
    partial class ADDTBLS
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Contacts.Domain.Entities.Contact", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"));

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactFirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactLastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDT")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhotoURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Contacts.Domain.Entities.FullAddress", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"));

                    b.Property<int>("BuildingNumber")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ContactId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Entrance")
                        .HasColumnType("int");

                    b.Property<int>("Flat")
                        .HasColumnType("int");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isAuthNeeded")
                        .HasColumnType("bit");

                    b.Property<bool>("isVisibleAPI")
                        .HasColumnType("bit");

                    b.Property<bool>("isVisibleWeb")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("FullAddress");
                });

            modelBuilder.Entity("Contacts.Domain.Entities.Messenger", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"));

                    b.Property<decimal>("ContactId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("MainEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelegramName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelegramNick")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelegramPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WhatsAppName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WhatsAppNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isAuthNeeded")
                        .HasColumnType("bit");

                    b.Property<bool>("isVisibleAPI")
                        .HasColumnType("bit");

                    b.Property<bool>("isVisibleWeb")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("Messenger");
                });

            modelBuilder.Entity("Contacts.Domain.Entities.FullAddress", b =>
                {
                    b.HasOne("Contacts.Domain.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("Contacts.Domain.Entities.Messenger", b =>
                {
                    b.HasOne("Contacts.Domain.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contact");
                });
#pragma warning restore 612, 618
        }
    }
}
