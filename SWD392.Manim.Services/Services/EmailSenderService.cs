using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SWD392.Manim.Repositories.ViewModel.Email;

namespace SWD392.Manim.Services.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly string _emailAddress;
        private readonly string _appPassword;

        public EmailSenderService(IOptions<EmailSettings> emailSettings)
        {
            _emailAddress = emailSettings.Value.EmailAddress;
            _appPassword = emailSettings.Value.AppPassword;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_emailAddress, _appPassword)
            };

            return client.SendMailAsync(
                new MailMessage(from: _emailAddress,
                                 to: email,
                                 subject,
                                 message)
            );
        }
    }
}
