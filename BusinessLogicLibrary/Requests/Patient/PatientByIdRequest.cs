using DocAppLibrary.Entities;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class PatientByIdRequest : IRequest<Patient>
    {
        public int Id { get; set; }
    }
}
