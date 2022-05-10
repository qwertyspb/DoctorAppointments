using AutoMapper;
using BusinessLogicLibrary.Requests.Doctor;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Doctor;

public class DoctorQueryAllHandler : IRequestHandler<DoctorQueryAllRequest, List<DoctorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DoctorQueryAllHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new MapperConfiguration(x => x.CreateMap<Dal.Doctor, DoctorDto>()).CreateMapper();
    }

    public async Task<List<DoctorDto>> Handle(DoctorQueryAllRequest request, CancellationToken token)
    {
        var doctors = await _unitOfWork.GetRepository<Dal.Doctor>().Query()
            .OrderBy(x => x.Name).ToListAsync(token);
        return _mapper.Map<List<DoctorDto>>(doctors);
    }
}