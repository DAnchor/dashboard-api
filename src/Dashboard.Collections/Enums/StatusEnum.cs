using System.ComponentModel;

namespace Dashboard.Collections.Enums;

public enum StatusEnum
{
    [Description("Pending")]
    Pending = 1,
    [Description("InProgress")]
    InProgress = 2,
    [Description("Completed")]
    Completed = 3,
    [Description("Arhived")]
    Archived = 4
}