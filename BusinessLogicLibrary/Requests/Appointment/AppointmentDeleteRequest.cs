using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class AppointmentDeleteRequest : IRequest<int>
    {
        public int Id { get; set; }
    }
}
