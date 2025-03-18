using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public int UserId { get; set; }

    public DateOnly IssuedDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public string? CertificateCode { get; set; }

    public int? ExamId { get; set; }

    public bool IsApproved { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual User User { get; set; } = null!;
}
