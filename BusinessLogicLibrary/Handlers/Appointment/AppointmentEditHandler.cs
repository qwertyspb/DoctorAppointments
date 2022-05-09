using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class AppointmentEditHandler : IRequestHandler<AppointmentEditRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public AppointmentEditHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<int> Handle(AppointmentEditRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Appointment>();
            var entity = await repo.GetById(request.Id);

            entity.StartTime = await _mediator.Send(new RoundUpRequest
            {
                DT = request.StartTime,
                D = TimeSpan.FromMinutes(1)
            });

            entity.EndTime = await _mediator.Send(new RoundUpRequest
            {
                DT = request.EndTime,
                D = TimeSpan.FromMinutes(1)
            });

            entity.DoctorId = request.DoctorId;
            entity.PatientId = request.PatientId;

            await repo.Save();

            return 0;
        }
    }
}
