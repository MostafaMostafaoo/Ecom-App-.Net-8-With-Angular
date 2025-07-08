
using Ecom.core.DTO;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repostories.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmail(EmailDTO emailDTO)
        {//SMTP
            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("My Ecom", configuration["EmailSetting:From"]));
            message.Subject = emailDTO.Subject;

            message.To.Add(new MailboxAddress(emailDTO.To, emailDTO.To));

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(

                        configuration["EmailSetting:Smtp"],
                        int.Parse(configuration["EmailSetting:Port"]) , true);
                    await smtp.AuthenticateAsync(configuration["EmailSetting:Username"],
                        configuration["EmailSetting:Password"]);

                    await smtp.SendAsync(message);

                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    smtp.Disconnect(true);
                    smtp.Dispose();
                }
            }
        }
    }
}


/*
using Ecom.core.DTO;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repostories.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmail(EmailDTO emailDTO)
        {
           
            MimeMessage message = new MimeMessage();

         
            var fromAddress = configuration["EmailSetting:From"];
            if (string.IsNullOrWhiteSpace(fromAddress))
                throw new ArgumentException("Sender email address is not configured.");

            message.From.Add(new MailboxAddress("My Ecom", fromAddress));
            message.Subject = emailDTO.Subject;

            if (!string.IsNullOrWhiteSpace(emailDTO.To))
            {
                try
                {
                    var toAddress = MailboxAddress.Parse(emailDTO.To);
                    message.To.Add(toAddress);
                }
                catch (FormatException)
                {
                    throw new ArgumentException("Invalid email format in emailDTO.To");
                }
            }
            else
            {
                throw new ArgumentException("Email 'To' address cannot be null or empty");
            }

            // محتوى الرسالة
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };

            // إرسال الإيميل باستخدام SMTP
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(
                        configuration["EmailSetting:Smtp"],
                        int.Parse(configuration["EmailSetting:Port"]),
                        true
                    );

                    await smtp.AuthenticateAsync(
                        configuration["EmailSetting:Username"],
                        configuration["EmailSetting:Password"]
                    );

                    await smtp.SendAsync(message);
                }
                catch (Exception ex)
                {
                    // يمكنك إضافة logging هنا لو حابب
                    throw new InvalidOperationException("Failed to send email.", ex);
                }
                finally
                {
                    await smtp.DisconnectAsync(true);
                    smtp.Dispose();
                }
            }
        }
    }
}
*/