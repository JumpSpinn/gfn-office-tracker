using System;
using OfficeTracker.Enums;

namespace OfficeTracker.Data;

public sealed class TrackerDay
{
    public DAYTYPE DayType { get; set; }
    public DateTime Date { get; set; }
}