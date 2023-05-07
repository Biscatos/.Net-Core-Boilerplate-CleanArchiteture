using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOS.System
{
    public class MailSendDTO
    {
        public string To { get; set; }
        public List<string> Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
    }
    public class MailReplyDTO
    {
        public string MessageId { get; set; }
        public string Body { get; set; }
        public List<string> Cc { get; set; }
        //public List<AttachmentDTO> Attachment { get; set; } = new List<AttachmentDTO>();
    }
}
