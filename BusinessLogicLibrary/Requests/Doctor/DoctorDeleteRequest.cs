using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class DoctorDeleteRequest : IRequest<int>
    {
        public int Id { get; set; }
    }
}
