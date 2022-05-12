using AutoMapper;
using BusinessLogicLibrary.Requests.Appointment;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Enum;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLibrary.Handlers.Appointment;

public class GetPatientAppointmentsHandler : IRequestHandler<GetPatientAppointmentsRequest, List<AppointmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPatientAppointmentsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new MapperConfiguration(x => x.CreateMap<DocAppLibrary.Entities.Appointment, AppointmentDto>()).CreateMapper();
    }

    public async Task<List<AppointmentDto>> Handle(GetPatientAppointmentsRequest request, CancellationToken token)
    {
        var appointments = await _unitOfWork.GetRepository<DocAppLibrary.Entities.Appointment>()
            .Query()
            .Include(x => x.Doctor)
            .Include(x => x.Patient)
            .Where(x =>
                (x.Status == StatusType.Free || (x.Status != StatusType.Free && x.PatientId == request.PatientId)) &&
                !(x.EndTime <= request.Start || x.StartTime >= request.End))
            .ToListAsync(token);

        return _mapper.Map<List<AppointmentDto>>(appointments);
    }
}