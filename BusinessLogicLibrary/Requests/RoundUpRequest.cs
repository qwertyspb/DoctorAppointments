using MediatR;

namespace BusinessLogicLibrary.Requests
{
    public class RoundUpRequest : IRequest<DateTime>
    {
        public DateTime DT { get; set; }
        public TimeSpan D { get; set; }
    }
}
