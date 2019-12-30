using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace KPI.Model.helpers
{
    public class MailUtility
    {
        #region Static Method
        public static bool Send(string from, string password, string to, string subject, string content, int port, string server, bool ssl)
        {
            //Tạo ra 1 mail mới
            MailMessage mail = new MailMessage();

            //Điền các thông tin lên lá thư
            mail.From = new MailAddress(from);
            mail.To.Add(to); // Email cần đến
            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Priority = MailPriority.High;
          

            //Nhờ người đưa thư
            SmtpClient smtpServer = new SmtpClient(server);

            //Chỉ định người đưa thư phù hợp
            smtpServer.Port = port; //Do quy định
            smtpServer.Credentials = new NetworkCredential(from, password);
            smtpServer.EnableSsl = ssl;
            smtpServer.UseDefaultCredentials = true;
            

            //Gửi thư
            try
            {
                smtpServer.Send(mail);
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        #endregion

        #region Properties
        public string From { get; set; }

        public string Password { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public int Port { get; set; }

        public string Server { get; set; }

        public bool SSl { get; set; }
        #endregion

        #region Method

        public bool Send()
        {
            return Send(From, Password, To, Subject, Content, Port, Server, SSl);

        }
        #endregion

        #region Constructor //Hàm khơi tạo

        public MailUtility()
        {

        }

        public MailUtility(string from, string password)//Ham co tham so
        {
            From = from;
            Password = password;
        }
        #endregion
    }
}
