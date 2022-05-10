using AutoMapper;
using BusinessLogicLibrary.Requests.Doctor;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Doctor;

public class DoctorByIdHandler : IRequestHandler<DoctorByIdRequest, DoctorDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DoctorByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new MapperConfiguration(x => x.CreateMap<Dal.Doctor, DoctorDto>()).CreateMapper();
    }

    public async Task<DoctorDto> Handle(DoctorByIdRequest request, CancellationToken token)
    {
        var doctor = await _unitOfWork.GetRepository<Dal.Doctor>().GetById(request.Id).AsTask();
        return _mapper.Map<DoctorDto>(doctor);
    }
}