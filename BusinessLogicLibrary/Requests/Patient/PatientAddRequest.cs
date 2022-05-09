using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Requests
{
    public class PatientAddRequest : IRequest<int> 
    {
        public string Name { get; set; }
    }
}
