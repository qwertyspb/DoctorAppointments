using DocAppLibrary.Entities;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class DoctorByIdRequest : IRequest<Doctor>
    {
        public int Id { get; set; }
    }
}
