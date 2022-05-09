using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class PatientDeleteRequest : IRequest<int>
    {
        public int Id { get; set; }
    }
}
