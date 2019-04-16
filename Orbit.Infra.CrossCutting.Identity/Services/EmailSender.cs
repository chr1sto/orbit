using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Infra.CrossCutting.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly ILogger<EmailSender> _logger;
        private readonly IHostingEnvironment _env;
        private readonly MailAddress _mailFrom;

        public EmailSender(ILogger<EmailSender> logger, IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;

            var config = new ConfigurationBuilder()
            .SetBasePath(_env.ContentRootPath)
            .AddJsonFile("appsettings.json")
            .Build();

            _mailFrom = new MailAddress(config["IDENTITY_MAIL"]);
            _smtpClient = new SmtpClient(config["IDENTITY_SMTP_HOST"],int.Parse(config["IDENTITY_SMTP_PORT"]))
            {
#if DEBUG
                UseDefaultCredentials = true
#else
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(config["IDENTITY_SMTP_USER"], config["IDENTITY_SMTP_PASSWORD"])
#endif
        };
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = _mailFrom;
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            return _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
