using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class DoesRoomExistRequest : IRequest<bool>
    {
        public int Id { get; set; }
        public int Room { get; set; }
    }
}
