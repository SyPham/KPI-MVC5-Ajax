using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
    public class KPILevelDAO
    {
        KPIDbContext _dbContext = null;
        public KPILevelDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public async Task<object> GetAll(int page, int pageSize)
        {
            var model = await _dbContext.KPILevels
                .Where(x => x.UserCheck == "1")
                .Join(_dbContext.KPIs,
                kpilevel => kpilevel.KPIID,
                kpi => kpi.ID,
                (kpilevel, kpi) => new
                {
                    ID = kpilevel.ID,
                    Name = kpi.Name,
                    kpilevel.KPILevelCode,
                    CreateTime = kpilevel.CreateTime,
                })
                .ToListAsync();
            int totalRow = model.Count();
            model = model.OrderByDescending(x => x.CreateTime)

              .Skip((page - 1) * pageSize)
              .Take(pageSize).ToList();
            return new
            {
                data = model,
                total = totalRow,
                status = true,
                page,
                pageSize
            };
        }
        public KPILevel GetByID(int id)
        {
            return _dbContext.KPILevels.Where(x => x.ID == id).FirstOrDefault();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Update các cột WeeklyChecked, MonthlyChecked, QuarterlyChecked, YearlyChecked</returns>
        public async Task<bool> Update(ViewModel.UpdateKPILevelVM entity)
        {
            var comparedt = new DateTime(2001, 1, 1);
            var kpiLevel = await _dbContext.KPILevels.FirstOrDefaultAsync(x => x.ID == entity.ID);
            var kpiModel = await _dbContext.KPIs.FirstOrDefaultAsync(x => x.ID == kpiLevel.KPIID);
            var ocModel = await _dbContext.Levels.FirstOrDefaultAsync(x => x.ID == kpiLevel.LevelID);
            if (entity.Weekly != null)
            {
                kpiLevel.Weekly = entity.Weekly;
            }
            if (!entity.Monthly.IsNullOrEmpty())
            {
                kpiLevel.Monthly =Convert.ToDateTime(entity.Monthly);
            }
            if (!entity.Quarterly.IsNullOrEmpty())
            {
                kpiLevel.Quarterly = Convert.ToDateTime(entity.Quarterly);
            }
            if (!entity.Yearly.IsNullOrEmpty())
            {
                kpiLevel.Yearly = Convert.ToDateTime(entity.Yearly);
            }
            if (entity.WeeklyChecked != null)
            {
                kpiLevel.WeeklyChecked = entity.WeeklyChecked;
            }
            if (entity.MonthlyChecked != null)
            {
                kpiLevel.MonthlyChecked = entity.MonthlyChecked;
            }
            if (entity.QuarterlyChecked != null)
            {
                kpiLevel.QuarterlyChecked = entity.QuarterlyChecked;
            }
            if (entity.MonthlyChecked != null)
            {
                kpiLevel.MonthlyChecked = entity.MonthlyChecked;
            }
            if (entity.YearlyChecked != null)
            {
                kpiLevel.YearlyChecked = entity.YearlyChecked;
            }
            if (entity.WeeklyPublic != null)
            {
                kpiLevel.WeeklyPublic = entity.WeeklyPublic;
            }
            if (entity.MonthlyPublic != null)
            {
                kpiLevel.MonthlyPublic = entity.MonthlyPublic;
            }
            if (entity.QuarterlyPublic != null)
            {
                kpiLevel.QuarterlyPublic = entity.QuarterlyPublic;
            }
            if (entity.YearlyPublic != null)
            {
                kpiLevel.YearlyPublic = entity.YearlyPublic;
            }
            if (entity.Checked != null)
            {
                kpiLevel.Checked = entity.Checked;
                kpiLevel.KPILevelCode = ocModel.LevelNumber + ocModel.Code + kpiModel.Code;
            }
           
            kpiLevel.UserCheck = entity.UserCheck;
            kpiLevel.TimeCheck =DateTime.Now;
            kpiLevel.CreateTime = DateTime.Now;
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //logging
                return false;
            }

        }
        public int Total()
        {
            return _dbContext.KPILevels.Where(x => x.Checked == true).ToList().Count();
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns>Lấy danh sách các KPILevel</returns>
        public async Task<IEnumerable<EF.KPILevel>> GetAll()
        {
            return await _dbContext.KPILevels.ToListAsync();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="levelID"></param>
        /// <returns>Dnah sách các level theo điều kiện</returns>
        public IEnumerable<EF.KPILevel> GetAllByID(int levelID)
        {
            return _dbContext.KPILevels.Where(x => x.LevelID == levelID).ToList();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Tìm kiếm KPILevel theo ID</returns>
        public EF.KPILevel GetbyID(int ID)
        {
            return _dbContext.KPILevels.FirstOrDefault(x => x.ID == ID);
        }

        public async Task<object> GetDetail(int ID)
        {
            var item = await _dbContext.KPILevels.FirstOrDefaultAsync(x => x.ID == ID);

            return new
            {
                status = true,
                data = item

            };
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns>Danh sách tất cả các record trong bảng Category</returns>
        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            return await _dbContext.Categories.ToListAsync();
        }
        /// <summary>
        /// Lấy ra danh sách tất cả các KPILevel
        /// </summary>
        /// <param name="levelID"></param>
        /// <param name="categoryID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>Danh sách KPI theo điều kiện</returns>
        public async Task<object> LoadData(int levelID, int categoryID, int page, int pageSize = 3)
        {
            var model = await (from kpiLevel in _dbContext.KPILevels
                               where kpiLevel.LevelID == levelID
                               join kpi in _dbContext.KPIs on kpiLevel.KPIID equals kpi.ID
                               join unit in _dbContext.Units on kpi.Unit equals unit.ID
                               join level in _dbContext.Levels on kpiLevel.LevelID equals level.ID
                               select new KPILevelVM
                               {
                                   ID = kpiLevel.ID,
                                   KPILevelCode = kpiLevel.KPILevelCode,
                                   UserCheck = kpiLevel.KPILevelCode,
                                   KPIID = kpiLevel.KPIID,
                                   KPICode = kpi.Code,
                                   LevelID = kpiLevel.LevelID,
                                   LevelNumber = kpi.LevelID,
                                   Period = kpiLevel.Period,

                                   Weekly = kpiLevel.Weekly,
                                   Monthly = kpiLevel.Monthly,
                                   Quarterly = kpiLevel.Quarterly,
                                   Yearly = kpiLevel.Yearly,

                                   Checked = kpiLevel.Checked,
                                   WeeklyChecked = kpiLevel.WeeklyChecked,
                                   MonthlyChecked = kpiLevel.MonthlyChecked,
                                   QuarterlyChecked = kpiLevel.QuarterlyChecked,
                                   YearlyChecked = kpiLevel.YearlyChecked,
                                   CheckedPeriod = kpiLevel.CheckedPeriod,

                                   TimeCheck = kpiLevel.TimeCheck,

                                   CreateTime = kpiLevel.CreateTime,
                                   Unit = unit.Name,
                                   CategoryID = kpi.CategoryID,
                                   KPIName = kpi.Name,
                                   LevelCode = level.Code,
                               }).ToListAsync();
            if (categoryID != 0)
            {
                model = model.Where(x => x.CategoryID == categoryID).ToList();
            }

            int totalRow = model.Count();

            model = model.OrderByDescending(x => x.CreateTime)
              .Skip((page - 1) * pageSize)
              .Take(pageSize).ToList();


            return new
            {
                data = model,
                total = totalRow,
                status = true,
                page,
                pageSize
            };
        }

        public int GetByUsername(string username)
        {
            try
            {

                return _dbContext.Users.FirstOrDefault(x => x.Username == username).ID;

            }
            catch (Exception)
            {

                return 0;

            }
        }
        /// <summary>
        /// Lấy ra danh sách những KPI có checked bằng true.
        /// </summary>
        /// <param name="levelID"></param>
        /// <param name="categoryID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>Danh sách KPILevel có checked bằng true</returns>
        public async Task<object> LoadDataForUser(int levelID, int categoryID, int page, int pageSize = 3)
        {
            //Lấy các tuần tháng quý năm hiện tại
            var weekofyear = DateTime.Now.GetIso8601WeekOfYear();
            var monthofyear = DateTime.Now.Month;
            var quarterofyear = DateTime.Now.GetQuarterOfYear();
            var year = DateTime.Now.Year;
            var currentweekday = DateTime.Now.DayOfWeek.ToSafetyString().ToUpper().ConvertStringDayOfWeekToNumber();
            var currentdate = DateTime.Now.Date;
            var dt = new DateTime(2019, 8, 1);
            var value = DateTime.Compare(currentdate, dt);
            try
            {
                //Lấy ra danh sách data từ trong db
                var datas = _dbContext.Datas;
                var catKPILevel = _dbContext.CategoryKPILevels;
                var model = await (from kpiLevel in _dbContext.KPILevels
                                   where kpiLevel.LevelID == levelID && kpiLevel.Checked == true
                                   join kpi in _dbContext.KPIs on kpiLevel.KPIID equals kpi.ID
                                   join level in _dbContext.Levels on kpiLevel.LevelID equals level.ID
                                   select new KPILevelVM
                                   {
                                       ID = kpiLevel.ID,
                                       KPILevelCode = kpiLevel.KPILevelCode,
                                       UserCheck = kpiLevel.KPILevelCode,
                                       KPIID = kpiLevel.KPIID,
                                       KPICode = kpi.Code,
                                       LevelID = kpiLevel.LevelID,
                                       LevelNumber = kpi.LevelID,
                                       Period = kpiLevel.Period,

                                       Weekly = kpiLevel.Weekly,
                                       Monthly = kpiLevel.Monthly,
                                       Quarterly = kpiLevel.Quarterly,
                                       Yearly = kpiLevel.Yearly,

                                       Checked = kpiLevel.Checked,

                                       WeeklyChecked = kpiLevel.WeeklyChecked,
                                       MonthlyChecked = kpiLevel.MonthlyChecked,
                                       QuarterlyChecked = kpiLevel.QuarterlyChecked,
                                       YearlyChecked = kpiLevel.YearlyChecked,
                                       CheckedPeriod = kpiLevel.CheckedPeriod,

                                       //true co du lieu false khong co du lieu
                                       StatusEmptyDataW = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.WeeklyChecked == true ? "W" : "")) != null ? true : false,
                                       StatusEmptyDataM = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.MonthlyChecked == true ? "M" : "")) != null ? true : false,
                                       StatusEmptyDataQ = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.QuarterlyChecked == true ? "Q" : "")) != null ? true : false,
                                       StatusEmptyDataY = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.YearlyChecked == true ? "Y" : "")) != null ? true : false,

                                       TimeCheck = kpiLevel.TimeCheck,
                                       CreateTime = kpiLevel.CreateTime,

                                       //CategoryID = kpi.CategoryID,
                                       KPIName = kpi.Name,
                                       LevelCode = level.Code,
                                       //Nếu tuần hiện tại - tuần MAX trong bảng DATA > 1 return false,
                                       //ngược lại nếu == 1 thi kiểm tra thứ trong bảng KPILevel,
                                       //Nếu thứ nhỏ hơn thứ hiện tại thì return true,
                                       //ngược lại reutrn false
                                       StatusUploadDataW = weekofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W").Max(x => x.Week) > 1 ? false : ((weekofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W").Max(x => x.Week)) == 1 ? (kpiLevel.Weekly < currentweekday ? true : false) : false),

                                       StatusUploadDataM = monthofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "M").Max(x => x.Month) > 1 ? false : monthofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "M").Max(x => x.Month) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Monthly.Value) < 0 ? true : false) : false,

                                       StatusUploadDataQ = quarterofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Q").Max(x => x.Quarter) > 1 ? false : quarterofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Q").Max(x => x.Quarter) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Quarterly.Value) < 0 ? true : false) : false, //true dung han flase tre han

                                       StatusUploadDataY = year - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Y").Max(x => x.Year) > 1 ? false : year - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Y").Max(x => x.Year) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Yearly.Value) < 0 ? true : false) : false,

                                       CheckCategory = catKPILevel.FirstOrDefault(x => x.KPILevelID == kpiLevel.ID && x.CategoryID == categoryID) == null ? false : catKPILevel.FirstOrDefault(x => x.Status == true && x.KPILevelID == kpiLevel.ID && x.CategoryID == categoryID).Status == true ? true : false

                                   }).ToListAsync();



                //if (categoryID != 0)
                //{
                //    model = model.Where(x => x.CategoryID == categoryID);
                //}

                int totalRow = model.Count();

                model = model.OrderByDescending(x => x.CreateTime)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize).ToList();


                return new
                {
                    data = model,
                    total = totalRow,
                    status = true,
                    page,
                    pageSize
                };
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return "";
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kpilevelcode"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public async Task<object> ListDatas(string kpilevelcode, string period)
        {
            if (!string.IsNullOrEmpty(kpilevelcode) && !string.IsNullOrEmpty(period))
            {
                //label chartjs
                var item = await _dbContext.KPILevels.FirstOrDefaultAsync(x => x.KPILevelCode == kpilevelcode);
                var modelLevel = await _dbContext.Levels.FirstOrDefaultAsync(x => x.ID == item.LevelID);
                var label = modelLevel.Name;
                //datasets chartjs
                var model = await _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode).ToListAsync();

                if (period == "W".ToUpper())
                {

                    var datasets = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Week).ToArray();


                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else if (period == "M".ToUpper())
                {

                    var datasets = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Month).ToArray();
                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else if (period == "Q".ToUpper())
                {
                    var datasets = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Quarter).ToArray();
                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else if (period == "Y".ToUpper())
                {

                    var datasets = model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Value).ToArray();

                    //data: labels chartjs
                    var labels = model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Year).ToArray();
                    return new
                    {
                        datasets,
                        labels,
                        label
                    };
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        #region Add Comment Helper
        /// <summary>
        /// Kiểm tra levelNumber của userComment và owner
        /// </summary>
        /// <param name="OCIDOfUserComment">OCID của người comment</param>
        /// <param name="OCIDOfOwner">OCID của owner</param>
        /// <returns>Trả về true hoặc false</returns>
        public async Task<bool> CheckLevelNumberOfUser(int OCIDOfUserComment, int OCIDOfOwner)
        {
            var leveNumberOfUserComment = await _dbContext.Levels.FindAsync(OCIDOfUserComment);
            var leveNumberOfOwner = await _dbContext.Levels.FindAsync(OCIDOfOwner);
            if (leveNumberOfUserComment.LevelNumber > leveNumberOfOwner.LevelNumber)
                return true;
            return false;
        }

        public async Task<Comment> CreateComment(Comment comment)
        {
            try
            {
                _dbContext.Comments.Add(comment);
                await _dbContext.SaveChangesAsync();
                return comment;
            }
            catch (Exception)
            {

                return comment;
            }
        }
        /// <summary>
        /// Tạo mới SeenComment
        /// </summary>
        /// <param name="seenComment"></param>
        /// <returns>Trả về item SeenComment</returns>
        public async Task<SeenComment> CreateSeenComment(SeenComment seenComment)
        {
            try
            {
                _dbContext.SeenComments.Add(seenComment);
                await _dbContext.SaveChangesAsync();
                return seenComment;
            }
            catch (Exception)
            {

                return seenComment;
            }
        }

        /// <summary>
        /// Trả về danh sách gửi mail, tag, họ tên tag
        /// </summary>
        /// <param name="users">Danh sách các user tag</param>
        /// <param name="userID">ID của user comment</param>
        /// <param name="comment">Đối tượng comment vừa add</param>
        /// <returns>Trả về danh sách gửi mail, tag, họ tên user tag</returns>
        public async Task<Tuple<List<string[]>, List<string>, List<Tag>>> CreateTag(string users, int userID, Comment comment)
        {
            List<string[]> listEmail = new List<string[]>();
            List<Tag> listTags = new List<Tag>();
            List<string> listFullNameTag = new List<string>();
            var user = _dbContext.Users;
            if (users.IndexOf(',') == -1) //Neu tag 1 nguoi
            {
                var username = users.Trim();
                var recipient = await user.FirstOrDefaultAsync(x => x.Username == username);// nguoi nhan
                if (recipient != null)
                {
                    var itemtag = new Tag();
                    itemtag = new Tag();
                    itemtag.UserID = recipient.ID;
                    itemtag.CommentID = comment.ID;

                    string[] arrayString = new string[5];
                    arrayString[0] = user.Find(userID).Alias;
                    arrayString[1] = recipient.Email;
                    arrayString[2] = comment.Link;
                    arrayString[3] = comment.Title;
                    arrayString[4] = comment.CommentMsg;

                    listFullNameTag.Add(recipient.Alias);

                    listEmail.Add(arrayString);
                    listTags.Add(itemtag);
                }

            }
            else//Tag nhieu nguoi
            {
                var list = users.Split(',');
                var commentID = comment.ID;
                var listUserID = await _dbContext.Tags.Where(x => x.ActionPlanID == comment.ID).Select(x => x.UserID).ToListAsync();
                var listUsers = await _dbContext.Users.Where(x => list.Contains(x.Username)).ToListAsync();
                foreach (var item in listUsers)
                {
                    string[] arrayString = new string[5];
                    var itemtag = new Tag();
                    itemtag.CommentID = comment.ID;
                    itemtag.UserID = item.ID;

                    arrayString[0] = user.Find(userID).Alias;
                    arrayString[1] = item.Email;
                    arrayString[2] = comment.Link;
                    arrayString[3] = comment.Title;
                    arrayString[4] = comment.CommentMsg;

                    listTags.Add(itemtag);
                    listEmail.Add(arrayString);
                    listFullNameTag.Add(item.Alias);
                }
            }
            return Tuple.Create(listEmail, listFullNameTag, listTags);

        } 
        #endregion
        public async Task<AddCommentVM> AddComment(AddCommentViewModel entity, int levelIDOfUserComment)
        {
            var listEmail = new List<string[]>();
            var listTags = new List<Tag>();
            var listFullNameTag = new List<string>();
            var user = _dbContext.Users;
            var dataModel = _dbContext.Datas;
            try
            {
                //add vao comment
                var comment = new Comment();
                comment.CommentMsg = entity.CommentMsg;
                comment.DataID = entity.DataID;
                comment.UserID = entity.UserID;//sender
                comment.Link = entity.Link;
                comment.Title = entity.Title;
                await CreateComment(comment);

                //B1: Xu ly viec gui thong bao den Owner khi nguoi gui cap cao hon comment
                //Tim levelNumber cua user comment
                var kpilevelIDResult = await _dbContext.KPILevels.FirstOrDefaultAsync(x => x.KPILevelCode == entity.KPILevelCode);
                var userIDResult = await _dbContext.Owners.FirstOrDefaultAsync(x => x.KPILevelID == kpilevelIDResult.ID && x.CategoryID == entity.CategoryID);
                var userModel = await _dbContext.Users.FindAsync(userIDResult.UserID);

                //Lay ra danh sach owner thuoc categoryID va KPILevelCode
                var owners = await _dbContext.Owners.Where(x => x.KPILevelID == kpilevelIDResult.ID && x.CategoryID == entity.CategoryID).ToListAsync();

                //Neu nguoi comment ma la cap cao hon owner thi moi gui thong bao va gui mail cho owner
                if (await CheckLevelNumberOfUser(levelIDOfUserComment, userModel.LevelID))
                {
                    owners.ForEach(userItem =>
                    {
                        //Add Tag gui thong bao den cac owner
                        if (entity.UserID != userItem.ID) //Neu chinh owner do binh luan thi khong gui thong bao
                        {
                            var itemtag = new Tag();
                            itemtag = new Tag();
                            itemtag.UserID = userItem.ID;
                            itemtag.CommentID = comment.ID;

                            listTags.Add(itemtag); //Day la danh sach tag
                            //Add vao list gui mail
                            string[] arrayString = new string[5];
                            arrayString[0] = user.Find(entity.UserID).Alias; //Bi danh
                            arrayString[1] = user.Find(entity.UserID).Email;
                            arrayString[2] = comment.Link;
                            arrayString[3] = comment.Title;
                            arrayString[4] = comment.CommentMsg;

                            listEmail.Add(arrayString);
                        }

                    });
                    //B2: Neu ma nguoi cap cao hon owner tag ai do vao comment cua ho thi se gui mail va thong bao den nguoi do
                    if (!entity.Tag.IsNullOrEmpty())
                    {
                        var result = await CreateTag(entity.Tag, entity.UserID, comment);
                        listEmail = result.Item1;
                        listFullNameTag = result.Item2;
                        listTags = result.Item3;
                    }
                }
                else //Neu user co level nho hon owner commnent thi gui den owner 
                {
                    //B1: Gui thong bao den cac owner
                    owners.ForEach(x =>
                    {
                        //Add vao Tag de gui thong 
                        if (entity.UserID != x.UserID)
                        {
                            var itemtag = new Tag();
                            itemtag = new Tag();
                            itemtag.UserID = x.UserID;
                            itemtag.CommentID = comment.ID;

                            listTags.Add(itemtag); //Day la danh sach tag
                        }

                    });

                    //B2: Neu tag ai thi gui thong bao den nguoi do
                    if (!entity.Tag.IsNullOrEmpty())
                    {
                        var result = await CreateTag(entity.Tag, entity.UserID, comment);
                        listEmail = result.Item1;
                        listFullNameTag = result.Item2;
                        listTags = result.Item3;
                    }
                }
                //Add vao seencomment
                var seenComment = new SeenComment();
                seenComment.CommentID = comment.ID;
                seenComment.UserID = entity.UserID;
                seenComment.Status = true;

                _dbContext.Tags.AddRange(listTags);
                await _dbContext.SaveChangesAsync();
                await CreateSeenComment(seenComment);

                if (listTags.Count > 0)
                {
                    //Add vao Notification
                    var notify = new Notification();
                    notify.CommentID = comment.ID;
                    notify.Content = comment.CommentMsg;
                    notify.UserID = entity.UserID; //sender
                    notify.Title = comment.Title;
                    notify.Link = comment.Link;
                    notify.Action = "Comment";
                    notify.Tag = string.Join(",", listFullNameTag);
                    await new NotificationDAO().Add(notify);
                }


                return new AddCommentVM
                {
                    Status = true,
                    ListEmails = listEmail
                };
            }
            catch (Exception)
            {
                return new AddCommentVM { Status = false };
            }
        }

        public async Task<bool> AddCommentHistory(int userid, int dataid)
        {
            try
            {
                var comments = await _dbContext.Comments.Where(x => x.DataID == dataid).ToListAsync();
                foreach (var comment in comments)
                {
                    var item = await _dbContext.SeenComments.FirstOrDefaultAsync(x => x.UserID == userid && x.CommentID == comment.ID);
                    if (item == null)
                    {
                        var seencmt = new SeenComment();
                        seencmt.CommentID = comment.ID;
                        seencmt.UserID = userid;
                        seencmt.Status = true;
                        _dbContext.SeenComments.Add(seencmt);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Lấy ra các danh sách comment theo từng Value của KPILevel
        /// </summary>
        /// <param name="dataid">Là giá trị của KPILevel upload</param>
        /// <returns>Trả về các comment theo dataid</returns>
        public async Task<object> ListComments(int dataid, int userid)
        {

            var actionPlan = _dbContext.ActionPlans;
            //Cat chuoi
            //lay tat ca comment cua kpi
            var listcmts = await _dbContext.Comments.Where(x => x.DataID == dataid).ToListAsync();

            //Tong tat ca cac comment cua kpi
            var totalcomment = listcmts.Count();

            //Lay ra tat ca lich su cmt
            var seenCMT = _dbContext.SeenComments;

            //Lay ra tat ca lich su cmt
            var user = _dbContext.Users;

            //Lay ra tat ca cac comment cua kpi(userid nao post comment len thi mac dinh userid do da xem comment cua chinh minh roi)
            var data = await _dbContext.Comments.Where(x => x.DataID == dataid)
               .Select(x => new CommentVM
               {
                   CommentID = x.ID,
                   UserID = x.UserID,
                   CommentMsg = x.CommentMsg,
                   //KPILevelCode = x.KPILevelCode,
                   CommentedDate = x.CommentedDate,
                   FullName = user.FirstOrDefault(a => a.ID == x.UserID).FullName,
                   //Period = x.Period,
                   Read = seenCMT.FirstOrDefault(a => a.CommentID == x.ID && a.UserID == userid) == null ? true : false,
                   IsHasTask = actionPlan.FirstOrDefault(a => a.DataID == dataid && a.CommentID == x.ID) == null ? false : true,
                   Task = actionPlan.FirstOrDefault(a => a.DataID == dataid && a.CommentID == x.ID) == null ? false : true
               })
               .OrderByDescending(x => x.CommentedDate)
               .ToListAsync();

            return new
            {
                data,
                total = _dbContext.Comments.Where(x => x.DataID == dataid).Count()
            };

        }

        //public object LoadData(string obj)
        //{
        //    var value = obj.ToSafetyString().Split(',');
        //    var code = value[0].Substring(0, value[0].Length - 1).ToSafetyString();
        //    var period = value[0].Substring(value[0].Length - 1, 1).ToUpper().ToSafetyString();
        //    var userid = value[1].ToInt();
        //    var data = _dbContext.Comments
        //       .Where(x => x.KPILevelCode == code)
        //       .Select(x => new CommentVM
        //       {
        //           CommentMsg = x.CommentMsg,
        //           KPILevelCode = x.KPILevelCode,
        //           CommentedDate = x.CommentedDate,
        //           FullName = _dbContext.Users.FirstOrDefault(a => a.ID == x.UserID).FullName,
        //           Period = x.Period,
        //           Read = _dbContext.SeenComments.FirstOrDefault(b => b.CommentID == x.ID && b.UserID == userid).Status
        //       })
        //       .OrderByDescending(x => x.CommentedDate)
        //       .Take(4).ToList();

        //    return new
        //    {
        //        data,
        //        total = _dbContext.Comments.Where(x => x.KPILevelCode == code).Count()
        //    };

        //}
        /// <summary>
        /// Lấy ra danh sách để so sánh chart với nhau.
        /// </summary>
        /// <param name="obj">Chuỗi dữ liệu gồm KPIlevelcode, Period của các KPILevel</param>
        /// <returns>Trả về danh sách so sánh dữ liệu cùng cấp. So sánh tối đa 4 KPILevel</returns>
        public async Task<object> LoadDataProvide(string obj, int page, int pageSize)
        {
            var listCompare = new List<CompareVM>();
            var value = obj.ToSafetyString().Split(',');
            var kpilevelcode = value[0].ToSafetyString();
            var period = value[1].ToSafetyString();

            var itemkpilevel = await _dbContext.KPILevels.FirstOrDefaultAsync(x => x.KPILevelCode == kpilevelcode);
            var itemlevel = await _dbContext.Levels.FirstOrDefaultAsync(x => x.ID == itemkpilevel.LevelID);
            var levelNumber = itemlevel.LevelNumber;
            var kpiid = itemkpilevel.KPIID;
            //Lay ra tat ca kpiLevel cung levelNumber

            if (period == "W")
            {

                listCompare = await _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.WeeklyChecked == true && !x.KPILevelCode.Contains(kpilevelcode))
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true,
                        StatusPublic = (bool?)x.WeeklyPublic ?? false
                    }).
                    Where(c => c.LevelNumber == levelNumber).ToListAsync();

                int totalRow = listCompare.Count();
                listCompare = listCompare.OrderByDescending(x => x.LevelNumber)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return new
                {
                    total = totalRow,
                    listCompare
                };
            }

            if (period == "M")
            {
                listCompare = await _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.MonthlyChecked == true && !x.KPILevelCode.Contains(kpilevelcode))
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true,
                        StatusPublic = (bool?)x.MonthlyPublic ?? false
                    }).
                    Where(c => c.LevelNumber == levelNumber)
                    .ToListAsync();

                int totalRow = listCompare.Count();
                listCompare = listCompare.OrderByDescending(x => x.LevelNumber)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return new
                {
                    total = totalRow,
                    listCompare
                };
            }

            if (period == "Q")
            {
                listCompare = await _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.QuarterlyChecked == true && !x.KPILevelCode.Contains(kpilevelcode))
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true,
                        StatusPublic = (bool?)x.QuarterlyPublic ?? false
                    }).
                    Where(c => c.LevelNumber == levelNumber)
                    .ToListAsync();

                int totalRow = listCompare.Count();
                listCompare = listCompare.OrderByDescending(x => x.LevelNumber)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return new
                {
                    total = totalRow,
                    listCompare
                };
            }

            if (period == "Y")
            {
                listCompare = await _dbContext.KPILevels.Where(x => x.KPIID == kpiid && x.YearlyChecked == true && !x.KPILevelCode.Contains(kpilevelcode))
                    .Join(_dbContext.Levels,
                    x => x.LevelID,
                    a => a.ID,
                    (x, a) => new CompareVM
                    {
                        KPILevelCode = x.KPILevelCode + ",W",
                        LevelNumber = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).LevelNumber,
                        Area = _dbContext.Levels.FirstOrDefault(l => l.ID == x.LevelID).Name,
                        Status = _dbContext.Datas.FirstOrDefault(henry => henry.KPILevelCode == x.KPILevelCode) == null ? false : true,
                        StatusPublic = (bool?)x.YearlyPublic ?? false
                    }).
                    Where(c => c.LevelNumber == levelNumber)
                    .ToListAsync();

                int totalRow = listCompare.Count();
                listCompare = listCompare.OrderByDescending(x => x.LevelNumber)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return new
                {
                    total = totalRow,
                    listCompare
                };
            }
            //Lay tat ca kpilevel cung period

            return new
            {
                listCompare
            };
        }
        /// <summary>
        /// Lấy ra danh sách những KPI có checked bằng true.
        /// </summary>
        /// <param name="levelID"></param>
        /// <param name="categoryID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>Danh sách KPILevel có checked bằng true</returns>
        public object LoadKPILevel(int categoryID, int page, int pageSize = 3)
        {
            //Lấy các tuần tháng quý năm hiện tại
            var weekofyear = DateTime.Now.GetIso8601WeekOfYear();
            var monthofyear = DateTime.Now.Month;
            var quarterofyear = DateTime.Now.GetQuarterOfYear();
            var year = DateTime.Now.Year;
            var currentweekday = DateTime.Now.DayOfWeek.ToSafetyString().ToUpper().ConvertStringDayOfWeekToNumber();
            var currentdate = DateTime.Now.Date;
            var dt = new DateTime(2019, 8, 1);
            var value = DateTime.Compare(currentdate, dt);
            try
            {
                //Lấy ra danh sách data từ trong db
                var datas = _dbContext.Datas;
                var model = from cat in _dbContext.CategoryKPILevels
                            join kpiLevel in _dbContext.KPILevels on cat.KPILevelID equals kpiLevel.ID
                            join kpi in _dbContext.KPIs on kpiLevel.KPIID equals kpi.ID
                            where cat.CategoryID == categoryID && kpiLevel.Checked == true
                            select new KPILevelVM
                            {
                                ID = cat.ID,
                                KPILevelCode = kpiLevel.KPILevelCode,
                                UserCheck = kpiLevel.KPILevelCode,
                                KPIID = kpiLevel.KPIID,
                                KPICode = kpi.Code,
                                LevelID = kpiLevel.LevelID,
                                LevelNumber = kpi.LevelID,
                                Period = kpiLevel.Period,

                                Weekly = kpiLevel.Weekly,
                                Monthly = kpiLevel.Monthly,
                                Quarterly = kpiLevel.Quarterly,
                                Yearly = kpiLevel.Yearly,

                                Checked = kpiLevel.Checked,

                                WeeklyChecked = kpiLevel.WeeklyChecked,
                                MonthlyChecked = kpiLevel.MonthlyChecked,
                                QuarterlyChecked = kpiLevel.QuarterlyChecked,
                                YearlyChecked = kpiLevel.YearlyChecked,
                                CheckedPeriod = kpiLevel.CheckedPeriod,

                                //true co du lieu false khong co du lieu
                                StatusEmptyDataW = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.WeeklyChecked == true ? "W" : "")) != null ? true : false,
                                StatusEmptyDataM = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.MonthlyChecked == true ? "M" : "")) != null ? true : false,
                                StatusEmptyDataQ = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.QuarterlyChecked == true ? "Q" : "")) != null ? true : false,
                                StatusEmptyDataY = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.YearlyChecked == true ? "Y" : "")) != null ? true : false,

                                TimeCheck = kpiLevel.TimeCheck,
                                CreateTime = kpiLevel.CreateTime,

                                CategoryID = kpi.CategoryID,
                                KPIName = kpi.Name,
                                //Nếu tuần hiện tại - tuần MAX trong bảng DATA > 1 return false,
                                //ngược lại nếu == 1 thi kiểm tra thứ trong bảng KPILevel,
                                //Nếu thứ nhỏ hơn thứ hiện tại thì return true,
                                //ngược lại reutrn false
                                StatusUploadDataW = weekofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W").Max(x => x.Week) > 1 ? false : ((weekofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W").Max(x => x.Week)) == 1 ? (kpiLevel.Weekly < currentweekday ? true : false) : false),

                                StatusUploadDataM = monthofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "M").Max(x => x.Month) > 1 ? false : monthofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "M").Max(x => x.Month) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Monthly.Value) < 0 ? true : false) : false,

                                StatusUploadDataQ = quarterofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Q").Max(x => x.Quarter) > 1 ? false : quarterofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Q").Max(x => x.Quarter) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Quarterly.Value) < 0 ? true : false) : false, //true dung han flase tre han

                                StatusUploadDataY = year - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Y").Max(x => x.Year) > 1 ? false : year - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Y").Max(x => x.Year) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Yearly.Value) < 0 ? true : false) : false,

                            };



                int totalRow = model.Count();

                model = model.OrderByDescending(x => x.CreateTime)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize);


                return new
                {
                    data = model.ToList(),
                    total = totalRow,
                    status = true
                };
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return new
                {
                    status = false,
                };
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

