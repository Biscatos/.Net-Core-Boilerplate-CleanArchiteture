using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Application.DTOS.System;
using Core.Application.Interfaces.Services.System;

namespace Infrastruture.Shared.Services
{
    public class EmailIntegrationService : IEmailIntegrationService
    {
        public Task<bool> ReplyAsync(MailReplyDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendAsync(MailSendDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
