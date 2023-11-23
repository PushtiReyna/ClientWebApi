using System;
using System.Collections.Generic;

namespace DataLayer.Entities;

public partial class DocumentMst
{
    public int DocumentId { get; set; }

    public string? DocumentName { get; set; }

    public string? Document { get; set; }

    public int Id { get; set; }

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }

    public bool CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
