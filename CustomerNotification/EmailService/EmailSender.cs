using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        public EmailSender()
        {

        }
        public Task SendMessageAsync(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
