using BusinessLogicLibrary.Responses;
using MediatR;

namespace BusinessLogicLibrary.Requests.Patient;

public class PatientQueryAllRequest : IRequest<List<PatientDto>> { }