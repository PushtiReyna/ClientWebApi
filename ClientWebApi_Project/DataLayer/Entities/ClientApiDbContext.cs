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

    public virtual DbSet<LeaveMst> LeaveMsts { get; set; }

    public virtual DbSet<SalaryMst> SalaryMsts { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=ARCHE-ITD440\\SQLEXPRESS;Database=ClientApiDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientMst>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClientMs__3214EC07C566EAD4");

            entity.ToTable("ClientMst");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Ctc)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("CTC");
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
            entity.Property(e => e.JoiningDate)
                .HasColumnType("datetime")
                .HasColumnName("Joining_Date");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProbationPeriod).HasColumnName("Probation_Period");
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

        modelBuilder.Entity<LeaveMst>(entity =>
        {
            entity.HasKey(e => e.LeaveId).HasName("PK__LeaveMst__796DB9590CC68F96");

            entity.ToTable("LeaveMst");

            entity.Property(e => e.CasualLeave)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("Casual_Leave");
            entity.Property(e => e.ClosingLeaveBalance)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Closing_Leave_Balance");
            entity.Property(e => e.EarnedLeave)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("Earned_Leave");
            entity.Property(e => e.LeaveBalance)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Leave_Balance");
            entity.Property(e => e.MonthLeave)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("Month_Leave");
            entity.Property(e => e.OpeningLeaveBalance)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Opening_Leave_Balance");
            entity.Property(e => e.SeekLeave)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("Seek_Leave");
            entity.Property(e => e.TotalLeavesTaken)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("Total_Leaves_Taken");
        });

        modelBuilder.Entity<SalaryMst>(entity =>
        {
            entity.HasKey(e => e.Salaryid).HasName("PK__SalaryMs__4BE1387F70F1333C");

            entity.ToTable("SalaryMst");

            entity.Property(e => e.ActualNetPayable)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Actual_Net_Payable");
            entity.Property(e => e.AdditionalHraAllowance)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Additional_HRA_Allowance");
            entity.Property(e => e.Advances).HasColumnType("decimal(13, 2)");
            entity.Property(e => e.Basic).HasColumnType("decimal(13, 2)");
            entity.Property(e => e.ConveyanceAllowance)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Conveyance_Allowance");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.EmployerContPf)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Employer_Cont_PF");
            entity.Property(e => e.Esic)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("ESIC");
            entity.Property(e => e.FixedBasicSalary)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Fixed_Basic_Salary");
            entity.Property(e => e.FixedConveyanceAllowance)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Fixed_Conveyance_Allowance");
            entity.Property(e => e.FixedHra)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Fixed_HRA");
            entity.Property(e => e.FixedMedicalAllowance)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Fixed_Medical_Allowance");
            entity.Property(e => e.FlexibleAllowance)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Flexible_Allowance");
            entity.Property(e => e.GrossSalaryPayable)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Gross_salary_payable");
            entity.Property(e => e.HouseRentAllowance)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("House_Rent_Allowance");
            entity.Property(e => e.Incentives).HasColumnType("decimal(13, 2)");
            entity.Property(e => e.IncomeTax)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Income_Tax");
            entity.Property(e => e.MedicalAllowance)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Medical_Allowance");
            entity.Property(e => e.PayableDays).HasColumnName("Payable_days");
            entity.Property(e => e.PfOne)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("PF_one");
            entity.Property(e => e.PfTwo)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("PF_two");
            entity.Property(e => e.Pt)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("PT");
            entity.Property(e => e.SalaryAdditionalHraAllowance)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Salary_Additional_HRA_Allowance");
            entity.Property(e => e.Total).HasColumnType("decimal(13, 2)");
            entity.Property(e => e.TotalDays).HasColumnName("Total_days");
            entity.Property(e => e.TotalDed)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("Total_Ded");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
