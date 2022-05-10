using BusinessLogicLibrary.Requests.Doctor;
using DocAppLibrary.Interfaces;
using MediatR;
using Dal = DocAppLibrary.Entities;

namespace BusinessLogicLibrary.Handlers.Doctor;

public class DoctorAddHandler : IRequestHandler<DoctorAddRequest, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorAddHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(DoctorAddRequest request, CancellationToken token)
    {
        var entity = new Dal.Doctor
        {
            Name = request.Name,
            Room = request.Room
        };
        await _unitOfWork.GetRepository<Dal.Doctor>().Create(entity);
        return entity.Id;
    }
}