using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Configuration;
using PagedList;
using System.Data.Entity;
using System.Data.SqlClient;

namespace KPI.Model.DAO
{
    public class UploadDAO : IDisposable
    {
        public KPIDbContext _dbContext = null;
        public UploadDAO() => _dbContext = new KPIDbContext();
        public bool AddRange1(List<EF.Data> entity)
        {
            foreach (var item in entity)
            {
                var itemcode = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode);
                if (itemcode == null)
                {
                    try
                    {
                        _dbContext.Datas.Add(item);
                        _dbContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }
                else if (item.Week > 0 && itemcode.Week == item.Week)
                {
                    itemcode.Week = item.Week;
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }
                else if (item.Month > 0 && itemcode.Month == item.Month)
                {
                    itemcode.Month = item.Month;
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }
                else if (item.Quarter > 0 && itemcode.Quarter == item.Quarter)
                {
                    itemcode.Quarter = item.Quarter;
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }
                else if (item.Year > 0 && itemcode.Year == item.Year)
                {
                    itemcode.Year = item.Year;
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        return false;
                    }
                }

            }
            return true;
        }

        #region *) Helper của hàm ImportData

        /// <summary>
        /// Kiểm tra tồn tại Data
        /// </summary>
        /// <param name="kpilevelcode"></param>
        /// <param name="period"></param>
        /// <param name="periodValue"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public Data IsExistKPILevelData(string kpilevelcode, string period, int periodValue, int year)
        {
            switch (period.ToSafetyString().ToUpper())
            {
                case "W":
                    return _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period && x.Week == periodValue && x.Yearly == year);
                case "M":
                    return _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period && x.Month == periodValue && x.Yearly == year);
                case "Q":
                    return _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period && x.Quarter == periodValue && x.Yearly == year);
                case "Y":
                    return _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period && x.Year == periodValue && x.Yearly == year);

                default:
                    return null;
            }
        }
        /// <summary>
        /// Hàm này dùng để lọc dữ liệu "Tạo mới" hay là "Cập nhật" đọc từ file Excel.
        /// </summary>
        /// <param name="entity">Danh sách đọc từ excel</param>
        /// <returns>Trả về 2 danh sách "Tạo mới" và "Cập nhật" đọc từ file Excel</returns>
        public Tuple<List<Data>, List<Data>> CreateOrUpdateData(List<UploadDataVM> entity)
        {
            List<Data> listCreateData = new List<Data>();
            List<Data> listUpdateData = new List<Data>();
            List<UploadDataVM> list = new List<UploadDataVM>();
            foreach (var item in entity)
            {
                var value = item.KPILevelCode;
                var kpilevelcode = value.Substring(0, value.Length - 1);
                var period = value.Substring(value.Length - 1, 1);
                var year = item.Year; //dữ liệu trong năm vd: năm 2019
                var valuePeriod = item.Value;
                var target = item.TargetValue;
                //query trong bảng data nếu updated thì update lại db
                var isExistData = IsExistKPILevelData(kpilevelcode, period, item.PeriodValue, year);
                switch (period)
                {
                    case "W":
                        var dataW = new Data();
                        dataW.KPILevelCode = kpilevelcode;
                        dataW.Value = item.Value;
                        dataW.Week = item.PeriodValue;
                        dataW.Yearly = year;
                        dataW.CreateTime = item.CreateTime;
                        dataW.Period = period;
                        if (item.TargetValue.ToDouble() > 0)
                            dataW.Target = item.TargetValue.ToString();
                        else dataW.Target = "0";
                        if (isExistData == null)
                            listCreateData.Add(dataW);
                        else if (isExistData != null)
                        {
                            if (dataW.Value != valuePeriod || dataW.Target != target)
                            {
                                dataW.ID = isExistData.ID;
                                listUpdateData.Add(dataW);
                            }
                        }
                        else
                            list.Add(item);
                        break;
                    case "M":
                        var dataM = new Data();
                        dataM.KPILevelCode = kpilevelcode;
                        dataM.Value = item.Value;
                        dataM.Month = item.PeriodValue;
                        dataM.Yearly = year;
                        dataM.CreateTime = item.CreateTime;
                        dataM.Period = period;

                        if (item.TargetValue.ToDouble() > 0)
                            dataM.Target = item.TargetValue.ToString();
                        else dataM.Target = "0";
                        if (isExistData == null)
                            listCreateData.Add(dataM);
                        else if (isExistData != null)
                        {
                            if (isExistData.Value != valuePeriod || isExistData.Target != target)
                            {
                                dataM.ID = isExistData.ID;
                                listUpdateData.Add(dataM);
                            }
                        }
                        else
                            list.Add(item);
                        break;
                    case "Q":
                        var dataQ = new Data();
                        dataQ.KPILevelCode = kpilevelcode;
                        dataQ.Value = item.Value;
                        dataQ.Quarter = item.PeriodValue;
                        dataQ.Yearly = year;
                        dataQ.CreateTime = item.CreateTime;
                        dataQ.Period = period;

                        if (item.TargetValue.ToDouble() > 0)
                            dataQ.Target = item.TargetValue.ToString();
                        else dataQ.Target = "0";
                        if (isExistData == null)
                            listCreateData.Add(dataQ);
                        else if (isExistData != null)
                        {
                            if (isExistData.Value != valuePeriod || isExistData.Target != target)
                            {
                                dataQ.ID = isExistData.ID;
                                listUpdateData.Add(dataQ);
                            }
                        }
                        else
                            list.Add(item);
                        break;
                    case "Y":
                        var dataY = new Data();
                        dataY.KPILevelCode = kpilevelcode;
                        dataY.Value = item.Value;
                        dataY.Year = item.PeriodValue;
                        dataY.Yearly = year;
                        dataY.CreateTime = item.CreateTime;
                        dataY.Period = period;

                        if (item.TargetValue.ToDouble() > 0)
                            dataY.Target = item.TargetValue.ToString();
                        else dataY.Target = "0";
                        if (isExistData == null)
                            listCreateData.Add(dataY);
                        else if (isExistData != null)
                        {
                            if (isExistData.Value != valuePeriod || isExistData.Target != target)
                            {
                                dataY.ID = isExistData.ID;
                                listUpdateData.Add(dataY);
                            }
                        }
                        else
                            list.Add(item);
                        break;
                    default:

                        break;
                }
            }

            return Tuple.Create(listCreateData, listUpdateData);
        }
        public async Task<bool> IsExistsTag(int userId, int notifyId)
        {
            return await _dbContext.Tags.AnyAsync(x => x.UserID == userId && x.NotificationID == notifyId);
        }
        /// <summary>
        /// Hàm này dùng để tìm CÁC category của mỗi kpilevelcode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<List<int>> GetAllCategoryByKPILevel(string code)
        {
            var item = await _dbContext.KPILevels.FirstOrDefaultAsync(x => x.KPILevelCode == code && x.Checked == true);
            var kpilevelID = item.ID;
            var listCategory = await _dbContext.CategoryKPILevels.Where(x => x.KPILevelID == kpilevelID && x.Status == true).Select(x => x.CategoryID).ToListAsync();
            return listCategory;
        }

        public async Task<string> GetKPIName(string code)
        {
            var item = await _dbContext.KPILevels.FirstOrDefaultAsync(x => x.KPILevelCode == code && x.Checked == true);
            var kpilevelID = item.KPIID;
            var listCategory = await _dbContext.KPIs.Where(x => x.ID == kpilevelID).FirstOrDefaultAsync();
            return listCategory.Name;
        }

        /// <summary>
        /// Hàm này dùng để tạo url chuyển tới trang ChartPriod của từng data khi update hoặc create
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        private async Task<List<string[]>> ListURLToChartPriodAsync(List<Data> datas)
        {
            var listURLToChartPeriod = new List<string[]>();
            string http = ConfigurationManager.AppSettings["Http"].ToSafetyString();
            string url = string.Empty;
            foreach (var item in datas.DistinctBy(x => x.KPILevelCode))
            {
                var oc = new LevelDAO().GetNode(item.KPILevelCode);
                var kpiname = await GetKPIName(item.KPILevelCode);
                var listCategories = await GetAllCategoryByKPILevel(item.KPILevelCode);
                if (item.Period == "W")
                {

                    foreach (var cat in listCategories)
                    {
                        url = http + $"/ChartPeriod/?kpilevelcode={item.KPILevelCode}&catid={cat}&period={item.Period}&year={DateTime.Now.Year}&start=1&end=53";
                        listURLToChartPeriod.Add(new string[3]
                                           {
                                url,kpiname,oc
                                           });
                    }

                }
                if (item.Period == "M")
                {
                    foreach (var cat in listCategories)
                    {
                        url = http + $"/ChartPeriod/?kpilevelcode={item.KPILevelCode}&catid={cat}&period={item.Period}&year={DateTime.Now.Year}&start=1&end=12";
                        listURLToChartPeriod.Add(new string[3]
                        {
                                url,kpiname,oc
                        });
                    }
                }
                if (item.Period == "Q")
                {
                    foreach (var cat in listCategories)
                    {
                        url = http + $"/ChartPeriod/?kpilevelcode={item.KPILevelCode}&catid={cat}&period={item.Period}&year={DateTime.Now.Year}&start=1&end=4";
                        listURLToChartPeriod.Add(new string[3]
                        {
                                url,kpiname,oc
                        });
                    }
                }
                if (item.Period == "Y")
                {
                    foreach (var cat in listCategories)
                    {
                        url = http + $"/ChartPeriod/?kpilevelcode={item.KPILevelCode}&catid={cat}&period={item.Period}&year={DateTime.Now.Year}&start={DateTime.Now.Year}&end={DateTime.Now.Year}";
                        listURLToChartPeriod.Add(new string[3]
                        {
                                    url,kpiname,oc
                        });
                    }
                }
            }
            return listURLToChartPeriod;

        }
        /// <summary>
        /// Hàm này dùng để xem chi tiết cụ thể của thông báo
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="users"></param>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public async Task CreateListTagAndNotificationDetail(List<string[]> datas, IEnumerable<int> users, int notificationId)
        {
            var listNotification = new List<NotificationDetail>();
            foreach (var item in users)
            {
                foreach (var item2 in datas)
                {
                    listNotification.Add(new NotificationDetail
                    {
                        Content = item2[1] + " ( " + item2[2] + " ) ",
                        URL = item2[0],
                        NotificationID = notificationId,
                        UserID = item
                    });
                }
            }

            _dbContext.NotificationDetails.AddRange(listNotification);
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Tạo thông báo
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public async Task<int> CreateNotification(Notification notification)
        {
            _dbContext.Notifications.Add(notification);
           await  _dbContext.SaveChangesAsync();
            return notification.ID;
        }
        /// <summary>
        /// Tạo list PIC để thông báo gửi mail
        /// </summary>
        /// <param name="listKPILevelID"></param>
        /// <returns></returns>
        public async Task<List<int>> CreateListPIC(List<int> listKPILevelID)
        {
            #region *) Thông báo với các manager, owner, sponsor, updater khi upload xong
            var listManager = (await _dbContext.Managers.Where(x => listKPILevelID.Contains(x.KPILevelID)).ToListAsync()).DistinctBy(x => x.KPILevelID).Select(x => x.UserID).ToList();
            var listOwner = (await _dbContext.Owners.Where(x => listKPILevelID.Contains(x.KPILevelID)).ToListAsync()).DistinctBy(x => x.KPILevelID).Select(x => x.UserID).ToList();
            var listSponsor = (await _dbContext.Sponsors.Where(x => listKPILevelID.Contains(x.KPILevelID)).ToListAsync()).DistinctBy(x => x.KPILevelID).Select(x => x.UserID).ToList();
            var listUpdater = (await _dbContext.Uploaders.Where(x => listKPILevelID.Contains(x.KPILevelID)).ToListAsync()).DistinctBy(x => x.KPILevelID).Select(x => x.UserID).ToList();
            var listAll = listManager.Union(listOwner).Union(listOwner).Union(listSponsor).Union(listUpdater);
            #endregion
            return listManager.Union(listOwner).Union(listOwner).Union(listSponsor).Union(listUpdater).ToList();
        }
        #endregion
        
        public async Task<ImportDataVM> ImportData(List<UploadDataVM> entity, string userUpdate, UserProfileVM userProfileVM)
        {
            #region *) Biến toàn cục
            string http = ConfigurationManager.AppSettings["Http"].ToSafetyString();

            var listAdd = new List<Data>();
            var listTag = new List<Tag>();
            var listSendMail = new List<string>();
            var listUploadKPIVMs = new List<UploadKPIVM>();
            var listDataSuccess = new List<UploadKPIVM>();


            var dataModel = _dbContext.Datas;
            var kpiLevelModel = _dbContext.KPILevels;
            var kpiModel = _dbContext.KPIs;
            var levelModel = _dbContext.Levels;
            #endregion

            #region *) Lọc dữ liệu làm 2 loại là tạo mới và cập nhật
            var tuple = CreateOrUpdateData(entity);
            var listCreate = tuple.Item1;
            var listUpdate = tuple.Item2;
            #endregion

            try
            {
                #region *) Tạo mới
                if (listCreate.Count() > 0)
                {
                    _dbContext.Datas.AddRange(listCreate);
                    await _dbContext.SaveChangesAsync();
                    //Gui mail list nay khi update
                    //Tạo mới xong rồi thì thêm vào list gửi mail 
                    foreach (var item in listCreate)
                    {
                        var tblKPILevelByKPILevelCode = await kpiLevelModel.FirstOrDefaultAsync(x => x.KPILevelCode == item.KPILevelCode);
                        #region *) Upload thành công thì sẽ gửi mail thông báo
                        if (item.Value.ToDouble() > 0)
                        {
                            var dataSuccess = new UploadKPIVM()
                            {
                                KPILevelCode = item.KPILevelCode,
                                Area = levelModel.FirstOrDefault(x => x.ID == tblKPILevelByKPILevelCode.LevelID).Name,
                                KPIName = kpiModel.FirstOrDefault(x => x.ID == tblKPILevelByKPILevelCode.KPIID).Name,
                                Week = item.Week,
                                Month = item.Month,
                                Quarter = item.Quarter,
                                Year = item.Year
                            };
                            listDataSuccess.Add(dataSuccess);
                        } 
                        #endregion

                        #region *) Dưới target thì sẽ gửi mail thông báo
                        if (item.Value.ToDouble() < item.Target.ToDouble())
                        {
                            var dataUploadKPIVM = new UploadKPIVM()
                            {
                                KPILevelCode = item.KPILevelCode,
                                Area = levelModel.FirstOrDefault(x => x.ID == tblKPILevelByKPILevelCode.LevelID).Name,
                                KPIName = kpiModel.FirstOrDefault(x => x.ID == tblKPILevelByKPILevelCode.KPIID).Name,
                                Week = item.Week,
                                Month = item.Month,
                                Quarter = item.Quarter,
                                Year = item.Year
                            };
                            listUploadKPIVMs.Add(dataUploadKPIVM);
                        }
                        #endregion

                    }
                    //Tìm ID theo KPILevelCode trong bản KPILevel
                    var listKPILevel = listCreate.Select(x => x.KPILevelCode).Distinct().ToArray();
                    var listKPILevelID = _dbContext.KPILevels.Where(a => listKPILevel.Contains(a.KPILevelCode)).Select(a => a.ID).ToList();

                    #region *) Lưu vào bảng thông báo
                    var notifyId = await CreateNotification(new Notification
                    {
                        Content = "You have just uploaded some KPIs.",
                        Action = "Upload",
                        TaskName = "Upload KPI Data",
                        UserID = userProfileVM.User.ID,
                        Link = http + "/Home/ListSubNotificationDetail/"
                    });

                    #endregion

                    #region *) Thông báo với các manager, owner, sponsor, updater khi upload xong
                    var listAll =await CreateListPIC(listKPILevelID);
                    #endregion

                    #region *) Tạo danh sách gửi mail
                    listSendMail = _dbContext.Users.Where(x => listAll.Contains(x.ID)).Select(x => x.Email).ToList();
                    #endregion

                    #region *) Thêm vào bảng chi tiết thông báo
                    var listUrls = await ListURLToChartPriodAsync(listCreate);
                    await CreateListTagAndNotificationDetail(listUrls, listAll, notifyId);
                    #endregion
                }
                #endregion

                #region *) Cập nhật
                if (listUpdate.Count() > 0)
                {
                    foreach (var item in listUpdate)
                    {
                        switch (item.Period)
                        {
                            case "W":
                                var dataW = await dataModel.FirstOrDefaultAsync(x => x.KPILevelCode == item.KPILevelCode && x.Period == item.Period && x.Week == item.Week && x.Yearly == item.Yearly);
                                dataW.Value = item.Value;
                                dataW.Target = item.Target;
                                _dbContext.SaveChanges();
                                break;
                            case "M":
                                var dataM = await dataModel.FirstOrDefaultAsync(x => x.KPILevelCode == item.KPILevelCode && x.Period == item.Period && x.Month == item.Month && x.Yearly == item.Yearly);
                                dataM.Value = item.Value;
                                dataM.Target = item.Target;
                                _dbContext.SaveChanges();
                                break;
                            case "Q":

                                var dataQ = await dataModel.FirstOrDefaultAsync(x => x.KPILevelCode == item.KPILevelCode && x.Period == item.Period && x.Quarter == item.Quarter && x.Yearly == item.Yearly);
                                dataQ.Value = item.Value;
                                dataQ.Target = item.Target;
                                _dbContext.SaveChanges();
                                break;
                            case "Y":
                                var dataY = await dataModel.FirstOrDefaultAsync(x => x.KPILevelCode == item.KPILevelCode && x.Period == item.Period && x.Year == item.Year && x.Yearly == item.Yearly);
                                dataY.Value = item.Value;
                                dataY.Target = item.Target;
                                _dbContext.SaveChanges();
                                break;
                            default:
                                break;
                        }
                        var tblKPILevelByKPILevelCode = await kpiLevelModel.FirstOrDefaultAsync(x => x.KPILevelCode == item.KPILevelCode);
                        #region *)
                        if (item.Value.ToDouble() > 0)
                        {
                            var dataSuccess = new UploadKPIVM()
                            {
                                KPILevelCode = item.KPILevelCode,
                                Area = levelModel.FirstOrDefault(x => x.ID == tblKPILevelByKPILevelCode.LevelID).Name,
                                KPIName = kpiModel.FirstOrDefault(x => x.ID == tblKPILevelByKPILevelCode.KPIID).Name,
                                Week = item.Week,
                                Month = item.Month,
                                Quarter = item.Quarter,
                                Year = item.Year
                            };
                            listDataSuccess.Add(dataSuccess);
                        }
                        #endregion
                       
                        #region *) Nếu dữ liệu mà nhỏ hơn mục tiêu thì sẽ gửi mail
                        if (item.Value.ToDouble() < item.Target.ToDouble())
                        {
                            var dataUploadKPIVM = new UploadKPIVM()
                            {
                                KPILevelCode = item.KPILevelCode,
                                Area = levelModel.FirstOrDefault(x => x.ID == tblKPILevelByKPILevelCode.LevelID).Name,
                                KPIName = kpiModel.FirstOrDefault(x => x.ID == tblKPILevelByKPILevelCode.KPIID).Name,
                                Week = item.Week,
                                Month = item.Month,
                                Quarter = item.Quarter,
                                Year = item.Year
                            };
                            listUploadKPIVMs.Add(dataUploadKPIVM);
                        } 
                        #endregion
                    }
                    //Tìm ID theo KPILevelCode trong bản KPILevel
                    var listKPILevel = listUpdate.Select(x => x.KPILevelCode).Distinct().ToArray();
                    var listKPILevelID = _dbContext.KPILevels.Where(a => listKPILevel.Contains(a.KPILevelCode)).Select(a => a.ID).ToList();

                    #region *) Lưu vào bảng thông báo
                    var notifyId = await CreateNotification(new Notification
                    {
                        Content = "You have just uploaded some KPIs.",
                        Action = "Upload",
                        TaskName = "Upload KPI Data",
                        UserID = userProfileVM.User.ID,
                        Link = http + "/Home/ListSubNotificationDetail/"
                    });

                    #endregion

                    #region *) Thông báo với các manager, owner, sponsor, updater khi upload xong
                    var listAll = await CreateListPIC(listKPILevelID);
                    #endregion
                    #region Tạo danh sách gửi mail
                    listSendMail = _dbContext.Users.Where(x => listAll.Contains(x.ID)).Select(x => x.Email).ToList();
                    #endregion

                    #region *) Thêm vào bảng chi tiết thông báo
                    var listUrls = await ListURLToChartPriodAsync(listUpdate);
                    await CreateListTagAndNotificationDetail(listUrls, listAll, notifyId);
                    #endregion
                }

                #endregion
                if (listUploadKPIVMs.Count > 0 || listDataSuccess.Count > 0)
                {
                    return new ImportDataVM
                    {
                        ListUploadKPIVMs = listUploadKPIVMs,
                        ListDataSuccess = listDataSuccess,
                        ListSendMail = listSendMail,
                        Status = true,
                    };
                }
                else
                {
                    return new ImportDataVM
                    {
                        ListUploadKPIVMs = listUploadKPIVMs,
                        ListSendMail = listSendMail,
                        Status = true,
                    };
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return new ImportDataVM
                {
                    ListUploadKPIVMs = listUploadKPIVMs,
                    Status = false,
                };
            }
        }
        
        public async Task<List<NotificationDetail>> GetAllSubNotificationsByIdAsync(int notificationId, int user)
        {
            return await _dbContext.NotificationDetails.Where(x => x.NotificationID == notificationId && x.UserID == user).ToListAsync();
        }
        public bool Update(EF.Data entity)
        {
            var item = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == entity.KPILevelCode);
            try
            {
                item = entity;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }
        public async Task<bool> IsUpdater(int id)
        {
            if (await _dbContext.Uploaders.FirstOrDefaultAsync(x => x.UserID == id) == null)
                return false;
            return true;
        }
        public object UploadData()
        {
            var model = (from a in _dbContext.KPILevels
                         join h in _dbContext.KPIs on a.KPIID equals h.ID
                         join c in _dbContext.Levels on a.LevelID equals c.ID
                         where a.KPILevelCode != null && a.KPILevelCode != string.Empty
                         select new
                         {
                             KPILevelCode = a.KPILevelCode,
                             KPIName = h.Name,
                             LevelName = c.Name,
                             StatusW = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Weekly != null ? true : false,
                             StatusM = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Monthly != null ? true : false,
                             StatusQ = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Quarterly != null ? true : false,
                             StatusY = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == a.KPILevelCode).Yearly != null ? true : false,
                         }).AsEnumerable();
            model = model.ToList();

            return model;
        }

        //public object DataUpLoad(int updaterid, int page, int pageSize)
        //{
        //    var sql = @"SELECT DISTINCT(upl.KPILevelID),
        //                   kpilevel.ID as KPILevelID,
        //                kpilevel.KPILevelCode,
        //                kpi.Name as KPIName,
        //                kpilevel.WeeklyChecked,
        //                kpilevel.MonthlyChecked,
        //                kpilevel.QuarterlyChecked,
        //                kpilevel.YearlyChecked
        //             FROM  dbo.Uploaders as upl, 
        //                dbo.KPILevels as kpilevel,
        //                dbo.KPIs as kpi
        //             WHERE upl.KPILevelID = kpilevel.ID
        //                   and kpi.ID = kpilevel.KPIID
        //                   and upl.UserID = @UserID";

        //    var model = _dbContext.Database.SqlQuery<ListDataUploadVM>(sql, new SqlParameter("UserID", updaterid)).ToList();
        //    var data = model.DistinctBy(x => x.KPIName).Select(x => new
        //    {
        //        x.KPILevelID,
        //        x.KPILevelCode,
        //        x.KPIName,
        //        StateDataW = _dbContext.Datas.Max(a => a.Week) > 0 ? true : false,
        //        StateDataM = _dbContext.Datas.Max(a => a.Month) > 0 ? true : false,
        //        StateDataQ = _dbContext.Datas.Max(a => a.Quarter) > 0 ? true : false,
        //        StateDataY = _dbContext.Datas.Max(a => a.Year) > 0 ? true : false,

        //        StateW = x.StateW.ToInt() > 0 ? true : false,
        //        StateM = x.StateM.ToInt() > 0 ? true : false,
        //        StateQ = x.StateQ.ToInt() > 0 ? true : false,
        //        StateY = x.StateY.ToInt() > 0 ? true : false,

        //    }).ToList();

        //    int totalRow = data.Count();

        //    data = data.OrderByDescending(x => x.KPIName)
        //     .Skip((page - 1) * pageSize)
        //     .Take(pageSize).ToList();
        //    return new
        //    {
        //        data = data,
        //        page,
        //        pageSize,
        //        status = true,
        //        total = totalRow,
        //        isUpdater = true
        //    };

        //}

        public async Task<object> ListKPIUpload(int updaterid, int page, int pageSize)
        {
            var datas = await _dbContext.Datas.ToListAsync();
            var model = (await (from u in _dbContext.Uploaders.Where(x => x.UserID == updaterid)
                                join item in _dbContext.KPILevels on u.KPILevelID equals item.ID
                                join cat in _dbContext.CategoryKPILevels.Where(x => x.Status == true) on u.KPILevelID equals cat.KPILevelID
                                join kpi in _dbContext.KPIs on item.KPIID equals kpi.ID
                                select new
                                {
                                    KPILevelID = u.KPILevelID,
                                    KPIName = kpi.Name,
                                    StateDataW = item.WeeklyChecked ?? false,
                                    StateDataM = item.MonthlyChecked ?? false,
                                    StateDataQ = item.QuarterlyChecked ?? false,
                                    StateDataY = item.YearlyChecked ?? false,
                                }).ToListAsync())
                         .Select(x => new ListKPIUploadVM
                         {
                             KPILevelID = x.KPILevelID,
                             KPIName = x.KPIName,
                             StateW = datas.Max(x => x.Week) > 0 ? true : false,
                             StateM = datas.Max(x => x.Month) > 0 ? true : false,
                             StateQ = datas.Max(x => x.Quarter) > 0 ? true : false,
                             StateY = datas.Max(x => x.Year) > 0 ? true : false,

                             StateDataW = x.StateDataW,
                             StateDataM = x.StateDataM,
                             StateDataQ = x.StateDataQ,
                             StateDataY = x.StateDataY
                         }).DistinctBy(p => p.KPIName).ToList();
            //bảng uploader có nhiều KPILevel trùng nhau vì 1 KPILevel thuộc nhiều Category khác nhau 
            //nên ta phải distinctBy KPILevelID để lấy ra danh sách KPI không bị trùng nhau vì yêu cầu chỉ cần lấy ra KPI để upload dữ liệu
            ////Mỗi KPILevel ứng với 1 KPI khác nhau
            int totalRow = model.Count();

            model = model.OrderByDescending(x => x.KPIName)
             .Skip((page - 1) * pageSize)
             .Take(pageSize).ToList();

            return new
            {
                data = model,
                page,
                pageSize,
                status = true,
                total = totalRow,
                isUpdater = true

            };
        }

        public async Task<object> UpLoadKPILevel(int userid, int page, int pageSize)
        {
            var datas = _dbContext.Datas;
            var model = await (from u in _dbContext.Users
                               join l in _dbContext.Levels on u.LevelID equals l.ID
                               join item in _dbContext.KPILevels on l.ID equals item.LevelID
                               join kpi in _dbContext.KPIs on item.KPIID equals kpi.ID
                               where u.ID == userid && item.Checked == true
                               select new KPIUpLoadVM
                               {
                                   KPIName = kpi.Name,
                                   StateW = item.WeeklyChecked == true && datas.Where(x => x.KPILevelCode == item.KPILevelCode).Max(x => x.Week) > 0 ? true : false,
                                   StateM = item.MonthlyChecked == true && datas.Where(x => x.KPILevelCode == item.KPILevelCode).Max(x => x.Month) > 0 ? true : false,
                                   StateQ = item.QuarterlyChecked == true && datas.Where(x => x.KPILevelCode == item.KPILevelCode).Max(x => x.Quarter) > 0 ? true : false,
                                   StateY = item.YearlyChecked == true && datas.Where(x => x.KPILevelCode == item.KPILevelCode).Max(x => x.Year) > 0 ? true : false,

                                   StateDataW = item.WeeklyChecked ?? false,
                                   StateDataM = item.MonthlyChecked ?? false,
                                   StateDataQ = item.QuarterlyChecked ?? false,
                                   StateDataY = item.YearlyChecked ?? false,

                               }).ToListAsync();
            int totalRow = model.Count();
            model = model.OrderByDescending(x => x.KPIName)
              .Skip((page - 1) * pageSize)
              .Take(pageSize).ToList();
            var vm = new WorkplaceVM()
            {
                KPIUpLoads = model,
                total = totalRow,
                page = page,
                pageSize = pageSize
            };
            return vm;
        }

        public async Task<object> LoadActionPlan(string role, int page, int pageSize)
        {
            var model = new List<ActionPlanTagVM>();
            switch (role.ToSafetyString().ToUpper())
            {
                case "MAN":
                    model = (await (from d in _dbContext.Datas
                                    join ac in _dbContext.ActionPlans on d.ID equals ac.DataID
                                    join kpilevelcode in _dbContext.KPILevels on d.KPILevelCode equals kpilevelcode.KPILevelCode
                                    join own in _dbContext.Managers on kpilevelcode.ID equals own.KPILevelID
                                    select new
                                    {
                                        TaskName = ac.Title,
                                        Description = ac.Description,
                                        DuaDate = ac.Deadline,
                                        UpdateSheuleDate = ac.UpdateSheduleDate,
                                        ActualFinishDate = ac.ActualFinishDate,
                                        Status = ac.Status,
                                        PIC = ac.Tag,
                                        Approved = ac.ApprovedStatus,
                                        KPIID = _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == d.KPILevelCode).KPIID,
                                        KPILevelID = _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == d.KPILevelCode).ID
                                    }).Distinct()
                      .ToListAsync())
                      .Select(x => new ActionPlanTagVM
                      {
                          TaskName = x.TaskName,
                          Description = x.Description,
                          DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                          UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                          ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                          Status = x.Status,
                          PIC = x.PIC,
                          Approved = x.Approved,
                          KPIName = _dbContext.KPIs.FirstOrDefault(a => a.ID == x.KPIID).Name

                      }).ToList();
                    break;
                case "OWN":

                    model = (await (from d in _dbContext.Datas
                                    join ac in _dbContext.ActionPlans on d.ID equals ac.DataID
                                    join kpilevelcode in _dbContext.KPILevels on d.KPILevelCode equals kpilevelcode.KPILevelCode
                                    join own in _dbContext.Owners on kpilevelcode.ID equals own.KPILevelID
                                    select new
                                    {
                                        TaskName = ac.Title,
                                        Description = ac.Description,
                                        DuaDate = ac.Deadline,
                                        UpdateSheuleDate = ac.UpdateSheduleDate,
                                        ActualFinishDate = ac.ActualFinishDate,
                                        Status = ac.Status,
                                        PIC = ac.Tag,
                                        Approved = ac.ApprovedStatus,
                                        KPIID = _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == d.KPILevelCode).KPIID,
                                        KPILevelID = _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == d.KPILevelCode).ID
                                    }).Distinct()
                     .ToListAsync())
                     .Select(x => new ActionPlanTagVM
                     {
                         TaskName = x.TaskName,
                         Description = x.Description,
                         DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                         UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                         ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                         Status = x.Status,
                         PIC = x.PIC,
                         Approved = x.Approved,
                         KPIName = _dbContext.KPIs.FirstOrDefault(a => a.ID == x.KPIID).Name

                     }).ToList();
                    break;
                case "UPD":

                    model = (await (from d in _dbContext.Datas
                                    join ac in _dbContext.ActionPlans on d.ID equals ac.DataID
                                    join kpilevelcode in _dbContext.KPILevels on d.KPILevelCode equals kpilevelcode.KPILevelCode
                                    join own in _dbContext.Uploaders on kpilevelcode.ID equals own.KPILevelID
                                    select new
                                    {
                                        KPILevelCode = d.KPILevelCode,
                                        TaskName = ac.Title,
                                        Description = ac.Description,
                                        DuaDate = ac.Deadline,
                                        UpdateSheuleDate = ac.UpdateSheduleDate,
                                        ActualFinishDate = ac.ActualFinishDate,
                                        Status = ac.Status,
                                        PIC = ac.Tag,
                                        Approved = ac.ApprovedStatus,
                                        KPIID = _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == d.KPILevelCode).KPIID,
                                        KPILevelID = _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == d.KPILevelCode).ID
                                    }).Distinct()
                     .ToListAsync())
                     .Select(x => new ActionPlanTagVM
                     {
                         TaskName = x.TaskName,
                         Description = x.Description,
                         DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                         UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                         ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                         Status = x.Status,
                         PIC = x.PIC,
                         Approved = x.Approved,
                         KPIName = _dbContext.KPIs.FirstOrDefault(a => a.ID == x.KPIID).Name

                     }).ToList();
                    break;
                case "SPO":

                    model = (await (from d in _dbContext.Datas
                                    join ac in _dbContext.ActionPlans on d.ID equals ac.DataID
                                    join kpilevelcode in _dbContext.KPILevels on d.KPILevelCode equals kpilevelcode.KPILevelCode
                                    join own in _dbContext.Sponsors on kpilevelcode.ID equals own.KPILevelID
                                    select new
                                    {
                                        TaskName = ac.Title,
                                        Description = ac.Description,
                                        DuaDate = ac.Deadline,
                                        UpdateSheuleDate = ac.UpdateSheduleDate,
                                        ActualFinishDate = ac.ActualFinishDate,
                                        Status = ac.Status,
                                        PIC = ac.Tag,
                                        Approved = ac.ApprovedStatus,
                                        KPIID = _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == d.KPILevelCode).KPIID,
                                        KPILevelID = _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == d.KPILevelCode).ID
                                    }).Distinct()
                    .ToListAsync())
                    .Select(x => new ActionPlanTagVM
                    {
                        TaskName = x.TaskName,
                        KPIName = _dbContext.KPIs.FirstOrDefault(a => a.ID == x.KPIID).Name,
                        Description = x.Description,
                        DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                        UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                        ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                        Status = x.Status,
                        PIC = x.PIC,
                        Approved = x.Approved,

                    }).ToList();
                    break;
                case "PAR":

                    model = (await (from d in _dbContext.Datas
                                    join ac in _dbContext.ActionPlans on d.ID equals ac.DataID
                                    join kpilevelcode in _dbContext.KPILevels on d.KPILevelCode equals kpilevelcode.KPILevelCode
                                    join own in _dbContext.Participants on kpilevelcode.ID equals own.KPILevelID
                                    select new
                                    {
                                        TaskName = ac.Title,
                                        Description = ac.Description,
                                        DuaDate = ac.Deadline,
                                        UpdateSheuleDate = ac.UpdateSheduleDate,
                                        ActualFinishDate = ac.ActualFinishDate,
                                        Status = ac.Status,
                                        PIC = ac.Tag,
                                        Approved = ac.ApprovedStatus,
                                        KPIID = _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == d.KPILevelCode).KPIID,
                                        KPILevelID = _dbContext.KPILevels.FirstOrDefault(a => a.KPILevelCode == d.KPILevelCode).ID
                                    }).Distinct()
                    .ToListAsync())
                    .Select(x => new ActionPlanTagVM
                    {
                        TaskName = x.TaskName,
                        Description = x.Description,
                        DueDate = x.DuaDate.ToSafetyString("MM/dd/yyyy"),
                        UpdateSheduleDate = x.UpdateSheuleDate.ToSafetyString("MM/dd/yyyy"),
                        ActualFinishDate = x.ActualFinishDate.ToSafetyString("MM/dd/yyyy"),
                        Status = x.Status,
                        PIC = x.PIC,
                        Approved = x.Approved,
                        KPIName = _dbContext.KPIs.FirstOrDefault(a => a.ID == x.KPIID).Name

                    }).ToList();
                    break;
                default:

                    break;
            }
            int totalRow = model.Count();
            model = model.OrderByDescending(x => x.KPIName)
              .Skip((page - 1) * pageSize)
              .Take(pageSize).ToList();

            return new
            {
                status = true,
                data = model,
                total = totalRow,
                page = page,
                pageSize = pageSize
            };
        }

        public string GetValueData(string KPILevelCode, string CharacterPeriod, int period)
        {
            var value = CharacterPeriod.ToSafetyString();
            string obj = "0";
            switch (value)
            {
                case "W":
                    var item = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == KPILevelCode && x.Period == "W" && x.Week == period);
                    if (item != null)
                        obj = item.Value;
                    break;
                case "M":
                    var item1 = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == KPILevelCode && x.Period == "M" && x.Month == period);
                    if (item1 != null)
                        obj = item1.Value;
                    break;
                case "Q":
                    var item2 = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == KPILevelCode && x.Period == "Q" && x.Quarter == period);
                    if (item2 != null)
                        obj = item2.Value;
                    break;
                case "Y":
                    var item3 = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == KPILevelCode && x.Period == "Y" && x.Year == period);
                    if (item3 != null)
                        obj = item3.Value;
                    break;
            }
            return obj;
        }
        public string GetTargetData(string KPILevelCode, string CharacterPeriod, int period)
        {
            var value = CharacterPeriod.ToSafetyString();
            string obj = "0";
            switch (value)
            {
                case "W":
                    var item = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == KPILevelCode && x.Period == "W" && x.Week == period);
                    if (item != null)
                        obj = item.Target;
                    break;
                case "M":
                    var item1 = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == KPILevelCode && x.Period == "M" && x.Month == period);
                    if (item1 != null)
                        obj = item1.Target;
                    break;
                case "Q":
                    var item2 = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == KPILevelCode && x.Period == "Q" && x.Quarter == period);
                    if (item2 != null)
                        obj = item2.Target;
                    break;
                case "Y":
                    var item3 = _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == KPILevelCode && x.Period == "Y" && x.Year == period);
                    if (item3 != null)
                        obj = item3.Target;
                    break;
            }
            return obj;
        }
        public List<DataExportVM> DataExport(int userid)
        {
            var datas = _dbContext.Datas;
            var kpis = _dbContext.KPIs;
            var model = (from u in _dbContext.Uploaders.Where(x => x.UserID == userid).DistinctBy(x => x.KPILevelID)
                         join kpiLevel in _dbContext.KPILevels on u.KPILevelID equals kpiLevel.ID
                         join cat in _dbContext.CategoryKPILevels.Where(x => x.Status == true) on u.KPILevelID equals cat.KPILevelID
                         join kpi in _dbContext.KPIs on kpiLevel.KPIID equals kpi.ID
                         join l in _dbContext.Levels on kpiLevel.LevelID equals l.ID
                         where kpiLevel.Checked == true
                         select new DataExportVM
                         {
                             Area = l.Name,
                             KPILevelCode = kpiLevel.KPILevelCode,
                             KPIName = kpi.Name,
                             StateW = kpiLevel.WeeklyChecked ?? false,
                             StateM = kpiLevel.MonthlyChecked ?? false,
                             StateQ = kpiLevel.QuarterlyChecked ?? false,
                             StateY = kpiLevel.YearlyChecked ?? false,

                             PeriodValueW = datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode).FirstOrDefault() != null ? (int?)datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode).Max(x => x.Week) ?? 0 : 0,
                             PeriodValueM = datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode).FirstOrDefault() != null ? (int?)datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode).Max(x => x.Month) ?? 0 : 0,
                             PeriodValueQ = datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode).FirstOrDefault() != null ? (int?)datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode).Max(x => x.Quarter) ?? 0 : 0,
                             PeriodValueY = datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode).FirstOrDefault() != null ? (int?)datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode).Max(x => x.Year) ?? 0 : 0,

                             UploadTimeW = kpiLevel.Weekly,
                             UploadTimeM = kpiLevel.Monthly,
                             UploadTimeQ = kpiLevel.Quarterly,
                             UploadTimeY = kpiLevel.Yearly,
                             //TargetValueW = kpi.Unit == 1 ? "not require" : "require"
                         }).ToList();

            return model;
        }
        //group by, join sample
        //This is a sample
        public List<DataExportVM> DataExport2(int userid)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();
            var model = (from l in _dbContext.Levels
                         join u in _dbContext.Users on l.ID equals u.LevelID
                         join item in _dbContext.KPILevels on l.ID equals item.LevelID
                         join d in _dbContext.Datas on item.KPILevelCode equals d.KPILevelCode into JoinItem
                         from joi in JoinItem.DefaultIfEmpty()
                         join k in _dbContext.KPIs on item.KPIID equals k.ID
                         where u.ID == userid && item.Checked == true
                         group new { u, l, item, joi, k } by new
                         {
                             u.Username,
                             Area = l.Name,
                             KPIName = k.Name,
                             item.KPILevelCode,
                             item.Checked,
                             item.WeeklyChecked,
                             item.MonthlyChecked,
                             item.QuarterlyChecked,
                             item.YearlyChecked,

                         } into g
                         select new
                         {
                             g.Key.Username,
                             Area = g.Key.Area,
                             KPIName = g.Key.KPIName,
                             g.Key.KPILevelCode,
                             g.Key.Checked,
                             g.Key.WeeklyChecked,
                             g.Key.MonthlyChecked,
                             g.Key.QuarterlyChecked,
                             g.Key.YearlyChecked,

                             PeriodValueW = g.Select(x => x.joi.Week).Max(),
                             PeriodValueM = g.Select(x => x.joi.Month).Max(),
                             PeriodValueQ = g.Select(x => x.joi.Quarter).Max(),
                             PeriodValueY = g.Select(x => x.joi.Year).Max(),
                         }).AsEnumerable()
                         .Select(x => new DataExportVM
                         {
                             Value = "0",
                             Year = currentYear,
                             KPILevelCode = x.KPILevelCode,
                             KPIName = x.KPIName,
                             Area = x.Area,
                             Remark = string.Empty,
                             PeriodValueW = x.PeriodValueW,
                             PeriodValueM = x.PeriodValueM,
                             PeriodValueQ = x.PeriodValueQ,
                             PeriodValueY = x.PeriodValueY,
                         });

            return model.ToList();
        }
        public async Task<object> UpLoadKPILevelTrack(int userid, int page, int pageSize)
        {
            var model1 = await new LevelDAO().GetListTreeForWorkplace(userid);
            var relative = ConvertHierarchicalToFlattenObject(model1);
            var itemuser = _dbContext.Users.FirstOrDefault(x => x.ID == userid).LevelID;
            var level = _dbContext.Levels.Select(
                x => new LevelVM
                {
                    ID = x.ID,
                    Name = x.Name,
                    Code = x.Code,
                    ParentID = x.ParentID,
                    ParentCode = x.ParentCode,
                    LevelNumber = x.LevelNumber,
                    State = x.State,
                    CreateTime = x.CreateTime
                }).ToList();
            // here you get your list
            var itemlevel = _dbContext.Levels.FirstOrDefault(x => x.ID == itemuser);
            var tree = GetTree(level, itemuser).FirstOrDefault();

            var relative2 = ConvertHierarchicalToFlattenObject2(tree);
            //var KPILevels = _dbContext.KPILevels.Where(x => x.Checked == true).ToList();
            var list = new List<KPIUpLoadVM>();


            var userKPIlevel = _dbContext.KPILevels.Where(x => x.Checked == true && x.LevelID == itemuser).ToList();
            foreach (var item in userKPIlevel)
            {
                var data = new KPIUpLoadVM()
                {
                    KPIName = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Name,
                    Area = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name,
                    StateW = item.WeeklyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Week > 0) != null ? true : false,
                    StateM = item.MonthlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Month > 0) != null ? true : false,
                    StateQ = item.QuarterlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Quarter > 0) != null ? true : false,
                    StateY = item.YearlyChecked == true && _dbContext.Datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Year > 0) != null ? true : false,

                    StateDataW = (bool?)item.WeeklyChecked ?? false,
                    StateDataM = (bool?)item.MonthlyChecked ?? false,
                    StateDataQ = (bool?)item.QuarterlyChecked ?? false,
                    StateDataY = (bool?)item.YearlyChecked ?? false,

                };
                list.Add(data);
            }
            var total = 0;
            if (relative2 != null)
            {
                var KPILevels = new List<KPILevel>();
                foreach (var aa in relative2)
                {
                    if (aa != null)
                    {
                        KPILevels = (await _dbContext.KPILevels.Where(x => x.Checked == true && x.LevelID == aa.ID)
                       .Select(a => new
                       {
                           a.KPIID,
                           a.LevelID,
                           a.WeeklyChecked,
                           a.MonthlyChecked,
                           a.QuarterlyChecked,
                           a.YearlyChecked,
                           a.KPILevelCode
                       }).ToListAsync())
                       .Select(x => new KPILevel
                       {
                           KPIID = x.KPIID,
                           LevelID = x.LevelID,
                           WeeklyChecked = x.WeeklyChecked,
                           MonthlyChecked = x.MonthlyChecked,
                           QuarterlyChecked = x.QuarterlyChecked,
                           YearlyChecked = x.YearlyChecked,
                           KPILevelCode = x.KPILevelCode
                       }).ToList();
                    }

                    if (KPILevels != null)
                    {
                        foreach (var item in KPILevels)
                        {
                            var data = new KPIUpLoadVM()
                            {
                                KPIName = (await _dbContext.KPIs.FirstOrDefaultAsync(x => x.ID == item.KPIID)).Name,
                                Area = (await _dbContext.Levels.FirstOrDefaultAsync(x => x.ID == item.LevelID)).Name,
                                StateW = item.WeeklyChecked == true && (await _dbContext.Datas.FirstOrDefaultAsync(x => x.KPILevelCode == item.KPILevelCode && x.Week > 0)) != null ? true : false,
                                StateM = item.MonthlyChecked == true && (await _dbContext.Datas.FirstOrDefaultAsync(x => x.KPILevelCode == item.KPILevelCode && x.Month > 0)) != null ? true : false,
                                StateQ = item.QuarterlyChecked == true && (await _dbContext.Datas.FirstOrDefaultAsync(x => x.KPILevelCode == item.KPILevelCode && x.Quarter > 0)) != null ? true : false,
                                StateY = item.YearlyChecked == true && (await _dbContext.Datas.FirstOrDefaultAsync(x => x.KPILevelCode == item.KPILevelCode && x.Year > 0)) != null ? true : false
                            };
                            list.Add(data);
                        }
                    }

                }
                total = list.Count();
                list = list.OrderBy(x => x.KPIName).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            }

            return new
            {
                model = list,
                total,
                page,
                pageSize
            };
        }
        /// <summary>
        /// Convert the nested hierarchical object to flatten object
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerable<KPITreeViewModel> ConvertHierarchicalToFlattenObject(KPITreeViewModel parent)
        {
            yield return parent;
            foreach (KPITreeViewModel child in parent.children) // check null if you must
                foreach (KPITreeViewModel relative in ConvertHierarchicalToFlattenObject(child))
                    yield return relative;
        }
        public IEnumerable<LevelVM> ConvertHierarchicalToFlattenObject2(LevelVM parent)
        {
            if (parent == null)
                parent = new LevelVM();
            if (parent.Levels == null)
                parent.Levels = new List<LevelVM>();
            yield return parent;
            foreach (LevelVM child in parent.Levels) // check null if you must
                foreach (LevelVM relative in ConvertHierarchicalToFlattenObject2(child))
                    yield return relative;
        }
        public List<LevelVM> GetTree(List<LevelVM> list, int parent)
        {
            return list.Where(x => x.ParentID == parent).Select(x => new LevelVM
            {
                ID = x.ID,
                Name = x.Name,
                Levels = GetTree(list, x.ID)
            }).ToList();
        }
        public async Task<object> KPIRelated(int levelId, int page, int pageSize)
        {
            var obj = await _dbContext.KPILevels.Where(x => x.LevelID == levelId && x.Checked == true).ToListAsync();
            var kpiName = _dbContext.KPIs;
            var datas = _dbContext.Datas;
            var list = new List<KPIUpLoadVM>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    var data = new KPIUpLoadVM()
                    {
                        KPIName = kpiName.FirstOrDefault(x => x.ID == item.KPIID).Name,
                        StateW = item.WeeklyChecked == true && datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Week > 0) != null ? true : false,
                        StateM = item.MonthlyChecked == true && datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Month > 0) != null ? true : false,
                        StateQ = item.QuarterlyChecked == true && datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Quarter > 0) != null ? true : false,
                        StateY = item.YearlyChecked == true && datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Year > 0) != null ? true : false,

                        StateDataW = item.WeeklyChecked ?? false,
                        StateDataM = item.MonthlyChecked ?? false,
                        StateDataQ = item.QuarterlyChecked ?? false,
                        StateDataY = item.YearlyChecked ?? false,
                    };
                    list.Add(data);
                }
                var total = list.Count();
                list = list.OrderBy(x => x.KPIName).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return new
                {
                    model = list,
                    total,
                    page,
                    pageSize,
                    status = true
                };
            }
            return new
            {
                status = false
            };
        }
        public object MyWorkplace(int levelId, int page, int pageSize)
        {
            var obj = _dbContext.KPILevels.Where(x => x.LevelID == levelId && x.Checked == true).ToList();
            var kpiName = _dbContext.KPIs;
            var datas = _dbContext.Datas;
            var list = new List<KPIUpLoadVM>();
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    var data = new KPIUpLoadVM()
                    {
                        KPIName = kpiName.FirstOrDefault(x => x.ID == item.KPIID).Name,
                        StateW = item.WeeklyChecked == true && datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Week > 0) != null ? true : false,
                        StateM = item.MonthlyChecked == true && datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Month > 0) != null ? true : false,
                        StateQ = item.QuarterlyChecked == true && datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Quarter > 0) != null ? true : false,
                        StateY = item.YearlyChecked == true && datas.FirstOrDefault(x => x.KPILevelCode == item.KPILevelCode && x.Year > 0) != null ? true : false

                    };
                    list.Add(data);
                }
                var total = list.Count();
                list = list.OrderBy(x => x.KPIName).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return new
                {
                    model = list,
                    total,
                    status = true
                };
            }
            return new
            {
                status = false
            };
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
