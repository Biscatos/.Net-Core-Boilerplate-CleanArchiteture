using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOS.System
{
    public class ResetPasswordDTO
    {
        [Required]
        public string UserName { get; set; }

        public string Token { get; set; }

        [Required]
        [MinLength(6)]
        public string LastPassword { get; set; }
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
