using DocAppLibrary.Entities;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class PatientEditRequest : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
