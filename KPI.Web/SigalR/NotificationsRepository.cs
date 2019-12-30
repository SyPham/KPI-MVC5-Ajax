
using KPI.Model.DAO;
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KPI.Web
{
    public class NotificationsRepository
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["KPIDbContext"].ConnectionString;

        public IEnumerable<NotificationViewModel> GetAllNotifications(int UserID)
        {
            var messages = new List<NotificationViewModel>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                var sql = @"SELECT [ID]
                                  ,[UserID]
                                  ,[NotificationID]
                                  ,[Seen]
                                  ,[CreateTime]
                              FROM [KPI].[dbo].[NotificationDetails]";
                using (var command = new SqlCommand(sql, connection))
                {

                    //command.Parameters.AddWithValue("@UserID", UserID);
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        //messages.Add(item: new NotificationViewModel { ID = reader["ID"].ToInt() , UserID=reader["UserID"].ToInt(),Username=reader["Username"].ToSafetyString(), KPIName = reader["KPIName"].ToSafetyString(), Period =  reader["Period"].ToSafetyString(), Seen = reader["Seen"].ToBool(), Link = reader["Link"].ToSafetyString(), CreateTime = Convert.ToDateTime(reader["CreateTime"]), Tag = reader["Tag"].ToSafetyString(),Title=reader["Title"].ToSafetyString() });
                    }
                   // messages =await  new NotificationDAO().ListNotifications(UserID);
                }

            }

            return messages;
        }
       
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                NotificationHub.SendNotifications();
            }
        }
    }
}