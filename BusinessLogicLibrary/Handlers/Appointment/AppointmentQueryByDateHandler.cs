using AutoMapper;
using BusinessLogicLibrary.Requests.Appointment;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Appointment
{
    public class AppointmentQueryByDateHandler : IRequestHandler<AppointmentQueryByDateRequest, List<AppointmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentQueryByDateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x => x.CreateMap<Dal.Appointment, AppointmentDto>()).CreateMapper();
        }

        public async Task<List<AppointmentDto>> Handle(AppointmentQueryByDateRequest request, CancellationToken token)
        {
            var leftDate = request.DateFrom ?? DateTime.Today;
            var rightDate = leftDate.AddDays(7);

            var appointments = await _unitOfWork.GetRepository<Dal.Appointment>()
                .Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Where(x => x.StartTime >= leftDate && x.StartTime < rightDate)
                .OrderBy(x => x.StartTime)
                .ToListAsync(token);

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }
    }
}
