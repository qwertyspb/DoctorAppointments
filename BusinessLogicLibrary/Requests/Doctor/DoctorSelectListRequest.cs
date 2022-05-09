using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessLogicLibrary.Requests
{
    public class DoctorSelectListRequest : IRequest<SelectList> { }
}
