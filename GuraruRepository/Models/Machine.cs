using System;
using System.Collections.Generic;

namespace GuraruRepository.Models;

public partial class Machine
{
    public int MachineId { get; set; }

    public string? MachineName { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<ManufacturingStage1> ManufacturingStage1s { get; set; } = new List<ManufacturingStage1>();
}
