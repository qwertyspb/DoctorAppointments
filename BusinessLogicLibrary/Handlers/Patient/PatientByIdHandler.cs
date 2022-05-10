using AutoMapper;
using BusinessLogicLibrary.Requests.Patient;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Patient;

public class PatientByIdHandler : IRequestHandler<PatientByIdRequest, PatientDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatientByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new MapperConfiguration(x => x.CreateMap<Dal.Patient, PatientDto>()).CreateMapper();
    }

    public async Task<PatientDto> Handle(PatientByIdRequest request, CancellationToken token)
    {
        var patient = await _unitOfWork.GetRepository<Dal.Patient>().GetById(request.Id).AsTask();
        return _mapper.Map<PatientDto>(patient);
    }
}