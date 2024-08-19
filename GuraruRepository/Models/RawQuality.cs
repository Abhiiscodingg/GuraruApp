using System;
using System.Collections.Generic;

namespace GuraruRepository.Models;

public partial class RawQuality
{
    public int QualityId { get; set; }

    public string? QualityName { get; set; }

    public string? QualityCode { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<GuraruExit> GuraruExits { get; set; } = new List<GuraruExit>();

    public virtual ICollection<ManufacturingStage1> ManufacturingStage1s { get; set; } = new List<ManufacturingStage1>();

    public virtual ICollection<ManufacturingStage2> ManufacturingStage2s { get; set; } = new List<ManufacturingStage2>();

    public virtual ICollection<RawThread> RawThreads { get; set; } = new List<RawThread>();
}
