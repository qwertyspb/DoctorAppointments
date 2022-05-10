using MediatR;

namespace BusinessLogicLibrary.Requests.Doctor;

public class DoctorEditRequest : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Room { get; set; }
}