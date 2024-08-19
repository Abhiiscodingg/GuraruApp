using System;
using System.Collections.Generic;

namespace GuraruRepository.Models;

public partial class ManufacturingStage1
{
    public int Id { get; set; }

    public int? Machine { get; set; }

    public int? MachineOperator { get; set; }

    public int? Quality { get; set; }

    public int? SubmittedWeght { get; set; }

    public int? CompletedWeight { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public string? SubmittedBy { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public int? UsedWeight { get; set; }

    public virtual Machine? MachineNavigation { get; set; }

    public virtual MachineOperator? MachineOperatorNavigation { get; set; }

    public virtual RawQuality? QualityNavigation { get; set; }
}
