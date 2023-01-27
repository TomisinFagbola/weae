using System.ComponentModel;

namespace Domain.Enums
{
    public enum EUserStatus
    {
        [Description("Active")]
        Active = 1,
        [Description("Pending")]
        Pending = 2,
        [Description("Deactivated")]
        Deactivated = 3,
    }
}