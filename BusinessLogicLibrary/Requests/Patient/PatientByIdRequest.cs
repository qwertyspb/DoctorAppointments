using BusinessLogicLibrary.Responses;
using MediatR;

namespace BusinessLogicLibrary.Requests.Patient;

public class PatientByIdRequest : IRequest<PatientDto>
{
    public int Id { get; set; }
}