using MailKit.Security;
using MimeKit;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Data.ViewModels;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ServiceDesk.Data.Services
{
    public class SendMailService : ISendMailService
    {
        //private readonly MailSettings mailSettings;
        //public SendMailRepository(IOptions<MailSettings> _mailSettings)
        //{
        //    mailSettings = _mailSettings.Value;
        //}

        public SendMailService()
        {
        }

        // Gửi email, theo nội dung trong mailContent
        public async Task Send(MailContent mailContent)
        {
            MailSettings mailSettings = new MailSettings
            {
                //Mail = "itcenter.vietsov@gmail.com",
                //DisplayName = "itcenter.vietsov",
                //Password = "itCenter@2021",
                //Host = "smtp.gmail.com",
                //Port = 587
                Mail = "IT.ServiceDesk@vietsov.com.vn",
                DisplayName = "ServiceDesk",
                Password = "TTcntt@!21",
                Host = "mail.vietsov.com.vn",
                Port = 25
            };

            var email = new MimeMessage
            {
                Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail)
            };
            email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));

            string[] addressList = mailContent.To.Split(',');
            foreach (var address in addressList)
            {
                //mailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                email.To.Add(MailboxAddress.Parse(address));
            }

            //email.To.Add(MailboxAddress.Parse(mailContent.To));

            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = mailContent.Body
            };
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.None);
                    //smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.None);
                    smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                    await smtp.SendAsync(email);
                }
                catch (Exception ex)
                {
                    // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                    //System.IO.Directory.CreateDirectory("mailssave");
                    //var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                    //await email.WriteToAsync(emailsavefile);
                }

                //smtp.Disconnect(true);
            }
        }

        //public void SendEmail(string email, string subject, string htmlMessage)
        //{
        //    Send(new MailContent()
        //    {
        //        To = email,
        //        Subject = subject,
        //        Body = htmlMessage
        //    });
        //}

        public async Task SendEmail(string email, string subject, string htmlMessage)
        {
            await Send(new MailContent()
            {
                To = email,
                Subject = subject,
                Body = htmlMessage
            });
        }
    }
}