using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCore2.Infrastrucutre.Services
{
    public class EmailSender : IEmailSender
    { 

        public Task SendEmailAsync(string email, string subject, string message)
        {
            //return Execute(Options.SendGridKey, subject, message, email);
            return SendMail(email, subject, message);
        }

        private Task SendMail(string recipient, string subject, string body)
        {
            //var mailConfig = _smtpConfigService.ListAllSetupSmtpConfig().Where(u => u.configname == "mailconfig").FirstOrDefault();
            using (var client = new SmtpClient()
            {
                Port = 587,//mailConfig.port, //
                //Credentials = new System.Net.NetworkCredential(mailConfig.username, mailConfig.password),
                Credentials = new System.Net.NetworkCredential("{Your Username}", "{your-password}"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl =  true,//mailConfig.isSSL,// =
                Host = "smtp.live.com",//mailConfig.smtp //= 

            })
            using (var mail = new MailMessage())
            {
                mail.To.Add(recipient);
                mail.Subject = subject;
                mail.Body = body;
                //var attachment = new Attachment(filePath);
                //mail.Attachments.Add(attachment);
                mail.From = new MailAddress("My <" + "User" + ">");
                //mail.From = new MailAddress("Anchor <" + mailConfig.username + ">");
                try
                {
                   return client.SendMailAsync(mail);
                   // return true;
                }
                catch
                {
                    throw; 
                }

            }

        } 
    }
}
