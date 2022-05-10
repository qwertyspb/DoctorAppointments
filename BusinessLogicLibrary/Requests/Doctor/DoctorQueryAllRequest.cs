using BusinessLogicLibrary.Responses;
using MediatR;

namespace BusinessLogicLibrary.Requests.Doctor;

public class DoctorQueryAllRequest : IRequest<List<DoctorDto>> { }