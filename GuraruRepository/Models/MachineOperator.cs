using System;
using System.Collections.Generic;

namespace GuraruRepository.Models;

public partial class MachineOperator
{
    public int OperatorId { get; set; }

    public string? OperatorName { get; set; }

    public bool? IsDeleted { get; set; }

    public string? AdhaarNo { get; set; }

    public string? LocationDetails { get; set; }

    public virtual ICollection<ManufacturingStage1> ManufacturingStage1s { get; set; } = new List<ManufacturingStage1>();
}
