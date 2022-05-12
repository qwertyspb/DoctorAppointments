using AutoMapper;
using BusinessLogicLibrary.Requests.Appointment;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Appointment;

public class CreateAppointmentsHandler : IRequestHandler<CreateAppointmentsRequest, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAppointmentsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new MapperConfiguration(x =>
        {
            x.CreateMap<AppointmentDto, Dal.Appointment>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.PatientId, opt => opt.Ignore())
                .ForMember(dst => dst.Doctor, opt => opt.Ignore())
                .ForMember(dst => dst.Patient, opt => opt.Ignore());
        }).CreateMapper();
    }

    public async Task<bool> Handle(CreateAppointmentsRequest request, CancellationToken token)
    {
        var repo = _unitOfWork.GetRepository<Dal.Doctor>();
        var doctor = await repo.GetById(request.DoctorId);
        if (doctor == null)
            return false;

        var slots = TimeLineService.GenerateSlots(request.Start, request.End, request.Scale);
        slots.ForEach(x => x.DoctorId = doctor.Id);
            
        var appRepo = _unitOfWork.GetRepository<Dal.Appointment>();
        var appointments = _mapper.Map<List<Dal.Appointment>>(slots);
        foreach (var item in appointments)
            await appRepo.Create(item);

        return true;
    }
}