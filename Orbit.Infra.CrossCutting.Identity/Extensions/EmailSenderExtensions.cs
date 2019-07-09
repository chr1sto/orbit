using Orbit.Infra.CrossCutting.Identity.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Orbit.Infra.CrossCutting.Identity.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by clicking this link: \n{link}");
        }

        public static Task SendResetPasswordAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Password Reset", $"You can set a new password by clicking this link: \n{link}");
        }
    }
}
