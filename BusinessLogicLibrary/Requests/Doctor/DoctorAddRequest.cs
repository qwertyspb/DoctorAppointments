using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Requests
{
    public class DoctorAddRequest : IRequest<int> 
    {
        public string Name { get; set; }
        public int Room { get; set; }
    }
}
