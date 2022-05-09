using BusinessLogicLibrary.Requests;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class RoundUpHandler : IRequestHandler<RoundUpRequest, DateTime>
    {
        public Task<DateTime> Handle(RoundUpRequest request, CancellationToken cancellationToken)
        {
            var result = new DateTime((request.DT.Ticks + request.D.Ticks - 1) / request.D.Ticks * request.D.Ticks, request.DT.Kind);
            return Task.FromResult(result);
        }
    }
}
