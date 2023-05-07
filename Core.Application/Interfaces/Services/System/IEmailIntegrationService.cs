using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Application.DTOS.System;

namespace Core.Application.Interfaces.Services.System
{
    public interface IEmailIntegrationService
    {
        Task<bool> SendAsync(MailSendDTO dto);
        Task<bool> ReplyAsync(MailReplyDTO dto);
    }
}
