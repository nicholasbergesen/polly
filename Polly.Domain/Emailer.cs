using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public static class Emailer
    {
        public class EmailContext
        {
            public string Subject { get; set; }
            public string Body { get; set; }
            public string To { get; set; }
        }
        public static async Task Send(EmailContext context)
        {
            using (SmtpClient client = new SmtpClient()
            {
                Host = "mail.priceboar.com",
                Port = 25,
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("accounts@priceboar.com", "17Xls^t5SDFA#12!")
            })
            {
                using (var message = new MailMessage("accounts@priceboar.com", context.To))
                {
                    message.Subject = context.Subject;
                    message.Body = context.Body;
                    message.IsBodyHtml = true;
                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
