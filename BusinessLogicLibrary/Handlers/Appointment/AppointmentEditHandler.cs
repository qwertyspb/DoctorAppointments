using BusinessLogicLibrary.Extensions;
using BusinessLogicLibrary.Requests.Appointment;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Appointment
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
        public async Task<int> Handle(AppointmentEditRequest request, CancellationToken token)
        {
            var repo = _unitOfWork.GetRepository<Dal.Appointment>();

            var start = request.StartTime.RoundUp(TimeSpan.FromMinutes(1));
            var end = request.EndTime.RoundUp(TimeSpan.FromMinutes(1));

            var isIntersected = await repo.CheckIntersection(0, start, end, request.DoctorId, request.PatientId);
            if (isIntersected)
                return 0;

            var entity = await repo.GetById(request.Id);

            entity.StartTime = start;
            entity.EndTime = end;
            entity.DoctorId = request.DoctorId;
            entity.PatientId = request.PatientId;

            await repo.Save();

            return entity.Id;
        }
    }
}
