using BusinessLogicLibrary.Responses;
using MediatR;

namespace BusinessLogicLibrary.Requests.Doctor;

public class DoctorByIdRequest : IRequest<DoctorDto>
{
    public int Id { get; set; }
}