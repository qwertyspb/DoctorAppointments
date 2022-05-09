using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class DoctorEditRequest : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Room { get; set; }
    }
}
