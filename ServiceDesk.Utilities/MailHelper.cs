using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace ServiceDesk.Utilities
{
    public class MailHelper: IDisposable
    {
        private bool _disposedValue;
        public static string SmtpServer = "smtp.gmail.com";
        public static int Port = 587;
        public static string CredentialUserName = "vietsovpetrogas@gmail.com";
        public static string CredentialPassword = "@dMin123";
        public static string EnableSsl = "True";
        public static bool Ssl = true;
        public static string From = "vietsovpetrogas@gmail.com";

        public static void Send(string subject, string body, SqlDataReader reader)
        {
            const string to = "nghianl.ts@vietsov.com.vn";
            var cc = "";
            const string bcc = "";
            const string attachments = "";
            while (reader.Read()) cc = reader["Email"].ToString();
                var email = new Thread(delegate ()
                    {
                        SendAsyncEmail(From, to, cc, bcc, subject, body, attachments);
                    })
                    { IsBackground = true };
                email.Start();
            }


        /// <summary>
        ///     Gửi email đơn giản thông qua tài khoản gmail
        /// </summary>
        /// <param name="from">Email người gửi</param>
        /// <param name="to">Email người nhận</param>
        /// <param name="subject">Tiêu đề mail</param>
        /// <param name="body">Nội dung mail</param>
        public static void Send(string from, string to, string subject, string body)
        {
            const string cc = "";
            const string bcc = "";
            const string attachments = "";

            var email =
                new Thread(delegate ()
                {
                    SendAsyncEmail(from, to, cc, bcc, subject, body, attachments);
                })
                { IsBackground = true };
            email.Start();
        }

        /// <summary>
        ///     Gửi email thông qua tài khoản gmail
        /// </summary>
        /// <param name="from">Email người gửi</param>
        /// <param name="to">Email người nhận</param>
        /// <param name="cc">Danh sách email những người cùng nhận phân cách bởi dấu phẩy</param>
        /// <param name="bcc">Danh sách email những người cùng nhận phân cách bởi dấu phẩy</param>
        /// <param name="subject">Tiêu đề mail</param>
        /// <param name="body">Nội dung mail</param>
        /// <param name="attachments">Danh sách file định kèm phân cách bởi phẩy hoặc chấm phẩy</param>
        public static void Sends(string from, string to, string cc, string bcc, string subject, string body, string attachments)
        {
            Ssl = EnableSsl == "0" || EnableSsl == "true" || EnableSsl == "True" || EnableSsl == "TRUE";

            //System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            //bool result = regex.IsMatch(to);
            //if (result == false)
            //{
            //    //return "Địa chỉ email không hợp lệ.";
            // }

            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(@from)
            };
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;

            message.ReplyToList.Add(from);
            if (cc.Length > 0) message.CC.Add(cc);
            if (bcc.Length > 0) message.Bcc.Add(bcc);
            if (attachments.Length > 0)
            {
                var fileNames = attachments.Split(';', ',');
                foreach (var fileName in fileNames) message.Attachments.Add(new Attachment(fileName));
            }

            // Kết nối GMail
            using (var client = new SmtpClient(SmtpServer, Port)
            {
                Credentials = new NetworkCredential(CredentialUserName, CredentialPassword),
                EnableSsl = Ssl
            })
            {
                client.Send(message);
            }
        }

        public static void Send(string from, string to, string cc, string bcc, string subject, string body, string attachments)
        {
            var email = new Thread(delegate ()
            {
                SendAsyncEmail(@from, to, cc, bcc, subject, body, attachments);
            })
            { IsBackground = true };
            email.Start();
        }

        private static void SendAsyncEmail(string from, string to, string cc, string bcc, string subject, string body, string attachments)
        {
            try
            {
                //if (EnableSsl == "0" || EnableSsl == "true" || EnableSsl == "True" || EnableSsl == "TRUE")
                //    Ssl = true;
                //else
                //    Ssl = false;
                Ssl = EnableSsl == "0" || EnableSsl == "true" || EnableSsl == "True" || EnableSsl == "TRUE";

                var message = new MailMessage
                {
                    From = new MailAddress(@from),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.ReplyToList.Add(from);

                if (to != null)
                {
                    var toes = to.Split(';', ',', ' ');
                    foreach (var t in toes) message.To.Add(new MailAddress(t));
                }

                if (cc.Length > 0)
                {
                    var ccs = cc.Split(';', ',', ' ');
                    foreach (var c in ccs) message.CC.Add(new MailAddress(c));
                }

                if (bcc.Length > 0)
                {
                    var bccs = bcc.Split(';', ',', ' ');
                    foreach (var b in bccs) message.Bcc.Add(new MailAddress(b));
                }

                if (attachments.Length > 0)
                {
                    var fileNames = attachments.Split(';', ',');
                    foreach (var fileName in fileNames) message.Attachments.Add(new Attachment(fileName));
                }

                using (var client = new SmtpClient(SmtpServer, Port)
                {
                    Credentials = new NetworkCredential(CredentialUserName, CredentialPassword),
                    EnableSsl = Ssl,
                    Timeout = 100000
                })
                {
                    client.Send(message);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        public static void SendEmail(string subject, string destination, string body)
        {
            try
            {
                var email = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromEmailAddress"]),
                    destination)
                {
                    Subject = subject,
                    Body = body,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    Priority = MailPriority.High
                };

                using (var mailClient = new SmtpClient(Convert.ToString(ConfigurationManager.AppSettings["SMTPHost"]), Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]))
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["FromEmailAddress"]), Convert.ToString(ConfigurationManager.AppSettings["FromEmailPassword"])),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Timeout = 100000
                })
                {
                    mailClient.SendMailAsync(email);
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Helper()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
