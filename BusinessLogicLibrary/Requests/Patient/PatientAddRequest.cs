using MediatR;

namespace BusinessLogicLibrary.Requests.Patient;

public class PatientAddRequest : IRequest<int> 
{
    public string Name { get; set; }
}