using AutoMapper;
using BusinessLogicLibrary.Requests.Patient;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Patient;

public class PatientEditHandler : AsyncRequestHandler<PatientEditRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatientEditHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new MapperConfiguration(x =>
        {
            x.CreateMap<PatientEditRequest, Dal.Patient>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Appointments, opt => opt.Ignore());
        }).CreateMapper();
    }

    protected override async Task Handle(PatientEditRequest request, CancellationToken token)
    {
        var repo = _unitOfWork.GetRepository<Dal.Patient>();
            
        var entity = await repo.GetById(request.Id);
        _mapper.Map(request, entity);
            
        await repo.Save();
    }
}