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
}