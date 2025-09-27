using System.Collections.Generic;

namespace OfficeTracker.Data;

public sealed class OfficeTrackerData
{
    public bool CurrentDayCreated { get; set; }
    public uint CurrentHomeOfficeDays { get; set; }
    public uint CurrentOfficeDays { get; set; }
    public List<TrackerDay> TrackerDays { get; set; } = [];
}