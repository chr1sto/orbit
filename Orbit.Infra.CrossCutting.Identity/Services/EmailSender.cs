using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using MailKit;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Orbit.Infra.CrossCutting.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IHostingEnvironment _env;
        private readonly MailboxAddress _mailFrom;
        private readonly string _host;
        private readonly int _port;
        private readonly string _login;
        private readonly string _password;

        public EmailSender(ILogger<EmailSender> logger, IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;

            var config = new ConfigurationBuilder()
            .SetBasePath(_env.ContentRootPath)
            .AddJsonFile("appsettings.json")
            .Build();

            _mailFrom = new MailboxAddress("Euphresia Flyff", config["IDENTITY_MAIL"]);
            _host = config["IDENTITY_SMTP_HOST"];
            _port = int.Parse(config["IDENTITY_SMTP_PORT"]);
            _login = config["IDENTITY_SMTP_USER"];
            _password = config["IDENTITY_SMTP_PASSWORD"];
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var msg = new MimeMessage();
            msg.From.Add(_mailFrom);
            msg.To.Add(new MailboxAddress(email));
            msg.Subject = subject;
            msg.Body = new TextPart("plain") { Text = message };

            using(var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_host, _port, true);
                client.Authenticate(_login, _password);
                client.Send(msg);
                client.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
