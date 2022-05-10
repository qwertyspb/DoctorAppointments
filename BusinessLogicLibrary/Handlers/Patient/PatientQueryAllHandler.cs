using AutoMapper;
using BusinessLogicLibrary.Requests.Patient;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Patient;

public class PatientQueryAllHandler : IRequestHandler<PatientQueryAllRequest, List<PatientDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatientQueryAllHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new MapperConfiguration(x => x.CreateMap<Dal.Patient, PatientDto>()).CreateMapper();
    }

    public async Task<List<PatientDto>> Handle(PatientQueryAllRequest request, CancellationToken token)
    {
        var patients = await _unitOfWork.GetRepository<Dal.Patient>().Query()
            .OrderBy(x => x.Name).ToListAsync(token);
        return _mapper.Map<List<PatientDto>>(patients);
    }
}
