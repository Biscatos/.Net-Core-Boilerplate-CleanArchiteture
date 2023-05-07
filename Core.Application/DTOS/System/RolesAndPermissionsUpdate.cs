using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOS.System
{
    public class RolesOrClaimsUpdateDTO
    {
        public Guid IdUser { get; set; }
        public List<string> Roles { get; set; }
    }
}
