using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Models;

public partial class CinemaAppDBContext : DbContext
{
    public CinemaAppDBContext()
    {
    }

    public CinemaAppDBContext(DbContextOptions<CinemaAppDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppAdmin> AppAdmins { get; set; }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<ContentAdmin> ContentAdmins { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Screening> Screenings { get; set; }

    public virtual DbSet<ScreeningRoom> ScreeningRooms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=cinema_app_db;Trusted_Connection=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AppAdmin__3213E83F0A57CAD6");

            entity.HasOne(d => d.User).WithOne(p => p.AppAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AppAdmin__user_i__5F492382");
        });

        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cinema__3213E83F254BF967");
        });

        modelBuilder.Entity<ContentAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContentA__3213E83FDC890A15");

            entity.HasOne(d => d.Cinema).WithMany(p => p.ContentAdmins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ContentAd__cinem__6BAEFA67");

            entity.HasOne(d => d.User).WithMany(p => p.ContentAdmins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ContentAd__user___6ABAD62E");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3213E83FB8241A14");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__user_i__6319B466");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genre__3213E83FABDF4B79");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Movie__3213E83FCDB68AC3");

            entity.HasOne(d => d.Genre).WithMany(p => p.Movies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Movie__genre_id__725BF7F6");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservat__3213E83F9E83D30A");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__custo__7EC1CEDB");

            entity.HasOne(d => d.Screening).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__scree__7FB5F314");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3213E83FEB36B871");
        });

        modelBuilder.Entity<Screening>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Screenin__3213E83FDCEA45FE");

            entity.HasOne(d => d.Movie).WithMany(p => p.Screenings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Screening__movie__79FD19BE");

            entity.HasOne(d => d.ScreeningRoom).WithMany(p => p.Screenings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Screening__scree__7AF13DF7");
        });

        modelBuilder.Entity<ScreeningRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Screenin__3213E83F86870A2C");

            entity.HasOne(d => d.Cinema).WithMany(p => p.ScreeningRooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Screening__cinem__762C88DA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F94781963");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__role_id__5B78929E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
