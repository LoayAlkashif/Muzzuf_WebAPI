using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Muzzuf.Service.IService;

namespace Muzzuf.Service.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient
            {
                Host = _config["Email:Smtp"],
                Port = int.Parse(_config["Email:Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(

                    _config["Email:Username"],
                    _config["Email:Password"]
                    )
            };

            var message = new MailMessage()
            {
                From = new MailAddress(_config["Email:From"]),
                Subject= subject,
                Body = body,
                IsBodyHtml = true

            };

            message.To.Add(to);

            await smtp.SendMailAsync(message);
        }
    }
}
