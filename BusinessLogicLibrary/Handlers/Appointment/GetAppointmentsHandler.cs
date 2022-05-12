using AutoMapper;
using BusinessLogicLibrary.Requests.Appointment;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Appointment;

public class GetAppointmentsHandler : IRequestHandler<GetAppointmentsRequest, List<AppointmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAppointmentsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new MapperConfiguration(x => x.CreateMap<Dal.Appointment, AppointmentDto>()).CreateMapper();
    }

    public async Task<List<AppointmentDto>> Handle(GetAppointmentsRequest request, CancellationToken token)
    {
        var query = _unitOfWork.GetRepository<Dal.Appointment>()
            .Query()
            .Include(x => x.Doctor)
            .Where(x => !(x.EndTime <= request.Start || x.StartTime >= request.End));
        if (request.DoctorId.HasValue)
            query = query.Where(x => x.DoctorId == request.DoctorId);
        var appointments = await query.ToListAsync(token);

        return _mapper.Map<List<AppointmentDto>>(appointments);
    }
}