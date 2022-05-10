using AutoMapper;
using BusinessLogicLibrary.Requests.Appointment;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Appointment
{
    public class AppointmentQueryAllHandler : IRequestHandler<AppointmentQueryAllRequest, List<AppointmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentQueryAllHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x => x.CreateMap<Dal.Appointment, AppointmentDto>()).CreateMapper();
        }

        public async Task<List<AppointmentDto>> Handle(AppointmentQueryAllRequest request, CancellationToken token)
        {
            var appointments = await _unitOfWork.GetRepository<Dal.Appointment>()
                .Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .OrderBy(x => x.StartTime)
                .ToListAsync(token);

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }
    }
}
