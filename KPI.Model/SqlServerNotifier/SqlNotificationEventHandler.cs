using System.Data.SqlClient;

namespace KPI.Model.SqlServerNotifier
{
    public delegate void SqlNotificationEventHandler(object sender, SqlNotificationEventArgs e);
}