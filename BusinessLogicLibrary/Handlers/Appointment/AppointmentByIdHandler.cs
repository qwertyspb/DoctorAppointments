using AutoMapper;
using BusinessLogicLibrary.Requests.Appointment;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Appointment
{
    public class AppointmentByIdHandler : IRequestHandler<AppointmentByIdRequest, AppointmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<Dal.Appointment, AppointmentDto>();
            }).CreateMapper();
        }

        public async Task<AppointmentDto> Handle(AppointmentByIdRequest request, CancellationToken token)
        {
            var appointment = await _unitOfWork.GetRepository<Dal.Appointment>().Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .FirstOrDefaultAsync(token);
            return _mapper.Map<AppointmentDto>(appointment);
        }
    }
}
