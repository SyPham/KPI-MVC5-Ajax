using KPI.Model;
using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Web.helpers;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KPI.Web.WindowService
{
    public class EmailJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //throw new NotImplementedException();
            string from = ConfigurationManager.AppSettings["FromEmailAddress"].ToSafetyString();
            string host = ConfigurationManager.AppSettings["Http"].ToSafetyString();

            string content2 = System.IO.File.ReadAllText(host + "/Templates/LateOnUpDateData.html");
            content2 = content2.Replace("{{{content}}}", "Your below KPIs have expired: ");

            string content = System.IO.File.ReadAllText(host + "/Templates/LateOnTask.html");
            content = content.Replace("{{{content}}}", "Your below KPIs have expired: ");
            var html = string.Empty;
            var count = 0;
            var model2 = new ActionPlanDAO().CheckLateOnUpdateData(1);
            var model = new ActionPlanDAO().CheckDeadline();
            if (await new SettingDAO().IsSendMail("CHECKLATEONUPDATEDATA"))
            {
                foreach (var item2 in model2.Item1)
                {
                    count++;
                    html += @"<tr>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{no}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{kpiname}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{year}}</td>
                             </tr>"
                            .Replace("{{no}}", count.ToSafetyString())
                            .Replace("{{kpiname}}", item2[0].ToSafetyString())
                            .Replace("{{year}}", item2[1].ToSafetyString());
                    content2 = content2.Replace("{{{html-template}}}", html);
                }
                 Commons.SendMail(model2.Item2.Select(x => x.Email).ToList(), "[KPI System] Late on upload data", content2, "Late on upload data");

            }

            if (await new SettingDAO().IsSendMail("CHECKDEADLINE"))
            {
                foreach (var item in model.Item1)
                {
                    //string content = "Please note that the action plan we are overdue on " + item.Deadline;
                    count++;
                    html += @"<tr>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{no}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{kpiname}}</td>
                            <td valign='top' style='padding:5px; font-family: Arial,sans-serif; font-size: 16px; line-height:20px;'>{{deadline}}</td>
                             </tr>"
                            .Replace("{{no}}", count.ToString())
                            .Replace("{{kpiname}}", item[0].ToSafetyString())
                            .Replace("{{deadline}}", item[1].ToSafetyString("MM/dd/yyyy"));
                    content = content.Replace("{{{html-template}}}", html);
                }
               Commons.SendMail(model.Item2.Select(x => x.Email).ToList(), "[KPI System] Late on task", content, "Late on task ");

            }
            var itemSendMail = new StateSendMail();
            await new NotificationDAO().AddSendMail(itemSendMail);
            var hh = ConfigurationManager.AppSettings["hh"].ToInt();
            var mm = ConfigurationManager.AppSettings["mm"].ToInt();
            var db = new KPIDbContext();
            var items = new ErrorMessage()
            {
                Function = "Test window service " + hh + ":" + mm,
                Name = "EmailJob"
            };
            db.ErrorMessages.Add(items);
            await db.SaveChangesAsync();
            db.Dispose();
        }


    }

}