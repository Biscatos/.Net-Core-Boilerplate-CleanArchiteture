using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Core.Application.DTOS.System
{
    public class AuthenticationResponse
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public List<ClaimDTO> Claims { get; set; }
        public bool IsVerified { get; set; }
        public string JWToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public string PeerdID { get; set; }
        //public CompanyResponseDTO Company { get; set; }
    }
}
