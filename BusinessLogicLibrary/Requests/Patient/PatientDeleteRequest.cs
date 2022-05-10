using MediatR;

namespace BusinessLogicLibrary.Requests.Patient;

public class PatientDeleteRequest : IRequest
{
    public int Id { get; set; }
}