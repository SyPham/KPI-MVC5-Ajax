using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.SqlServerNotifier;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
    public class NotificationDAO
    {
        private KPIDbContext _dbContext = null;
        public NotificationDAO()
        {
            _dbContext = new KPIDbContext();
        }
        public async Task<bool> UpdateRange(string listID)
        {
            if (listID == null) return false;
            if (listID.Length > 0)
            {
                var arr = listID.Split(',').Select(Int32.Parse).ToList();
                var some = await _dbContext.NotificationDetails.Where(x => arr.Contains(x.ID)).ToListAsync();
                some.ForEach(a => a.Seen = true);
                try
                {
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;

                }
            }

            return false;

        }

        public async Task<object> Update(int ID)
        {
            var some = _dbContext.NotificationDetails.FirstOrDefault(x => x.ID == ID);
            var user = _dbContext.Users;
            var kpilevel = _dbContext.KPILevels;
            try
            {
                some.Seen = true;
                _dbContext.SaveChanges();
                //var detail = _dbContext.Notifications.FirstOrDefault(x => x.ID == some.NotificationID);

                var detail = await _dbContext.Notifications.FirstOrDefaultAsync(x => x.ID == some.NotificationID);
                var tag = await _dbContext.Tags.Where(x => x.CommentID == detail.CommentID).Select(x => x.UserID).ToListAsync();
                var listArr = string.Empty;
                if (tag.Count > 0)
                {
                    listArr = string.Join(",", user.Where(x => tag.Contains(x.ID)).Select(x => x.FullName).ToArray()); ;
                }
                var vm = new NotificationViewModel();
                vm.ID = detail.ID;
                vm.Title = detail.Title;
                vm.CreateTime = detail.CreateTime;
                vm.Link = detail.Link;
                vm.Tag = listArr;
                vm.Content = detail.Content;
                vm.Sender = user.Find(detail.UserID).FullName;
                vm.Recipient = listArr;
                vm.Content = detail.Content;
                vm.Title = detail.Title;
                vm.KPIName = detail.KPIName;
                vm.Owner = kpilevel.FirstOrDefault(a => a.KPILevelCode == detail.KPILevelCode).Owner;
                vm.Manager = kpilevel.FirstOrDefault(a => a.KPILevelCode == detail.KPILevelCode).OwnerManagerment;
                //vm.PIC = kpilevel.FirstOrDefault(a => a.KPILevelCode == detail.KPILevelCode).PIC;
                vm.PIC = detail.UserID;
                if (detail.ActionplanID > 0)
                {
                    vm.DueDate = _dbContext.ActionPlans.FirstOrDefault(x => x.ID == detail.ActionplanID).Deadline.ToString("MM/dd/yyyy");
                }

                return new { status = true, data = vm };

            }
            catch (Exception)
            {
                return new { status = false, data = "" }; ;
            }

        }

        public bool IsExistsActionPlanNotification(int userID, int actionPlanID)
        {
            var model = _dbContext.Notifications.FirstOrDefault(x => x.ActionplanID == actionPlanID && x.UserID == userID);
            if (model == null)
                return false;
            else return true;
        }
        public bool IsExistsCommentNotification(int userID, int commentID)
        {
            var model = _dbContext.Notifications.FirstOrDefault(x => x.CommentID == commentID && x.UserID == userID);
            if (model == null)
                return false;
            else return true;
        }
        public async Task<bool> Add(Notification entity)
        {
            try
            {
                //Add thong bao
                _dbContext.Notifications.Add(entity);
                await _dbContext.SaveChangesAsync();
                var listUserID = new List<int>();
                var listDetails = new List<NotificationDetail>();

                var user = _dbContext.Users;
                var tag = _dbContext.Tags;

                //Neu thong bao cua comment thi vao bang tag lay tat ca user dc tag ra
                if (entity.CommentID > 0)
                {
                    listUserID.AddRange(await tag.Where(x => x.CommentID == entity.CommentID).Select(x => x.UserID).ToArrayAsync());
                }
                var itemActionPlan = await _dbContext.Notifications.FirstOrDefaultAsync(x => x.UserID == entity.UserID && x.Action == entity.Action && x.ActionplanID == entity.ActionplanID);
                //Neu thong bao cua actioPlan thi vao bang tag lay tat ca user dc tag ra
                if (entity.ActionplanID > 0)
                {
                    listUserID.AddRange(await tag.Where(x => x.ActionPlanID == entity.ActionplanID).Select(x => x.UserID).ToArrayAsync());
                }

                //Neu co tag thi them vao bang NotificationDetail
                if (listUserID.Count > 0)
                {
                    foreach (var item in listUserID)
                    {
                        var detail = new NotificationDetail();
                        detail.UserID = item;
                        detail.Seen = false;
                        detail.NotificationID = entity.ID;
                        listDetails.Add(detail);
                    }
                    _dbContext.NotificationDetails.AddRange(listDetails);
                    await _dbContext.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddNotificationWorkplace(Notification entity)
        {
            try
            {
                entity.CreateTime = DateTime.Now;
                _dbContext.Notifications.Add(entity);
                _dbContext.SaveChanges();
                var listDetails = new List<NotificationDetail>();
                var kpiLevel = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == entity.KPILevelCode);
                var listID = new List<int> { entity.UserID, kpiLevel.Owner, kpiLevel.OwnerManagerment };
                foreach (var item in listID)
                {
                    var detail = new NotificationDetail();
                    detail.UserID = item;
                    detail.Seen = false;
                    detail.NotificationID = entity.ID;
                    listDetails.Add(detail);
                }
                _dbContext.NotificationDetails.AddRange(listDetails);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<IEnumerable<Notification>> NotifyCollection() => await _dbContext.Notifications.ToListAsync();


        public object Notification(int userid, DateTime notificationRegisterTime)
        {

            var user = _dbContext.Users;
            var username = user.FirstOrDefault(x => x.ID == userid).Username.ToSafetyString();
            var notifications = _dbContext.Notifications
                .Where(x => x.Tag.Contains(username) && x.CreateTime > notificationRegisterTime)
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            var list = new List<NotificationViewModel>();
            var period = string.Empty;
            foreach (var item in notifications)
            {
                var notification = new NotificationViewModel();
                notification.Link = item.Link;

                switch (item.Period)
                {
                    case "W":
                        period = "Weekly"; break;
                    case "M":
                        period = "Monthly"; break;
                    case "Q":
                        period = "Quarterly"; break;
                    case "Y":
                        period = "Yearly"; break;
                    default:
                        period = "...";
                        break;
                }

                //notification.Content = '@' + user.FirstOrDefault(x => x.ID == item.UserID).Username + " mentioned you in a comment of " + item.KPIName + " - " + period;
                //notification.Seen = item.Seen;
                list.Add(notification);
            }
            return new
            {
                data = list.ToList()
            };

        }

        public async Task<List<NotificationViewModel>> ListNotifications(int userid)
        {
            var kpilevel = _dbContext.KPILevels;
            var kpilevel2 = _dbContext.Notifications.ToList();
            var model = (await (from notify in _dbContext.Notifications
                                join notifyDetail in _dbContext.NotificationDetails.Where(x => x.UserID == userid) on notify.ID equals notifyDetail.NotificationID
                                select new
                                {
                                    NotificationID = notify.ID,
                                    notifyDetail.ID,
                                    notify.Title,
                                    notify.KPIName,
                                    notify.Period,
                                    ContentDetail = notifyDetail.Content,
                                    notifyDetail.URL,
                                    notifyDetail.CreateTime,
                                    notify.Link,
                                    notifyDetail.Seen,
                                    notify.Tag,
                                    notify.Content,
                                    notify.KPILevelCode,
                                    notify.Action,
                                    SenderID = notify.UserID,
                                    RecipientID = notifyDetail.UserID,
                                    notify.TaskName
                                }).ToListAsync())
           .OrderByDescending(x => x.CreateTime)
           .Select(x => new NotificationViewModel
           {
               ID = x.ID,
               NotificationID = x.NotificationID,
               Title = x.Title,
               KPIName = _dbContext.KPIs.FirstOrDefault(kpi => kpi.ID == kpilevel.FirstOrDefault(a => a.KPILevelCode == x.KPILevelCode).KPIID).Name,
               Period = x.Period,
               Sender = _dbContext.Users.FirstOrDefault(a => a.ID == x.SenderID)?.Alias,
               Recipient = _dbContext.Users.FirstOrDefault(a => a.ID == x.RecipientID)?.Alias,
               SenderID = x.SenderID,
               RecipientID = x.RecipientID,
               CreateTime = x.CreateTime,
               Link = x.Link,
               Seen = x.Seen,
               Tag = x.Tag,
               Action = x.Action,
               TaskName = x.TaskName,
               Content = x.Content.ToSafetyString(),
               ContentDetail = x.ContentDetail,
               URL = x.URL
           }).ToList();

            return model;
        }
        public async Task<bool> IsSend()
        {
            TimeSpan timespan = new TimeSpan(00, 00, 00);
            DateTime today = DateTime.Today.Add(timespan);
            return await _dbContext.StateSendMails.FirstOrDefaultAsync(x => x.ToDay == today) == null ? false : true;
        }
        public async Task<bool> IsSended()
        {
            return await _dbContext.StateSendMails.AnyAsync(x => x.ToDay == DateTime.Today);
        }
        public async Task<bool> AddSendMail(StateSendMail stateSendMail)
        {
            try
            {
                _dbContext.StateSendMails.Add(stateSendMail);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public object GetNotification(int userid)
        {

            var user = _dbContext.Users;
            var username = user.FirstOrDefault(x => x.ID == userid).Username.ToSafetyString();
            var notifications = _dbContext.Notifications
                .Where(x => x.Tag.Contains(username))
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            var list = new List<NotificationViewModel>();
            var period = string.Empty;
            foreach (var item in notifications)
            {
                var notification = new NotificationViewModel();
                notification.Link = item.Link;

                switch (item.Period)
                {
                    case "W":
                        period = "Weekly"; break;
                    case "M":
                        period = "Monthly"; break;
                    case "Q":
                        period = "Quarterly"; break;
                    case "Y":
                        period = "Yearly"; break;
                    default:
                        period = "...";
                        break;
                }

                //notification.Content = '@' + user.FirstOrDefault(x => x.ID == item.UserID).Username + " mentioned you in a comment of " + item.KPIName + " - " + period;
                //notification.Seen = item.Seen;
                list.Add(notification);
            }
            return new
            {
                total = list.Count,
                data = list.ToList()
            };

        }
        public List<NotificationViewModel> GetHistoryNotification(int userid)
        {

            var actionPlan = _dbContext.ActionPlans;
            var model = from notify in _dbContext.Notifications
                        join notifyDetail in _dbContext.NotificationDetails on notify.ID equals notifyDetail.NotificationID
                        where notifyDetail.UserID == userid
                        join recipient in _dbContext.Users on notifyDetail.UserID equals recipient.ID // recipient
                        join sender in _dbContext.Users on notify.UserID equals sender.ID //sender
                        select new NotificationViewModel
                        {
                            ID = notifyDetail.ID,
                            Title = notify.Title,
                            KPIName = notify.KPIName,
                            Period = notify.Period,
                            CreateTime = notifyDetail.CreateTime,
                            RecipientID = recipient.ID,
                            Recipient = recipient.Alias,
                            Link = notify.Link,
                            Seen = notifyDetail.Seen,
                            Tag = notify.Tag,
                            Deadline = (DateTime?)actionPlan.FirstOrDefault(x => x.ID == notify.ActionplanID).Deadline ?? new DateTime(2001, 1, 1),
                            Sender = sender.Alias,
                            SenderID = notify.ID,
                            Action = notify.Action,
                            Content = notify.Content
                        };
            var model1 = model.OrderByDescending(x => x.CreateTime).ToList();
            return model1;
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
