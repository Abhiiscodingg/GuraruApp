using System;
using System.Collections.Generic;

namespace GuraruRepository.Models;

public partial class RawThread
{
    public int ThreadId { get; set; }

    public int? ThWeight { get; set; }

    public int? BillAmount { get; set; }

    public int? BillNo { get; set; }

    public DateTime? BillDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CompanyName { get; set; }

    public int? Quality { get; set; }

    public bool? IsDeleted { get; set; }

    public int? UsedWeight { get; set; }

    public virtual RawQuality? QualityNavigation { get; set; }
}
