﻿// <auto-generated />
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Domain.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Domain.Dj", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DjName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PricePerSecond")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Djs");
                });

            modelBuilder.Entity("Domain.Set", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DjId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SetName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DjId");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("Domain.SetSong", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SetId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SongId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.HasIndex("SongId");

                    b.ToTable("SetSongs");
                });

            modelBuilder.Entity("Domain.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Composer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Length")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LyricArtist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Performer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SongName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("Domain.Set", b =>
                {
                    b.HasOne("Domain.Dj", "Dj")
                        .WithMany("Sets")
                        .HasForeignKey("DjId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dj");
                });

            modelBuilder.Entity("Domain.SetSong", b =>
                {
                    b.HasOne("Domain.Set", "Set")
                        .WithMany("SetSongs")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Song", "Song")
                        .WithMany("SetSongs")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Set");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("Domain.Song", b =>
                {
                    b.HasOne("Domain.Category", "Category")
                        .WithMany("Songs")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Domain.Category", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("Domain.Dj", b =>
                {
                    b.Navigation("Sets");
                });

            modelBuilder.Entity("Domain.Set", b =>
                {
                    b.Navigation("SetSongs");
                });

            modelBuilder.Entity("Domain.Song", b =>
                {
                    b.Navigation("SetSongs");
                });
#pragma warning restore 612, 618
        }
    }
}
