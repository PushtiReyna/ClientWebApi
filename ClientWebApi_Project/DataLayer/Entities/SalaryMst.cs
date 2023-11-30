using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class SalaryMst
{
    public int Salaryid { get; set; }

    public int Id { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public decimal FixedBasicSalary { get; set; }

    public decimal FixedHra { get; set; }

    public decimal FixedConveyanceAllowance { get; set; }

    public decimal FixedMedicalAllowance { get; set; }

    public decimal AdditionalHraAllowance { get; set; }

    public int TotalDays { get; set; }

    public int PayableDays { get; set; }

    public decimal GrossSalaryPayable { get; set; }

    public decimal Basic { get; set; }

    public decimal HouseRentAllowance { get; set; }

    public decimal EmployerContPf { get; set; }

    public decimal ConveyanceAllowance { get; set; }

    public decimal MedicalAllowance { get; set; }

    public decimal SalaryAdditionalHraAllowance { get; set; }

    public decimal FlexibleAllowance { get; set; }

    public decimal? Incentives { get; set; }

    public decimal Total { get; set; }

    public decimal PfOne { get; set; }

    public decimal PfTwo { get; set; }

    public decimal Esic { get; set; }

    public decimal Pt { get; set; }

    public decimal Advances { get; set; }

    public decimal IncomeTax { get; set; }

    public decimal TotalDed { get; set; }

    public decimal ActualNetPayable { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
