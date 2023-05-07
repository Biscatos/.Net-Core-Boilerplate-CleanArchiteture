using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOS.System
{
    public class UserStateChangeDTO
    {
        public bool IsActive { get; set; }

        public string UserId { get; set; }
    }
}
