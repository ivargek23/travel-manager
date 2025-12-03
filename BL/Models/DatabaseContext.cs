using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BL.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Destination> Destinations { get; set; }

    public virtual DbSet<Guide> Guides { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Travel> Travels { get; set; }

    public virtual DbSet<TravelGuide> TravelGuides { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserTravel> UserTravels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=ConnectionStrings:DbConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Destination>(entity =>
        {
            entity.ToTable("Destination");

            entity.Property(e => e.Country).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Guide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tag");

            entity.ToTable("Guide");

            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("Log");

            entity.Property(e => e.Message).HasMaxLength(1024);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<Travel>(entity =>
        {
            entity.ToTable("Travel");

            entity.Property(e => e.Description).HasMaxLength(1024);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Destination).WithMany(p => p.Travels)
                .HasForeignKey(d => d.DestinationId)
                .HasConstraintName("FK_Travel_Destination");

            entity.HasOne(d => d.Image).WithMany(p => p.Travels)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Travel_Image");
        });

        modelBuilder.Entity<TravelGuide>(entity =>
        {
            entity.ToTable("TravelGuide");

            entity.HasOne(d => d.Guide).WithMany(p => p.TravelGuides)
                .HasForeignKey(d => d.GuideId)
                .HasConstraintName("FK_TravelGuide_Guide");

            entity.HasOne(d => d.Travel).WithMany(p => p.TravelGuides)
                .HasForeignKey(d => d.TravelId)
                .HasConstraintName("FK_TravelGuide_Travel");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ_User_Username").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(256);
            entity.Property(e => e.PwdHash).HasMaxLength(256);
            entity.Property(e => e.PwdSalt).HasMaxLength(256);
            entity.Property(e => e.SecurityToken).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_UserRole");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<UserTravel>(entity =>
        {
            entity.ToTable("UserTravel");

            entity.HasOne(d => d.Travel).WithMany(p => p.UserTravels)
                .HasForeignKey(d => d.TravelId)
                .HasConstraintName("FK_UserTravel_Travel");

            entity.HasOne(d => d.User).WithMany(p => p.UserTravels)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserTravel_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
