using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using KPI.Model.ViewModel.EmailViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
    public class ActionPlanDAO
    {
        KPIDbContext _dbContext = null;
        public ActionPlanDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        /// <summary>
        /// Khi thêm 1 comment nếu tag nhiều user thì lưu vào bảng Tag đồng thời lưu vào bảng Notification để thông báo đẩy
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="subject"></param>
        /// <param name="auditor"></param>
        /// <returns></returns>
        public async Task<AddCommentVM> Add(AddActionPlanViewModel obj)//(ActionPlan entity, string subject, string auditor,int catid)
        {
            var subject = obj.Subject;
            var auditor = obj.Auditor;
            var kpilevelcode = obj.KPILevelCode;
            var catid = obj.CategoryID;
            var kpilevelModel = await _dbContext.KPILevels.FirstOrDefaultAsync(x => x.KPILevelCode == kpilevelcode);
            if(kpilevelModel == null)
                return new AddCommentVM
                {
                    Status = false,
                    ListEmails = new List<string[]>(),
                    Message = "Error!",
                };
            //Kiem tra neu la owner thi moi cho add task(actionPlan)
            if (await _dbContext.Owners.FirstOrDefaultAsync(x => x.CategoryID == catid && x.UserID == obj.OwnerID && x.KPILevelID == kpilevelModel.ID) == null)
                return new AddCommentVM
                {
                    Status = false,
                    ListEmails = new List<string[]>(),
                    Message = "You are not Owner of this KPI.",
                };
            else
            {
                var entity = new ActionPlan();
                entity.Title = obj.Title;
                entity.Description = obj.Description;
                entity.KPILevelCodeAndPeriod = obj.KPILevelCodeAndPeriod;
                entity.KPILevelCode = obj.KPILevelCode;
                entity.Tag = obj.Tag;
                entity.UserID = obj.UserID;
                entity.DataID = obj.DataID;
                entity.CommentID = obj.CommentID;
                entity.Link = obj.Link;
                entity.SubmitDate = obj.SubmitDate.ToDateTime();
                entity.Deadline = obj.Deadline.ToDateTime();

                var user = _dbContext.Users;
                var itemActionPlanDetail = new ActionPlanDetail();
                var listEmail = new List<string[]>();
                var listUserID = new List<int>();
                var listFullNameTag = new List<string>();
                var listTags = new List<Tag>();
                var itemTag = _dbContext.Tags;

                try
                {

                    if (!entity.Description.IsNullOrEmpty())
                    {
                        if (entity.Description.IndexOf(";") == -1)
                        {
                            entity.Description = entity.Description;

                        }
                        else
                        {
                            var des = string.Empty;
                            entity.Description.Split(';').ToList().ForEach(line =>
                            {
                                des += line + "&#13;&#10;";
                            });
                            entity.Description = des;
                        }
                    }
                    else
                    {
                        entity.Description = "#N/A";
                    }
                    if (!auditor.IsNullOrEmpty())
                    {
                        var userResult = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == auditor);
                        entity.Auditor = userResult.ID;
                    }
                    _dbContext.ActionPlans.Add(entity);
                    await _dbContext.SaveChangesAsync();

                    if (!entity.Tag.IsNullOrEmpty())
                    {
                        string[] arrayString = new string[5];


                        if (entity.Tag.IndexOf(",") == -1)
                        {
                            var userItem = await user.FirstOrDefaultAsync(x => x.Username == entity.Tag);

                            if (userItem != null)
                            {
                                var tag = new Tag();
                                tag.ActionPlanID = entity.ID;
                                tag.UserID = userItem.ID;
                                _dbContext.Tags.Add(tag);
                                await _dbContext.SaveChangesAsync();

                                arrayString[0] = user.FirstOrDefault(x => x.ID == entity.UserID).FullName;
                                arrayString[1] = userItem.Email;
                                arrayString[2] = entity.Link;
                                arrayString[3] = entity.Title;
                                arrayString[4] = entity.Description;
                                listFullNameTag.Add(userItem.FullName);
                                listEmail.Add(arrayString);
                            }
                        }
                        else
                        {
                            var list = entity.Tag.Split(',');
                            var listUsers = await _dbContext.Users.Where(x => list.Contains(x.Username)).ToListAsync();
                            foreach (var item in listUsers)
                            {
                                var tag = new Tag();
                                tag.ActionPlanID = entity.ID;
                                tag.UserID = item.ID;
                                listTags.Add(tag);

                                arrayString[0] = user.FirstOrDefault(x => x.ID == entity.UserID).FullName;
                                arrayString[1] = item.Email;
                                arrayString[2] = entity.Link;
                                arrayString[3] = entity.Title;
                                arrayString[4] = entity.Description;
                                listFullNameTag.Add(item.FullName);
                                listEmail.Add(arrayString);
                            }
                            _dbContext.Tags.AddRange(listTags);
                            await _dbContext.SaveChangesAsync();
                        }
                    }


                    //Add vao Notification
                    var notify = new Notification();
                    notify.ActionplanID = entity.ID;
                    notify.Content = entity.Description;
                    notify.UserID = entity.UserID;
                    //notify.Title = entity.Title;
                    notify.Link = entity.Link;
                    notify.Tag = string.Join(",", listFullNameTag);
                    notify.Title = subject;
                    notify.Action = "Task";
                    notify.TaskName = entity.Title;
                    await new NotificationDAO().Add(notify);

                    //add vao user


                    return new AddCommentVM
                    {
                        Status = true,
                        ListEmails = listEmail
                    };
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return new AddCommentVM
                    {
                        Status = true,
                        ListEmails = listEmail
                    };
                }
            }


        }
        public async Task<bool> Update(ActionPlan entity)
        {
            try
            {
                var item = await _dbContext.ActionPlans.FindAsync(entity.ID);
                item.Title = entity.Title;
                item.Description = entity.Description;
                item.Tag = entity.Tag;
                item.Deadline = entity.Deadline;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }
        public bool Approve(ActionPlan entity)
        {
            try
            {
                var item = _dbContext.ActionPlans.Find(entity.ID);
                item.ApprovedBy = entity.ApprovedBy;
                item.ApprovedStatus = entity.ApprovedStatus;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }
        public async Task<List<ActionPlanTagVM>> GetActionPlanByUserID(int userID)
        {
            var data = await _dbContext.ActionPlans
                        .Join(_dbContext.Tags,
                        acp => acp.ID,
                        tag => tag.ActionPlanID,
                        (acp, tag) => new
                        {
                            acp,
                            tag
                        })
                        .Select(x => new
                        {
                            TaskName = x.acp.Title,
                            Description = x.acp.Description,
                            DuaDate = x.acp.Deadline,
                            UpdateSheuleDate = x.acp.UpdateSheduleDate,
                            ActualFinishDate = x.acp.ActualFinishDate,
                            Status = x.acp.Status,
                            Approved = x.acp.ApprovedStatus,
                            UserTag = x.tag.UserID

                        }).ToListAsync();
            var model = data
                        .Select(x => new ActionPlanTagVM
                        {
                            TaskName = x.TaskName,
                            Description = x.Description,
                            DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                            UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                            ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                            Status = x.Status,
                            Approved = x.Approved,
                            UserTag = x.UserTag
                        }).Where(x => x.UserTag == userID).ToList();

            return model;
        }
        public List<ActionPlanTagVM> GetActionPlanByUser(int userID, string role)
        {
            var model = new List<ActionPlanTagVM>();

            switch (role.ToSafetyString().ToUpper())
            {
                case "MAN":
                    var manager = _dbContext.Managers.Where(x => x.UserID == userID).DistinctBy(x => x.KPILevelID).ToList();
                    model = _dbContext.ActionPlans
                      .Join(_dbContext.Tags,
                      acp => acp.ID,
                      tag => tag.ActionPlanID,
                      (acp, tag) => new
                      {
                          acp,
                          tag
                      })
                      .Select(x => new
                      {
                          TaskName = x.acp.Title,
                          Description = x.acp.Description,
                          DuaDate = x.acp.Deadline,
                          UpdateSheuleDate = x.acp.UpdateSheduleDate,
                          ActualFinishDate = x.acp.ActualFinishDate,
                          Status = x.acp.Status,
                          Approved = x.acp.ApprovedStatus,
                          UserTag = x.tag.UserID

                      }).AsEnumerable()
                      .Select(x => new ActionPlanTagVM
                      {
                          TaskName = x.TaskName,
                          Description = x.Description,
                          DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                          UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                          ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                          Status = x.Status,
                          Approved = x.Approved,
                          UserTag = x.UserTag
                      }).Where(x => x.UserTag == userID).ToList();
                    break;
                case "OWN":

                    model = _dbContext.ActionPlans
                      .Join(_dbContext.Tags,
                      acp => acp.ID,
                      tag => tag.ActionPlanID,
                      (acp, tag) => new
                      {
                          acp,
                          tag
                      })
                      .Select(x => new
                      {
                          TaskName = x.acp.Title,
                          Description = x.acp.Description,
                          DuaDate = x.acp.Deadline,
                          UpdateSheuleDate = x.acp.UpdateSheduleDate,
                          ActualFinishDate = x.acp.ActualFinishDate,
                          Status = x.acp.Status,
                          Approved = x.acp.ApprovedStatus,
                          UserTag = x.tag.UserID

                      }).AsEnumerable()
                      .Select(x => new ActionPlanTagVM
                      {
                          TaskName = x.TaskName,
                          Description = x.Description,
                          DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                          UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                          ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                          Status = x.Status,
                          Approved = x.Approved,
                          UserTag = x.UserTag
                      }).Where(x => x.UserTag == userID).ToList();
                    break;
                case "UPD":

                    model = _dbContext.ActionPlans
                      .Join(_dbContext.Tags,
                      acp => acp.ID,
                      tag => tag.ActionPlanID,
                      (acp, tag) => new
                      {
                          acp,
                          tag
                      })
                      .Select(x => new
                      {
                          TaskName = x.acp.Title,
                          Description = x.acp.Description,
                          DuaDate = x.acp.Deadline,
                          UpdateSheuleDate = x.acp.UpdateSheduleDate,
                          ActualFinishDate = x.acp.ActualFinishDate,
                          Status = x.acp.Status,
                          Approved = x.acp.ApprovedStatus,
                          UserTag = x.tag.UserID

                      }).AsEnumerable()
                      .Select(x => new ActionPlanTagVM
                      {
                          TaskName = x.TaskName,
                          Description = x.Description,
                          DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                          UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                          ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                          Status = x.Status,
                          Approved = x.Approved,
                          UserTag = x.UserTag
                      }).Where(x => x.UserTag == userID).ToList();
                    break;
                case "SPO":

                    model = _dbContext.ActionPlans
                      .Join(_dbContext.Tags,
                      acp => acp.ID,
                      tag => tag.ActionPlanID,
                      (acp, tag) => new
                      {
                          acp,
                          tag
                      })
                      .Select(x => new
                      {
                          TaskName = x.acp.Title,
                          Description = x.acp.Description,
                          DuaDate = x.acp.Deadline,
                          UpdateSheuleDate = x.acp.UpdateSheduleDate,
                          ActualFinishDate = x.acp.ActualFinishDate,
                          Status = x.acp.Status,
                          Approved = x.acp.ApprovedStatus,
                          UserTag = x.tag.UserID

                      }).AsEnumerable()
                      .Select(x => new ActionPlanTagVM
                      {
                          TaskName = x.TaskName,
                          Description = x.Description,
                          DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                          UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                          ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                          Status = x.Status,
                          Approved = x.Approved,
                          UserTag = x.UserTag
                      }).Where(x => x.UserTag == userID).ToList();
                    break;
                case "PAR":

                    model = _dbContext.ActionPlans
                      .Join(_dbContext.Tags,
                      acp => acp.ID,
                      tag => tag.ActionPlanID,
                      (acp, tag) => new
                      {
                          acp,
                          tag
                      })
                      .Select(x => new
                      {
                          TaskName = x.acp.Title,
                          Description = x.acp.Description,
                          DuaDate = x.acp.Deadline,
                          UpdateSheuleDate = x.acp.UpdateSheduleDate,
                          ActualFinishDate = x.acp.ActualFinishDate,
                          Status = x.acp.Status,
                          Approved = x.acp.ApprovedStatus,
                          UserTag = x.tag.UserID

                      }).AsEnumerable()
                      .Select(x => new ActionPlanTagVM
                      {
                          TaskName = x.TaskName,
                          Description = x.Description,
                          DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                          UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                          ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                          Status = x.Status,
                          Approved = x.Approved,
                          UserTag = x.UserTag
                      }).Where(x => x.UserTag == userID).ToList();
                    break;
                default:
                    break;
            }


            return model;
        }
        public int Total()
        {
            return _dbContext.ActionPlans.Count();
        }
        public List<ActionPlan> GetActionPlanCode()
        {
            return _dbContext.ActionPlans.ToList();
        }
        public List<ActionPlan> GetActionPlanByCommentID(int commentID)
         => _dbContext.ActionPlans.Where(x => x.CommentID == commentID).ToList();
        public async Task<bool> Delete(int id)
        {
            try
            {
                var category = await _dbContext.ActionPlans.FindAsync(id);
                _dbContext.ActionPlans.Remove(category);
                await _dbContext.SaveChangesAsync();

                var notifications = await _dbContext.Notifications.FirstOrDefaultAsync(x => x.ActionplanID == category.ID);

                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }
        public async Task<AddActionPlanViewModel> GetByID(int id)
        {
            var modal = await _dbContext.ActionPlans.FirstOrDefaultAsync(x => x.ID == id);
            var vm = new AddActionPlanViewModel();
            vm.ID = modal.ID;
            vm.Title = modal.Title;
            vm.Deadline = modal.Deadline.ToString("yyyy-MM-dd");
            vm.Description = modal.Description;
            vm.Tag = modal.Tag;
            return vm;

        }
        public async Task<Tuple<List<string[]>, bool>> Approval(int id, int aproveBy, string KPILevelCode, int CategoryID)
        {
            var listTags = new List<Tag>();
            var listEmail = new List<string[]>();
            var user = _dbContext.Users;
            var model = await _dbContext.ActionPlans.FirstOrDefaultAsync(x => x.ID == id);
            model.ApprovedBy = aproveBy;
            model.ApprovedStatus = !model.ApprovedStatus;
            if (model.ApprovedStatus == true)
                model.Status = true;
            else
                model.Status = false;

            try
            {

                //Thong bao den nhung user duoc de cap trong tag
                //Nguoi nao xet duyet thi khong can thong bao den chinh ho

                //var relatedUsers = await _dbContext.Tags.Where(x => x.ActionPlanID == id && x.UserID != aproveBy).ToListAsync();
                //relatedUsers.ForEach(item =>
                //{

                //    var tag = new Tag();
                //    tag.UserID = item.UserID;
                //    tag.ActionPlanID = model.ID;
                //    listTags.Add(tag);
                //});

                //_dbContext.Tags.AddRange(listTags);
                await _dbContext.SaveChangesAsync();
                //Add vao Notification
                var tags = await _dbContext.Tags.Where(x => x.ActionPlanID == model.ID).ToListAsync();
                foreach (var tag in tags)
                {
                    string[] arrayString = new string[5];
                    arrayString[0] = user.Find(aproveBy).Alias; //Bi danh
                    arrayString[1] = user.Find(tag.UserID).Email;
                    arrayString[2] = "Approve Task";
                    arrayString[3] = model.Title;
                    listEmail.Add(arrayString);
                }
                var notify = new Notification();
                notify.ActionplanID = model.ID;
                notify.Content = model.Description;
                notify.UserID = aproveBy; //Nguoi xet duyet
                notify.Title = model.Title;
                notify.Link = model.Link;
                notify.TaskName = model.Title;

                if (model.Status == false && model.ApprovedStatus == false)
                {
                    notify.Action = "UpdateApproval";
                }
                else
                {
                    notify.Action = "Approval";
                }

                await new NotificationDAO().Add(notify);
                return Tuple.Create(listEmail, true);
            }
            catch (Exception ex)
            {
                var a = new ErrorMessage();
                a.Name = ex.Message;
                a.Function = "Approval";
                new ErrorMessageDAO().Add(a);
                return Tuple.Create(new List<string[]>(), true);
            }
        }
        public async Task<Tuple<List<string[]>, bool>> Done(int id, int userid, string KPILevelCode, int CategoryID)
        {
            var listTags = new List<Tag>();
            var model = await _dbContext.ActionPlans.FindAsync(id);
            var listEmail = new List<string[]>();
            var user = _dbContext.Users;
            //Chua duyet thi moi cho update lai status
            if (!model.ApprovedStatus)
            {
                //B1: Update status xong thi thong bao den cac user lien quan va owner
                model.Status = !model.Status;
                if (userid == model.UserID)
                {
                    model.UpdateSheduleDate = DateTime.Now;
                }
                else
                {
                    model.ActualFinishDate = DateTime.Now;

                }
                //B2: Thong bao den owner
                var kpiLevel = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == KPILevelCode);
                var owners = await _dbContext.Owners.Where(x => x.CategoryID == CategoryID && x.KPILevelID == kpiLevel.ID).ToListAsync();
                owners.ForEach(item =>
                {
                    var tag = new Tag();
                    tag.UserID = item.UserID;
                    tag.ActionPlanID = model.ID;
                    listTags.Add(tag);


                });

                try
                {
                    _dbContext.Tags.AddRange(listTags);
                    var tags = await _dbContext.Tags.Where(x => x.ActionPlanID == model.ID).ToListAsync();
                    foreach (var tag in tags)
                    {
                        string[] arrayString = new string[5];
                        arrayString[0] = user.Find(userid).Alias; //Bi danh
                        arrayString[1] = user.Find(tag.UserID).Email;
                        arrayString[2] = "Update Status Task";
                        arrayString[3] = model.Title;
                        listEmail.Add(arrayString);
                    }
                    await _dbContext.SaveChangesAsync();
                    //Add vao Notification
                    var notify = new Notification();
                    notify.ActionplanID = model.ID;
                    notify.Content = model.Description;
                    notify.UserID = userid;//Nguoi update Status task
                    notify.Title = model.Title;
                    notify.Link = model.Link;
                    notify.TaskName = model.Title;
                    notify.Action = "Done";
                    await new NotificationDAO().Add(notify);
                    return Tuple.Create(listEmail, true);
                }
                catch (Exception ex)
                {
                    var a = new ErrorMessage();
                    a.Name = ex.Message;
                    a.Function = "Done";
                    new ErrorMessageDAO().Add(a);
                    return Tuple.Create(new List<string[]>(), false);
                }
            }
            else
            {
                return Tuple.Create(new List<string[]>(), false);
            }
        }
        public async Task<object> GetAll(int DataID, int CommentID, int userid)
        {
            var userModel = await _dbContext.Users.FirstOrDefaultAsync(x => x.ID == userid);
            var permission = _dbContext.Permissions;
            var data = await _dbContext.ActionPlans
                .Where(x => x.DataID == DataID && x.CommentID == CommentID)
                .Select(x => new
                {
                    x.ID,
                    x.Title,
                    x.Description,
                    x.Tag,
                    x.ApprovedStatus,
                    x.Deadline,
                    x.UpdateSheduleDate,
                    x.ActualFinishDate,
                    x.Status,
                    x.UserID,
                    IsBoss = (int?)permission.FirstOrDefault(a => a.ID == userModel.Permission).ID < 3 ? true : false,
                    CreatedBy = x.UserID,
                    x.Auditor
                })
                .ToListAsync();
            var model = data
            .Select(x => new ActionPlanGettAllViewModel
            {
                ID = x.ID,
                Title = x.Title,
                Description = x.Description,
                Tag = x.Tag,
                ApprovedStatus = x.ApprovedStatus,
                Deadline = x.Deadline.ToString("dddd, MMMM d, yyyy"),
                UpdateSheduleDate = x.UpdateSheduleDate.HasValue ? x.UpdateSheduleDate.Value.ToString("dddd, MMMM d, yyyy") : "N/A",
                ActualFinishDate = x.ActualFinishDate.HasValue ? x.ActualFinishDate.Value.ToString("dddd, MMMM d, yyyy") : "N/A",
                Status = x.Status,
                IsBoss = x.IsBoss,
                CreatedBy = x.UserID,
                ListUserIDs = _dbContext.Tags.Where(a => a.ActionPlanID == x.ID).Select(a => a.UserID).ToList(),
                Auditor = x.Auditor
            }).ToList();
            return new
            {
                status = true,
                data = model,

            };
        }
        public ActionPlan GetbyID(int ID)
        {
            return _dbContext.ActionPlans.FirstOrDefault(x => x.ID == ID);
        }
        public object ListActionPlan()
        {
            return _dbContext.ActionPlans.ToList();
        }
        public bool CreateTagOwnerAndUpdater(List<UserViewModel> uploaders, int notifyID)
        {
            var tags = new List<Tag>();
            var notificationDetails = new List<NotificationDetail>();
            foreach (var item in uploaders)
            {
                var tag = new Tag();
                tag.UserID = item.ID;
                tag.NotificationID = notifyID;
                tags.Add(tag);
            }
            try
            {
                foreach (var value in tags)
                {
                    var itemNotifyDetail = new NotificationDetail();
                    itemNotifyDetail.UserID = value.UserID;
                    itemNotifyDetail.NotificationID = notifyID;
                    itemNotifyDetail.Seen = false;
                    notificationDetails.Add(itemNotifyDetail);
                }
                _dbContext.NotificationDetails.AddRange(notificationDetails);
                _dbContext.Tags.AddRange(tags);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool CheckExistsData(string code, string period)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();
            //Kiem tra period hien tai trong bang data
            switch (period)
            {
                case "W":
                    var W = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.Period == period && x.Week == currentWeek)?.Value;

                    if (W == null || W == "" || W == "0")
                        return true;
                    return false;
                case "M":
                    var M = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.Period == period && x.Month == currentMonth)?.Value;

                    if (M == null || M == "" || M == "0")
                        return true;
                    return false;
                case "Q":
                    var Q = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.Period == period && x.Quarter == currentQuarter)?.Value;

                    if (Q == null || Q == "" || Q == "0")
                        return true;
                    return false;
                case "Y":
                    var Y = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == code && x.Period == period && x.Year == currentYear)?.Value;

                    if (Y == null || Y == "" || Y == "0")
                        return true;
                    return false;
            }
            return false;

        }
        public Tuple<List<object[]>, List<UserViewModel>> CheckLateOnUpdateData(int userid)
        {
            #region 0) Biến toàn cục

            var listSendMail = new List<object[]>();
            var listSendMailDetail = new List<string[]>();
            var listNotify = new List<Notification>();
            var listNotifyDetail = new List<NotificationDetail>();
            var listTag = new List<Tag>();
            var dayOfWeek = DateTime.Today.DayOfWeek.ToSafetyString().ToUpper().ConvertStringDayOfWeekToNumber();
            var count = 0;
            #endregion

            #region 1) Lấy dữ liệu
            var model2 = (from cat in _dbContext.CategoryKPILevels
                          join kpilevel in _dbContext.KPILevels on cat.KPILevelID equals kpilevel.ID
                          join kpi in _dbContext.KPIs on kpilevel.KPIID equals kpi.ID
                          join level in _dbContext.Levels on kpilevel.LevelID equals level.ID
                          where kpilevel.Checked == true && cat.Status == true
                          select new ListCheckTaskVM
                          {
                              ID = kpilevel.ID,
                              Title = kpi.Name,
                              Area = level.Name,
                              KPILevelCode = kpilevel.KPILevelCode,
                              Weekly = kpilevel.Weekly ?? 1,
                              Monthly = kpilevel.Monthly ?? DateTime.MinValue,
                              Quarterly = kpilevel.Quarterly ?? DateTime.MinValue,
                              Yearly = kpilevel.Yearly ?? DateTime.MinValue,
                              WeeklyChecked = kpilevel.WeeklyChecked ?? false,
                              MonthlyChecked = kpilevel.MonthlyChecked ?? false,
                              QuarterlyChecked = kpilevel.QuarterlyChecked ?? false,
                              YearlyChecked = kpilevel.YearlyChecked ?? false,
                              UpdateDataStatusW = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode && x.Period == (kpilevel.WeeklyChecked == true ? "W" : "")) != null ? true : false,
                              UpdateDataStatusM = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode && x.Period == (kpilevel.MonthlyChecked == true ? "M" : "")) != null ? true : false,
                              UpdateDataStatusQ = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode && x.Period == (kpilevel.QuarterlyChecked == true ? "Q" : "")) != null ? true : false,
                              UpdateDataStatusY = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevel.KPILevelCode && x.Period == (kpilevel.YearlyChecked == true ? "Y" : "")) != null ? true : false
                          }).ToList();
            #endregion

            #region 2) Lấy tất cả uploader và owner theo danh sách vừa lấy ở trên
            var uploaders = (from a in model2
                             join uploader in _dbContext.Uploaders on a.ID equals uploader.KPILevelID
                             join user in _dbContext.Users on uploader.UserID equals user.ID
                             select new UserViewModel
                             {
                                 ID = user.ID,
                                 Email = user.Email
                             }).ToList();
            var owners = (from a in model2
                          join owner in _dbContext.Owners on a.ID equals owner.KPILevelID
                          join user in _dbContext.Users on owner.UserID equals user.ID
                          select new UserViewModel
                          {
                              ID = user.ID,
                              Email = user.Email
                          }).ToList();

            var listEmails = uploaders.Concat(owners).DistinctBy(x => x.ID);
            #endregion

            #region 3) Lọc Dữ liệu để gửi mail
            foreach (var item in model2)
            {
                var oc = new LevelDAO().GetNode(item.KPILevelCode);
                var time = new TimeSpan(00, 00, 00, 00);

                int month = DateTime.Compare(DateTime.Today.Add(new TimeSpan(00, 00, 00, 00)), item.Monthly);
                int quarter = DateTime.Compare(DateTime.Today.Add(new TimeSpan(00, 00, 00, 00)), item.Quarterly);
                int year = DateTime.Compare(DateTime.Today.Add(new TimeSpan(00, 00, 00, 00)), item.Yearly);
                //less than zero if ToDay is earlier than item.yearly 
                //greater than zero if ToDay is later than item.yearly

                if (month > 0 || quarter > 0 || year > 0 || item.Weekly > dayOfWeek)
                {
                    count++;
                    if (item.Weekly >= dayOfWeek && item.Weekly != 1 && item.UpdateDataStatusW == true && item.WeeklyChecked == true)
                    {
                        if (CheckExistsData(item.KPILevelCode, "W"))
                        {
                            var itemSendMail = new object[] {
                                item.Title, item.Weekly.ConvertNumberDayOfWeekToString().ToString(),item.KPILevelCode,oc,"Weekly "
                            };
                            listSendMail.Add(itemSendMail);
                        }
                    }
                    if (month >= 0 && item.Monthly != DateTime.MinValue && item.UpdateDataStatusM == true && item.MonthlyChecked == true)
                    {
                        //Kiem tra period hien tai trong bang data
                        if (CheckExistsData(item.KPILevelCode, "M"))
                        {
                            var itemSendMail = new object[] {
                                item.Title,item.Monthly.ToString(),item.KPILevelCode,oc,"Monthly"
                            };
                            listSendMail.Add(itemSendMail);
                        }

                    }
                    if (quarter >= 0 && item.Quarterly != DateTime.MinValue && item.UpdateDataStatusQ == true && item.QuarterlyChecked == true)
                    {
                        if (CheckExistsData(item.KPILevelCode, "Q"))
                        {
                            var itemSendMail = new object[] {
                                item.Title,item.Quarterly.ToString(),item.KPILevelCode,oc,"Quarterly"
                            };
                            listSendMail.Add(itemSendMail);
                        }
                    }
                    if (year >= 0 && item.Yearly != DateTime.MinValue && item.UpdateDataStatusY == true && item.YearlyChecked == true)
                    {
                        if (CheckExistsData(item.KPILevelCode, "Y"))
                        {
                            var itemSendMail = new object[] {
                                item.Title,item.Yearly.ToString(),item.KPILevelCode,oc,"Yearly"
                            };
                            listSendMail.Add(itemSendMail);
                        }
                    }

                }
            }
            #endregion

            #region 4) Nếu có dữ liệu gửi mail thì lưu vào bảng tag để thông báo đẩy 
            if (listSendMail.Count > 0)
            {
                var notify = new Notification();
                notify.Action = "LateOnUploadData";
                notify.UserID = 1;
                _dbContext.Notifications.Add(notify);
                _dbContext.SaveChanges();

                foreach (var it in listEmails)
                {
                    listNotifyDetail.Add(new NotificationDetail
                    {
                        NotificationID = notify.ID,
                        Seen = false,
                        UserID = it.ID,
                    });
                }
                _dbContext.NotificationDetails.AddRange(listNotifyDetail);
                _dbContext.SaveChanges();

                var listLateOnUploads = new List<LateOnUpLoad>();


                listSendMail.ForEach(x =>
                {
                    listLateOnUploads.Add(new LateOnUpLoad
                    {
                        KPIName = x[0].ToString(),
                        Area = x[3].ToString(),
                        Code = x[2].ToString(),
                        Year = x[4].ToString(),
                        DeadLine = x[1].ToString(),
                        UserID = userid,
                        NotificationID = notify.ID

                    });
                });
                new DataChartDAO().AddLateOnUploadAsync(listLateOnUploads).Wait();
            }
            #endregion

          

            return Tuple.Create(listSendMail, listEmails.ToList());
        }
        public Tuple<List<object[]>, List<UserViewModel>> CheckDeadline()
        {
            #region 0) Biến toàn cục
            var listSendMail = new List<object[]>();
            var currentDate = DateTime.Now;
            var timeSpan = new TimeSpan(24, 00, 00);
            var date = currentDate - timeSpan;
            var listAc = new List<ActionPlanVM>();
            var listAcID = new List<int>();
            var count = 0;

            #endregion

            //Lấy danh sách action Plan chưa hoàn thành và chưa được owner duyệt
            var model = _dbContext.ActionPlans.Where(x => x.Status == false && x.ApprovedStatus == false).ToList();
            foreach (var item in model)
                listAcID.Add(item.ID);

            //Lấy ra danh sách user được tag trong danh sách actionplan ở trên
            var listUser = (from a in _dbContext.Tags.Where(x => listAcID.Contains(x.ActionPlanID))
                            join c in _dbContext.Users on a.UserID equals c.ID
                            select new UserViewModel
                            {
                                ID = c.ID,
                                Email = c.Email
                            }).ToList();

            foreach (var item in model)
            {
                //< 0 date nho hon deadline, > 0 date lon hon deadline
                //deadline "11/06/2019" date "11/07/2019"
                if (DateTime.Compare(item.Deadline, date) < 0)
                {
                    count++;
                    var itemSendMail = new object[] {
                        item.Title,
                        String.Format("{0:dddd, MMMM d, yyyy}", item.Deadline),
                    };
                    listSendMail.Add(itemSendMail);
                }
            }
            if (count > 0)
            {
                var notificationDetails = new List<NotificationDetail>();
                foreach (var item in listSendMail)
                {
                    var itemAc = new ActionPlanVM();
                    itemAc.UserID = 1;
                    itemAc.Deadline = Convert.ToDateTime(item[1]);
                    itemAc.Email = (string)item[0];
                    listAc.Add(itemAc);
                }
               
                var notify = new Notification();
                notify.Action = "LateOnTask";
                notify.UserID = 1;
                _dbContext.Notifications.Add(notify);
                _dbContext.SaveChanges();
                foreach (var item in listUser)
                {
                    notificationDetails.Add(new NotificationDetail { NotificationID = notify.ID, UserID = item.ID, Seen = false });

                }
                _dbContext.NotificationDetails.AddRange(notificationDetails);
                _dbContext.SaveChanges();
            }
            return Tuple.Create(listSendMail, listUser);
        }
        public bool AddActionDetail(ActionPlanDetail entity)
        {
            try
            {
                _dbContext.ActionPlanDetails.Add(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<bool> UpdateActionPlan(UpdateActionPlanVM actionPlan)
        {
            try
            {
                var item = await _dbContext.ActionPlans.FindAsync(actionPlan.ID);
                if (actionPlan.Title.IsNullOrEmpty())
                {
                    item.Title = actionPlan.Title;
                }
                if (actionPlan.Description.IsNullOrEmpty())
                {
                    item.Description = actionPlan.Description;
                }
                if (actionPlan.Tag.IsNullOrEmpty())
                {
                    item.Tag = actionPlan.Tag;
                }
                if (actionPlan.DeadLine.IsNullOrEmpty())
                {
                    item.Deadline = Convert.ToDateTime(actionPlan.DeadLine);
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateSheduleDate(string name, string value, string pk, int userid)
        {
            try
            {

                var id = pk.ToSafetyString().ToInt();
                var item = await _dbContext.ActionPlans.FirstOrDefaultAsync(x => x.UserID == userid && x.ID == id);
                if (item == null)
                {
                    return false;
                }
                if (name.ToLower() == "title")
                {
                    item.Title = value;
                }
                if (name.ToLower() == "description")
                {

                    if (value.IndexOf("/n") == -1)
                    {
                        item.Description = value;
                    }
                    else
                    {
                        var des = string.Empty;
                        value.Split('\n').ToList().ForEach(line =>
                        {
                            des += line + "&#13;&#10;";
                        });
                        item.Description = des;
                    }

                }
                if (name.ToLower() == "tag")
                {
                    item.Tag = value;
                }
                if (name.ToLower() == "deadline")
                {
                    item.Deadline = Convert.ToDateTime(value);
                }
                if (name.ToLower() == "updatesheduledate")
                {
                    var dt = Convert.ToDateTime(value);

                    item.UpdateSheduleDate = dt;
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Update(string name, string value, string pk)
        {
            try
            {
                var id = pk.ToSafetyString().ToInt();
                var item = await _dbContext.ActionPlans.FindAsync(id);
                if (name.ToLower() == "title")
                {
                    item.Title = value;
                }
                if (name.ToLower() == "description")
                {

                    if (value.IndexOf("/n") == -1)
                    {
                        item.Description = value;
                    }
                    else
                    {
                        var des = string.Empty;
                        value.Split('\n').ToList().ForEach(line =>
                        {
                            des += line + "&#13;&#10;";
                        });
                        item.Description = des;
                    }

                }
                if (name.ToLower() == "tag")
                {
                    item.Tag = value;
                }
                if (name.ToLower() == "deadline")
                {
                    item.Deadline = Convert.ToDateTime(value);
                }
                if (name.ToLower() == "updatesheduledate")
                {
                    var dt = Convert.ToDateTime(value);

                    item.UpdateSheduleDate = dt;
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<object> GetActionPlanByCategory(int id)
        {
            if (id > 0)
            {
                var actionplans = (await (from a in _dbContext.ActionPlans
                                          join b in _dbContext.KPILevels on a.KPILevelCodeAndPeriod.Substring(0, a.KPILevelCodeAndPeriod.Length - 1) equals b.KPILevelCode
                                          join c in _dbContext.KPIs on b.KPIID equals c.ID
                                          join d in _dbContext.Users on a.UserID equals d.ID
                                          join e in _dbContext.Categories on c.CategoryID equals e.ID
                                          join f in _dbContext.Users on a.Auditor equals f.ID
                                          where c.CategoryID == id
                                          select new
                                          {
                                              a.ID,
                                              d.FullName,
                                              a.DataID,
                                              a.CommentID,
                                              a.Title,
                                              a.Description,
                                              a.Tag,
                                              a.ApprovedBy,
                                              a.Link,
                                              a.CreateTime,
                                              a.Deadline,
                                              a.SubmitDate,
                                              a.Status,
                                              a.ApprovedStatus,
                                              a.Auditor,
                                              c.CategoryID,
                                              Auditors = f.FullName,
                                              a.UpdateSheduleDate,
                                              a.ActualFinishDate,
                                              Category = e.Name,

                                          }).ToListAsync())
                             .Select(x => new ListActionPlanVM
                             {
                                 ID = x.ID,
                                 FullName = x.Category,
                                 DataID = x.DataID,
                                 CommentID = x.CommentID,
                                 Title = x.Title,
                                 Description = x.Description,
                                 Tag = x.Tag,
                                 ApprovedBy = x.ApprovedBy,
                                 Link = x.Link,
                                 Deadline = x.Deadline.ToString("MM/dd/yyyy"),
                                 SubmitDate = x.SubmitDate.ToString("MM/dd/yyyy"),
                                 Status = x.Status,
                                 ApprovedStatus = x.ApprovedStatus,
                                 Auditor = x.Auditors,
                                 Category = x.Category,
                                 UpdateSheduleDate = x.UpdateSheduleDate.HasValue ? x.UpdateSheduleDate.Value.ToString("MM/dd/yyyy") : "N/A",
                                 ActualFinishDate = x.ActualFinishDate.HasValue ? x.ActualFinishDate.Value.ToString("MM/dd/yyyy") : "N/A",


                             }).ToList();

                return actionplans;
            }
            else
            {
                var actionplans = (await (from a in _dbContext.ActionPlans
                                          join b in _dbContext.KPILevels on a.KPILevelCodeAndPeriod.Substring(0, a.KPILevelCodeAndPeriod.Length - 1) equals b.KPILevelCode
                                          join c in _dbContext.KPIs on b.KPIID equals c.ID
                                          join d in _dbContext.Users on a.UserID equals d.ID
                                          join e in _dbContext.Categories on c.CategoryID equals e.ID
                                          join f in _dbContext.Users on a.Auditor equals f.ID
                                          select new
                                          {
                                              a.ID,
                                              d.FullName,
                                              a.DataID,
                                              a.CommentID,
                                              a.Title,
                                              a.Description,
                                              a.Tag,
                                              a.ApprovedBy,
                                              a.Link,
                                              a.CreateTime,
                                              a.Deadline,
                                              a.SubmitDate,
                                              a.Status,
                                              a.ApprovedStatus,
                                              a.Auditor,
                                              c.CategoryID,
                                              Auditors = f.FullName,
                                              a.UpdateSheduleDate,
                                              a.ActualFinishDate,
                                              Category = e.Name
                                          }).ToListAsync())
                              .Select(x => new ListActionPlanVM
                              {
                                  ID = x.ID,
                                  FullName = x.Category,
                                  DataID = x.DataID,
                                  CommentID = x.CommentID,
                                  Title = x.Title,
                                  Description = x.Description,
                                  Tag = x.Tag,
                                  ApprovedBy = x.ApprovedBy,
                                  Link = x.Link,
                                  Deadline = x.Deadline.ToString("MM/dd/yyyy"),
                                  SubmitDate = x.SubmitDate.ToString("MM/dd/yyyy"),
                                  Status = x.Status,
                                  ApprovedStatus = x.ApprovedStatus,
                                  Auditor = x.Auditors,
                                  Category = x.Category,
                                  UpdateSheduleDate = x.UpdateSheduleDate.HasValue ? x.UpdateSheduleDate.Value.ToString("MM/dd/yyyy") : "N/A",
                                  ActualFinishDate = x.ActualFinishDate.HasValue ? x.ActualFinishDate.Value.ToString("MM/dd/yyyy") : "N/A",

                              }).ToList();

                return actionplans;
            }

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
