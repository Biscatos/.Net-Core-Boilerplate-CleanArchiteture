using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOS.System
{
    public class UserResponseDTO
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string PeerdID { get; set; }

        public List<string> Roles { get; set; }

        public Guid CompanyId { get; set; }
    }
}
