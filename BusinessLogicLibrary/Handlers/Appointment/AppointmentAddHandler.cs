using AutoMapper;
using BusinessLogicLibrary.Requests;
using DocAppLibrary.Entities;
using DocAppLibrary.Interfaces;
using MediatR;

namespace BusinessLogicLibrary.Handlers
{
    public class AppointmentAddHandler : IRequestHandler<AppointmentAddRequest, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AppointmentAddHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = new MapperConfiguration(x => x.CreateMap<AppointmentAddRequest, Appointment>()).CreateMapper();
        }

        public async Task<int> Handle(AppointmentAddRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Appointment>(request);
            await _unitOfWork.GetRepository<Appointment>().Create(entity);
            return entity.Id; 
        }
    }
}
