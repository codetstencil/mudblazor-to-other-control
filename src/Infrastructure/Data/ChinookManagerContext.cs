using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class ChinookManagerContext : IdentityDbContext
{
    public ChinookManagerContext(DbContextOptions<ChinookManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<AlbumView> AlbumViews { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
        base.OnModelCreating(modelBuilder); // Call this first for Identity tables

        // Configure Identity tables
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
        });

        // Your custom configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        modelBuilder.Entity<Album>(entity =>
        {
            entity.ToTable("Album");

            entity.HasIndex(e => e.ArtistId, "IFK_AlbumArtistId");

            entity.Property(e => e.Title).HasMaxLength(160);

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlbumArtistId");
        });

        modelBuilder.Entity<AlbumView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AlbumView");

            entity.Property(e => e.Name).HasMaxLength(120);
            entity.Property(e => e.Title).HasMaxLength(160);
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("Artist");

            entity.Property(e => e.Name).HasMaxLength(120);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
