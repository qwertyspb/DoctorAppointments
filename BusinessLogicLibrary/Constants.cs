using BusinessLogicLibrary.Enums;
using BusinessLogicLibrary.Responses;

namespace BusinessLogicLibrary;

public class Constants
{
    public const string AdminRole = "admin";
    public const string ManagerRole = "manager";
    public const string PatientRole = "patient";
    public const string DoctorRole = "doctor";

    public static string[] KnownRoles = 
    {
        AdminRole,
        ManagerRole,
        PatientRole,
        DoctorRole
    };

    public static DoctorShift Shifts = new()
    {
        MorningShiftStarts = 8,
        MorningShiftEnds = 12,
        AfternoonShiftStarts = 13,
        AfternoonShiftEnds = 17
    };

    public static Dictionary<SlotDurationType, int> Scale = new()
    {
        { SlotDurationType.Hours, 60 },
        { SlotDurationType.Shifts, 60 },
        { SlotDurationType.Min15, 15 },
        { SlotDurationType.Min30, 30 }
    };
}