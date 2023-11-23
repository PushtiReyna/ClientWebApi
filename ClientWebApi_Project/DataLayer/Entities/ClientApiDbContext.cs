using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities;

public partial class ClientApiDbContext : DbContext
{
    public ClientApiDbContext()
    {
    }

    public ClientApiDbContext(DbContextOptions<ClientApiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClientMst> ClientMsts { get; set; }

    public virtual DbSet<DocumentMst> DocumentMsts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ARCHE-ITD440\\SQLEXPRESS;Database=ClientApiDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClientMs__3214EC07A039A7B8");

            entity.ToTable("ClientMst");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.DocumentName).HasMaxLength(200);
            entity.Property(e => e.Fullname)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Image).HasMaxLength(400);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RefreshToken).HasMaxLength(200);
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(500);
            entity.Property(e => e.TokenExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DocumentMst>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__Document__1ABEEF0F787B4C77");

            entity.ToTable("DocumentMst");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DocumentName).HasMaxLength(200);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
