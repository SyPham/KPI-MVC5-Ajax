using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KPI.Web.helpers
{
    public static class Commons
    {
        public static void SendMail(List<string> tos, string subject, string content, string errorTitle)
        {
            string title = ConfigurationManager.AppSettings["Title"].ToSafetyString();
            string EmailAddress = ConfigurationManager.AppSettings["EmailAddress"].ToSafetyString();
            string from = ConfigurationManager.AppSettings["FromEmailAddress"].ToSafetyString();

            tos.Add(EmailAddress);

            MailMessage mail = new MailMessage();
            foreach (var to in tos)
            {
                mail.To.Add(new MailAddress(to.ToSafetyString()));
            }
            mail.From = new MailAddress(from, title);
            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Priority = MailPriority.High;
      
            SendEmail(mail);
             
        }
        public static void SendMail(string from, List<string> tos, string subject, string content, string errorTitle)
        {
            string title = ConfigurationManager.AppSettings["Title"].ToSafetyString();
            string EmailAddress = ConfigurationManager.AppSettings["EmailAddress"].ToSafetyString();

            tos.Add(EmailAddress);

            MailMessage mail = new MailMessage();
            foreach (var to in tos)
            {
                mail.To.Add(new MailAddress(to.ToSafetyString()));
            }
            mail.From = new MailAddress(from, title);
            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Priority = MailPriority.High;

            try
            {
                SendEmail(mail);
               
                var errorMessage = new ErrorMessage();
                errorMessage.Name = errorMessage + " Sending mail successfully!";
                errorMessage.Function = errorTitle;
                errorMessage.CreateTime = DateTime.Now;
                new ErrorMessageDAO().Add(errorMessage);
            }
            catch (Exception ex)
            {
                var errorMessage = new ErrorMessage();
                errorMessage.Name = ex.Message;
                errorMessage.Function = errorTitle;
                errorMessage.CreateTime = DateTime.Now;
                new ErrorMessageDAO().Add(errorMessage);
            }
        }
        public static bool SendMail(string from, string to, string subject, string content, string errorTitle)
        {
            string title = ConfigurationManager.AppSettings["Title"].ToSafetyString();

            MailMessage mail = new MailMessage();
            mail.To.Add(to.ToString());
            mail.From = new MailAddress(from, title);
            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Priority = MailPriority.High;

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(mail);
                }
                var errorMessage = new ErrorMessage();
                errorMessage.Name = errorMessage + " Sending mail successfully!";
                errorMessage.Function = errorTitle;
                errorMessage.CreateTime = DateTime.Now;
                new ErrorMessageDAO().Add(errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                var errorMessage = new ErrorMessage();
                errorMessage.Name = ex.Message;
                errorMessage.Function = errorTitle;
                errorMessage.CreateTime = DateTime.Now;
                new ErrorMessageDAO().Add(errorMessage);
                return false;

            }
        }
        public static bool SendMail(string from, string to, string subject, string content, string errorTitle, List<string> CCs)
        {
            string title = ConfigurationManager.AppSettings["Title"].ToSafetyString();

            MailMessage mail = new MailMessage();
            mail.To.Add(to.ToString());
            mail.From = new MailAddress(from, title);
            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Priority = MailPriority.High;
            foreach (var cc in CCs)
            {
                mail.CC.Add(new MailAddress(cc.ToSafetyString()));
            }
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(mail);
                }
                var errorMessage = new ErrorMessage();
                errorMessage.Name = errorMessage + " Sending mail successfully!";
                errorMessage.Function = errorTitle;
                errorMessage.CreateTime = DateTime.Now;
                new ErrorMessageDAO().Add(errorMessage);
                return true;
            }
            catch (Exception ex)
            {
                var errorMessage = new ErrorMessage();
                errorMessage.Name = ex.Message;
                errorMessage.Function = errorTitle;
                errorMessage.CreateTime = DateTime.Now;
                new ErrorMessageDAO().Add(errorMessage);
                return false;

            }
        }
        public static void SendEmail(MailMessage m)
        {
            SendEmail(m, true);
        }
        public static void SendEmail(MailMessage m, Boolean Async)
        {
            SmtpClient smtp = null;
            smtp = new SmtpClient();

            if (Async)
            {
                SendEmailDelegate sd = new SendEmailDelegate(smtp.Send);
                AsyncCallback cb = new AsyncCallback(SendEmailResponse);
                sd.BeginInvoke(m, cb, sd);
            }
            else
            {
                smtp.Send(m);
            }

        }
        private delegate void SendEmailDelegate(MailMessage m);
        private static void SendEmailResponse(IAsyncResult ar)
        {
            SendEmailDelegate sd = (SendEmailDelegate)(ar.AsyncState);
            try
            {
                sd.EndInvoke(ar);
             
                var errorMessage = new ErrorMessage();
                errorMessage.Name = "Sending mail successfully!";
                errorMessage.Function = "Sending mail";
                errorMessage.CreateTime = DateTime.Now;
                new ErrorMessageDAO().Add(errorMessage);
            }
            catch (Exception ex)
            {
                var errorMessage = new ErrorMessage();
                errorMessage.Name = ex.Message;
                errorMessage.Function = "Sending mail";
                errorMessage.CreateTime = DateTime.Now;
                new ErrorMessageDAO().Add(errorMessage);
            }
        }
    }
}