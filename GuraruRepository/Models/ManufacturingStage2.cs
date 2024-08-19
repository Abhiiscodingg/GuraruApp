using System;
using System.Collections.Generic;

namespace GuraruRepository.Models;

public partial class ManufacturingStage2
{
    public int Id { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public int? SubmittedWeight { get; set; }

    public int? Quality { get; set; }

    public int? CompletedWeight { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string? SubmittedBy { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? UsedWeight { get; set; }

    public virtual RawQuality? QualityNavigation { get; set; }
}
