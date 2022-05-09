using DocAppLibrary.Entities;
using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class DoctorQueryAllRequest : IRequest<List<Doctor>> { }
}
