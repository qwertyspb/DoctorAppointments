using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessLogicLibrary.Requests
{
    public class PatientSelectListRequest : IRequest<SelectList> { }
}
