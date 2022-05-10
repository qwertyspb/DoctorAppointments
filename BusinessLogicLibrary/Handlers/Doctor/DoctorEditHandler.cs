using AutoMapper;
using BusinessLogicLibrary.Requests.Doctor;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Doctor;

public class DoctorEditHandler : AsyncRequestHandler<DoctorEditRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DoctorEditHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = new MapperConfiguration(x =>
        {
            x.CreateMap<DoctorEditRequest, Dal.Patient>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Appointments, opt => opt.Ignore());
        }).CreateMapper();
    }

    protected override async Task Handle(DoctorEditRequest request, CancellationToken token)
    {
        var repo = _unitOfWork.GetRepository<Dal.Doctor>();

        var entity = await repo.GetById(request.Id);
        _mapper.Map(request, entity);

        await repo.Save();
    }
}