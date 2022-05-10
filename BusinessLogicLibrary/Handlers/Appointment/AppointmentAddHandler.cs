using AutoMapper;
using BusinessLogicLibrary.Requests.Appointment;
using BusinessLogicLibrary.Extensions;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Appointment
{
    public class AppointmentAddHandler : IRequestHandler<AppointmentAddRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AppointmentAddHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<AppointmentAddRequest, Dal.Appointment>()
                    .ForMember(dst => dst.Id, opt => opt.Ignore())
                    .ForMember(dst => dst.Doctor, opt => opt.Ignore())
                    .ForMember(dst => dst.Patient, opt => opt.Ignore());
            }).CreateMapper();
        }

        public async Task<int> Handle(AppointmentAddRequest request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Dal.Appointment>();
            var isIntersected = await repo.CheckIntersection(0, request.StartTime, request.EndTime, request.DoctorId,
                request.PatientId);
            if (isIntersected)
                return 0;

            var entity = _mapper.Map<Dal.Appointment>(request);
            await repo.Create(entity);
            return entity.Id; 
        }
    }
}
