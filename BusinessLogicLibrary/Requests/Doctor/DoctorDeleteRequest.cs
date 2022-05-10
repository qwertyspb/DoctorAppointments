using MediatR;

namespace BusinessLogicLibrary.Requests.Doctor;

public class DoctorDeleteRequest : IRequest
{
    public int Id { get; set; }
}