﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MuseumExhibits.Infrastructure.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MuseumExhibits.Infrastructure.Migrations
{
    [DbContext(typeof(MuseumExhibitsDbContext))]
    partial class MuseumExhibitsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MuseumExhibits.Core.Models.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("MuseumExhibits.Core.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MuseumExhibits.Core.Models.Exhibit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalDetails")
                        .HasColumnType("text");

                    b.Property<string>("ArchivalMaterials")
                        .HasColumnType("text");

                    b.Property<string>("ArrivalMethod")
                        .HasColumnType("text");

                    b.Property<string>("AssociatedPersonsAndEvents")
                        .HasColumnType("text");

                    b.Property<string>("AuthorOrManufacturer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Bibliography")
                        .HasColumnType("text");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Classification")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly?>("ConfirmationDate")
                        .HasColumnType("date");

                    b.Property<decimal?>("Cost")
                        .HasColumnType("numeric");

                    b.Property<int?>("CreationCentury")
                        .HasColumnType("integer");

                    b.Property<DateOnly?>("CreationExactDate")
                        .HasColumnType("date");

                    b.Property<int?>("CreationYear")
                        .HasColumnType("integer");

                    b.Property<string>("DiscoveryTimeAndPlace")
                        .HasColumnType("text");

                    b.Property<int?>("ElementCount")
                        .HasColumnType("integer");

                    b.Property<string>("EnteredBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly?>("EntryDate")
                        .HasColumnType("date");

                    b.Property<string>("ExistenceTimeAndPlace")
                        .HasColumnType("text");

                    b.Property<string>("FullDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InventoryVerificationData")
                        .HasColumnType("text");

                    b.Property<bool?>("IsTransportable")
                        .HasColumnType("boolean");

                    b.Property<string>("Material")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Measurements")
                        .HasColumnType("text");

                    b.Property<string>("MovementDetails")
                        .HasColumnType("text");

                    b.Property<string>("MuseumDetails")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlaceOfCreation")
                        .HasColumnType("text");

                    b.Property<string>("PreciousMetalContent")
                        .HasColumnType("text");

                    b.Property<string>("PreciousStoneContent")
                        .HasColumnType("text");

                    b.Property<string>("PreservationState")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RelatedDocuments")
                        .HasColumnType("text");

                    b.Property<DateOnly?>("RemovalDate")
                        .HasColumnType("date");

                    b.Property<string>("RemovalReason")
                        .HasColumnType("text");

                    b.Property<string>("ResponsibleKeeper")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RestorationDetails")
                        .HasColumnType("text");

                    b.Property<string>("RestorationRecommendations")
                        .HasColumnType("text");

                    b.Property<DateOnly?>("ReturnDeadline")
                        .HasColumnType("date");

                    b.Property<string>("ReturnDetails")
                        .HasColumnType("text");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("text");

                    b.Property<string>("SourceOfArrival")
                        .HasColumnType("text");

                    b.Property<string>("StorageGroup")
                        .HasColumnType("text");

                    b.Property<string>("Technique")
                        .HasColumnType("text");

                    b.Property<string>("TemporaryStoragePurpose")
                        .HasColumnType("text");

                    b.Property<bool>("Visible")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Exhibits");
                });

            modelBuilder.Entity("MuseumExhibits.Core.Models.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ExhibitId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsTitleImage")
                        .HasColumnType("boolean");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ExhibitId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("MuseumExhibits.Core.Models.Exhibit", b =>
                {
                    b.HasOne("MuseumExhibits.Core.Models.Category", "Category")
                        .WithMany("Exhibits")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MuseumExhibits.Core.Models.Image", b =>
                {
                    b.HasOne("MuseumExhibits.Core.Models.Exhibit", "Exhibit")
                        .WithMany("Images")
                        .HasForeignKey("ExhibitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exhibit");
                });

            modelBuilder.Entity("MuseumExhibits.Core.Models.Category", b =>
                {
                    b.Navigation("Exhibits");
                });

            modelBuilder.Entity("MuseumExhibits.Core.Models.Exhibit", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
