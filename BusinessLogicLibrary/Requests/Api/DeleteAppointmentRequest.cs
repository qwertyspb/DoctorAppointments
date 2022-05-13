using MediatR;

namespace BusinessLogicLibrary.Requests.Api;

public class DeleteAppointmentRequest : IRequest
{
    public int Id { get; set; }
}