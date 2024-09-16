using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Assignment01.Entities;

namespace Assignment01.Context
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Film> Films { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<Show> Shows { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                // optionsBuilder.UseSqlServer("Server=localhost;Database=Cinema;Trusted_Connection=True;MultipleActiveResultSets=true;User Id=henrygphp;Password=123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.SeatStatus).IsFixedLength();

                entity.HasOne(d => d.Show)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.ShowID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bookings_Shows");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryCode)
                    .HasName("PK_Country");

                entity.Property(e => e.CountryCode).IsFixedLength();
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.Property(e => e.CountryCode).IsFixedLength();

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Films)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Films_Countries");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Films)
                    .HasForeignKey(d => d.GenreID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Films_Genres");
            });

            modelBuilder.Entity<Show>(entity =>
            {
                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Shows)
                    .HasForeignKey(d => d.FilmID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shows_Films");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Shows)
                    .HasForeignKey(d => d.RoomID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shows_Rooms");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
