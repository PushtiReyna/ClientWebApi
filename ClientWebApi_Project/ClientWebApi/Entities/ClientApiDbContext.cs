using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClientWebApi.Entities;

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

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=ARCHE-ITD440\\SQLEXPRESS;Database=ClientApiDB;Trusted_Connection=True;TrustServerCertificate=True;");

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
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
