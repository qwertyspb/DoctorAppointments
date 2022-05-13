using AutoMapper;
using BusinessLogicLibrary.Requests.Api;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Api;

public class GetDoctorsHandler : IRequestHandler<GetDoctorsRequest, List<DoctorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDoctorsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new MapperConfiguration(x => x.CreateMap<Dal.Doctor, DoctorDto>()).CreateMapper();
    }


    public async Task<List<DoctorDto>> Handle(GetDoctorsRequest request, CancellationToken token)
    {
        var doctors = await _unitOfWork.GetRepository<Dal.Doctor>().Query()
            .OrderBy(x => x.Name).ToListAsync(token);
        return _mapper.Map<List<DoctorDto>>(doctors);
    }
}