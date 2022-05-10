using MediatR;

namespace BusinessLogicLibrary.Requests.Patient;

public class PatientEditRequest : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
}