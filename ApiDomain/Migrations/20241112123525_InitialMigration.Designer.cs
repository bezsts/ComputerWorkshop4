﻿// <auto-generated />
using System;
using ApiDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiDomain.Migrations
{
    [DbContext(typeof(DataModelContext))]
    [Migration("20241112123525_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("ApiDomain.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("ReleaseDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateOnly(2024, 11, 12));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Movie");
                });

            modelBuilder.Entity("ApiDomain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MovieUser", b =>
                {
                    b.Property<int>("UsersWhoWatchedId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WatchedMoviesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UsersWhoWatchedId", "WatchedMoviesId");

                    b.HasIndex("WatchedMoviesId");

                    b.ToTable("MovieUser");
                });

            modelBuilder.Entity("MovieUser", b =>
                {
                    b.HasOne("ApiDomain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersWhoWatchedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiDomain.Entities.Movie", null)
                        .WithMany()
                        .HasForeignKey("WatchedMoviesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
