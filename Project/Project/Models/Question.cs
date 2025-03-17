using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int ExamId { get; set; }

    public string? ImagePath { get; set; }

    public string Content { get; set; } = null!;

    public virtual Exam Exam { get; set; } = null!;

    public virtual ICollection<Option> Options { get; set; } = new List<Option>();
}
