
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
    public class ActionPlanRepository
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["KPIDbContext"].ConnectionString;

        public IEnumerable<CommentVM> GetAllComments(int userid, int dataid,int commentid)
        {
            var messages = new List<CommentVM>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sql = @"SELECT Comments.ID
                                      ,CommentMsg
                                      ,CommentedDate
                                      ,UserID
                                      ,DataID
	                                  ,FullName
                                      ,(SELECT Status FROM SeenComments WHERE CommentID = @CommentID AND UserID = @UserID ) AS Status
                                      ,(SELECT ID FROM ActionPlans WHERE DataID = @DataID AND CommentID = @CommentID ) AS IsHasTask
                              FROM Comments 
                              INNER JOIN dbo.Users on dbo.Users.ID = Comments.UserID
                              INNER JOIN dbo.Data on dbo.Comments.DataID = Data.ID
                              WHERE Comments.UserID = @UserID and Data.ID = @DataID";
                using (var command = new SqlCommand(sql, connection))
                {

                    command.Parameters.AddWithValue("@UserID", userid);
                    command.Parameters.AddWithValue("@DataID", dataid);
                    command.Parameters.AddWithValue("@CommentID", commentid);
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        messages.Add(item: new CommentVM { CommentID = reader["ID"].ToInt(), UserID = reader["UserID"].ToInt(), FullName = reader["FullName"].ToSafetyString(), CommentedDate = Convert.ToDateTime(reader["CommentedDate"]), CommentMsg = reader["CommentMsg"].ToSafetyString(),Read = reader["Status"].ToBool(),Task = reader["IsHasTask"].ToInt()>0? true : false });
                    }
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