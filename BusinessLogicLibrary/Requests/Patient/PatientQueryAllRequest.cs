using DocAppLibrary.Entities;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class PatientQueryAllRequest : IRequest<List<Patient>> { }
}
