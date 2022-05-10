using MediatR;

namespace BusinessLogicLibrary.Requests.Doctor;

public class DoctorAddRequest : IRequest<int> 
{
    public string Name { get; set; }
    public int Room { get; set; }
}