
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KPI.Web
{
    public class CommentRepository
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["KPIDbContext"].ConnectionString;

        public IEnumerable<CommentVM> GetAllComments(int userid, int dataid)
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
                                      ,(SELECT ID FROM SeenComments WHERE CommentID = Comments.ID AND UserID = @UserID ) AS Status
                                      ,(SELECT ID FROM ActionPlans WHERE DataID = @DataID AND CommentID = Comments.ID ) AS IsHasTask
                              FROM Comments
                              INNER JOIN dbo.Users on dbo.Users.ID = Comments.UserID
                              INNER JOIN dbo.Data on dbo.Comments.DataID = Data.ID
                              WHERE Data.ID = @DataID
                              ORDER BY CommentedDate DESC";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userid);
                    command.Parameters.AddWithValue("@DataID", dataid);

                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var task = reader["IsHasTask"].ToInt();

                        messages.Add(item: new CommentVM { CommentID = reader["ID"].ToInt(), UserID = reader["UserID"].ToInt(), FullName = reader["FullName"].ToSafetyString(), CommentedDate = Convert.ToDateTime(reader["CommentedDate"]), CommentMsg = reader["CommentMsg"].ToSafetyString(), Read = reader["Status"].ToInt()==0?true:false, Task = task > 0 ? true : false });
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