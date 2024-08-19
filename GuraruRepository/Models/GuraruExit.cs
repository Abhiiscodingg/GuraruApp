using System;
using System.Collections.Generic;

namespace GuraruRepository.Models;

public partial class GuraruExit
{
    public int ExitId { get; set; }

    public int? ExitWeight { get; set; }

    public int? BillNumber { get; set; }

    public int? BillAmount { get; set; }

    public string? DriverName { get; set; }

    public string? VehicleNumber { get; set; }

    public string? DestinationCity { get; set; }

    public string? ChallanPhotoPath { get; set; }

    public int? Quality { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? SubmittedDate { get; set; }

    public string? SubmittedBy { get; set; }

    public virtual RawQuality? QualityNavigation { get; set; }
}
