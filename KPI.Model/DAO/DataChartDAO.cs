using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using System.Data.Entity;

namespace KPI.Model.DAO
{
    public class DataChartDAO
    {
        KPIDbContext _dbContext = null;
        public DataChartDAO()
        {
            this._dbContext = new KPIDbContext();
        }

        public List<LateOnUpLoad> LateOnUpLoads(int userId, int notificationId)
        {
            
            return _dbContext.LateOnUpLoads.Where(x => x.UserID == userId && x.NotificationID == notificationId).ToList();
        }

        public Task AddLateOnUploadAsync(List<LateOnUpLoad> lateOnUpLoads)
        {
            try
            {
                _dbContext.LateOnUpLoads.AddRange(lateOnUpLoads);
                _dbContext.SaveChanges();
            }
            catch 
            {

            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// Linq Express tương đương với câu lệnh Join trong SQL
        /// SELECT dbo.Users.FullName FROM dbo.Uploaders
        /// JOIN dbo.Users ON dbo.Uploaders.UserID = dbo.Users.ID 
        /// </summary>
        /// <param name="kpilevelid"></param>
        /// <param name="categoryid"></param>
        /// <returns></returns>
        public object Updaters(int kpilevelid, int categoryid)
        {
            //var user = _dbContext.Users;
            var list = _dbContext.Uploaders
                .Where(x => x.CategoryID == categoryid && x.KPILevelID == kpilevelid)
                .Join(_dbContext.Users,
                cat => cat.UserID,
                user => user.ID,
                (cat, user) => new
                {
                    user.FullName
                }).Select(x => x.FullName).ToArray();
            var count = list.Length;
            if (count == 0)
                return "N/A";
            else if (list == null)
                return "N/A";
            else
                return string.Join(",", list);

        }
        public object Owners(int kpilevelid, int categoryid)
        {
            //var user = _dbContext.Users;
            var list = _dbContext.Owners
                .Where(x => x.CategoryID == categoryid && x.KPILevelID == kpilevelid)
                .Join(_dbContext.Users,
                cat => cat.UserID,
                user => user.ID,
                (cat, user) => new
                {
                    user.FullName
                }).Select(x => x.FullName).ToArray();
            var count = list.Length;
            if (count == 0)
                return "N/A";
            else if (list == null)
                return "N/A";
            else
                return string.Join(",", list);
        }
        public object Managers(int kpilevelid, int categoryid)
        {
            //var user = _dbContext.Users;
            var list = _dbContext.Managers
                .Where(x => x.CategoryID == categoryid && x.KPILevelID == kpilevelid)
                .Join(_dbContext.Users,
                cat => cat.UserID,
                user => user.ID,
                (cat, user) => new
                {
                    user.FullName
                }).Select(x => x.FullName).ToArray();
            var count = list.Length;
            if (count == 0)
                return "N/A";
            else if (list == null)
                return "N/A";
            else
                return string.Join(",", list);
        }
        public object Sponsors(int kpilevelid, int categoryid)
        {
            //var user = _dbContext.Users;
            var list = _dbContext.Sponsors
                .Where(x => x.CategoryID == categoryid && x.KPILevelID == kpilevelid)
                .Join(_dbContext.Users,
                cat => cat.UserID,
                user => user.ID,
                (cat, user) => new
                {
                    user.FullName
                }).Select(x => x.FullName).ToArray();
            var count = list.Length;
            if (count == 0)
                return "N/A";
            else if (list == null)
                return "N/A";
            else
                return string.Join(",", list);
        }
        public object Participants(int kpilevelid, int categoryid)
        {
            //var user = _dbContext.Users;
            var list = _dbContext.Participants
                .Where(x => x.CategoryID == categoryid && x.KPILevelID == kpilevelid)
                .Join(_dbContext.Users,
                cat => cat.UserID,
                user => user.ID,
                (cat, user) => new
                {
                    user.FullName
                }).Select(x => x.FullName).ToArray();
            var count = list.Length;
            if (count == 0)
                return "N/A";
            else if (list == null)
                return "N/A";
            else
                return string.Join(",", list);
        }
        public async Task<object> GetAllDataByCategory(int categoryid, string period, int? start, int? end, int? year)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();
            //labels của chartjs mặc định có 53 phần tử
            List<DatasetVM> listDatasetVM = new List<DatasetVM>();
            if (!period.IsNullOrEmpty())
            {
                var datasets = new List<object>();
                //labels của chartjs mặc định có 53 phần tử
                List<string> listLabels = new List<string>();

                var dataremarks = new List<Dataremark>();

                //var tbldata = _dbContext.Datas;
                

                var listKPILevelID = await GetAllManagerOwnerUpdaterSponsorParticipant(categoryid);

                if (year == 0)
                    year = currentYear;

                if (period.ToLower() == "w")
                {
                    foreach (var item in listKPILevelID)
                    {
                        //var kpi = tblKPI.Find(item.KPIID).Name;
                        var tblCategory =await _dbContext.Categories.FindAsync(item.CategoryID);
                        var categorycode = tblCategory.Code;

                        var obj = await GetAllOwner(categoryid, item.KPILevelID);
                        var tbldata = await _dbContext.Datas
                       .Where(x => x.KPILevelCode == item.KPILevelCode && x.Period == "W" && x.Yearly == year)
                        .OrderBy(x => x.Week)
                        .Select(x => new
                        {
                            ID = x.ID,
                            Value = x.Value,
                            Remark = x.Remark,
                            x.Target,
                            Week = x.Week
                        })
                        .ToListAsync();
                        dataremarks = tbldata
                                     .Where(a => a.Value.ToDouble() > 0)
                     .Select(a => new Dataremark
                     {
                         ID = a.ID,
                         Value = a.Value,
                         Remark = a.Remark,
                         Week = a.Week,
                         ValueArray = new string[3] { a.Value, (a.Target.ToDouble() >= a.Value.ToDouble() ? false : true).ToString(), a.Target },
                     }).ToList();

                        if (start > 0 && end > 0)
                        {
                            dataremarks = dataremarks.Where(x => x.Week >= start && x.Week <= end).ToList();
                        }

                        var datasetsvm = new DatasetVM();
                        datasetsvm.KPIName = item.KPIName;
                        datasetsvm.Manager = obj.Manager;
                        datasetsvm.Owner = obj.Owner;
                        datasetsvm.Updater = obj.Updater;
                        datasetsvm.Sponsor = obj.Sponsor;
                        datasetsvm.Participant = obj.Participant;
                        datasetsvm.CategoryName = tblCategory.Name;
                        datasetsvm.CategoryCode = categorycode;
                        datasetsvm.KPILevelCode = item.KPILevelCode;
                        datasetsvm.Datasets = dataremarks;
                        datasetsvm.Period = "Weekly";

                        listDatasetVM.Add(datasetsvm);

                    }
                }
                else if (period.ToLower() == "m")
                {
                    foreach (var item in listKPILevelID)
                    {
                        var tblCategory =await _dbContext.Categories.FindAsync(item.CategoryID);
                        var categorycode = tblCategory.Code;

                        var obj = await GetAllOwner(categoryid, item.KPILevelID);
                        var tbldata = await _dbContext.Datas
                            .Where(x => x.KPILevelCode == item.KPILevelCode && x.Period == "M" && x.Yearly == year)
                          .OrderBy(x => x.Month)
                          .Select(x => new
                          {
                              ID = x.ID,
                              Value = x.Value,
                              Remark = x.Remark,
                              x.Target,
                              Month = x.Month,

                          }).ToListAsync();
                        dataremarks = tbldata
                          .Where(a => a.Value.ToDouble() > 0)
                         .Select(a => new Dataremark
                         {
                             ID = a.ID,
                             Value = a.Value,
                             Remark = a.Remark,
                             Week = a.Month,
                             ValueArray = new string[3] { a.Value, (a.Target.ToDouble() >= a.Value.ToDouble() ? false : true).ToString(), a.Target },
                         }).ToList();

                        if (start > 0 && end > 0)
                        {
                            dataremarks = dataremarks.Where(x => x.Week >= start && x.Week <= end).ToList();
                        }
                        var datasetsvm = new DatasetVM();
                        datasetsvm.KPIName = item.KPIName;
                        datasetsvm.Manager = obj.Manager;
                        datasetsvm.Owner = obj.Owner;
                        datasetsvm.Updater = obj.Updater;
                        datasetsvm.Sponsor = obj.Sponsor;
                        datasetsvm.Participant = obj.Participant;
                        datasetsvm.CategoryName = tblCategory.Name;
                        datasetsvm.CategoryCode = categorycode;
                        datasetsvm.KPILevelCode = item.KPILevelCode;

                        datasetsvm.Datasets = dataremarks;
                        datasetsvm.Period = "Monthly";

                        listDatasetVM.Add(datasetsvm);

                    }
                }
                else if (period.ToLower() == "q")
                {
                    foreach (var item in listKPILevelID)
                    {
                        var tblCategory = await _dbContext.Categories.FindAsync(item.CategoryID);
                        var categorycode = tblCategory.Code;

                        var obj = await GetAllOwner(categoryid, item.KPILevelID);
                        var tbldata = await _dbContext.Datas
                            .Where(x => x.KPILevelCode == item.KPILevelCode && x.Period == "Q" && x.Yearly == year)
                          .OrderBy(x => x.Quarter)
                         .Select(x => new
                         {
                             ID = x.ID,
                             Value = x.Value,
                             Remark = x.Remark,
                             x.Target,
                             Quarter = x.Quarter,

                         }).ToListAsync();
                        dataremarks = tbldata
                        .Where(a => a.Value.ToDouble() > 0)
                       .Select(a => new Dataremark
                       {
                           ID = a.ID,
                           Value = a.Value,
                           Remark = a.Remark,
                           Week = a.Quarter,
                           ValueArray = new string[3] { a.Value, (a.Target.ToDouble() >= a.Value.ToDouble() ? false : true).ToString(), a.Target },
                       }).ToList();
                        if (start > 0 && end > 0)
                        {
                            dataremarks = dataremarks.Where(x => x.Week >= start && x.Week <= end).ToList();
                        }
                        var datasetsvm = new DatasetVM();
                        datasetsvm.KPIName = item.KPIName;
                        datasetsvm.Manager = obj.Manager;
                        datasetsvm.Owner = obj.Owner;
                        datasetsvm.Updater = obj.Updater;
                        datasetsvm.Sponsor = obj.Sponsor;
                        datasetsvm.Participant = obj.Participant;
                        datasetsvm.CategoryName = tblCategory.Name;
                        datasetsvm.CategoryCode = categorycode;
                        datasetsvm.KPILevelCode = item.KPILevelCode;

                        datasetsvm.Datasets = dataremarks;
                        datasetsvm.Period = "Quarterly";

                        listDatasetVM.Add(datasetsvm);

                    }
                }
                else if (period.ToLower() == "y")
                {
                    foreach (var item in listKPILevelID)
                    {
                        var tblCategory = await _dbContext.Categories.FindAsync(item.CategoryID);
                        var categorycode = tblCategory.Code;

                        var obj = await GetAllOwner(categoryid, item.KPILevelID);
                        var tbldata = await _dbContext.Datas
                          .Where(x => x.KPILevelCode == item.KPILevelCode && x.Period == "Y" && x.Yearly == year)
                          .OrderBy(x => x.Year)
                          .Select(x => new
                          {
                              ID = x.ID,
                              Value = x.Value,
                              Remark = x.Remark,
                              x.Target,
                              Year = x.Year,

                          }).ToListAsync();
                        dataremarks = tbldata
                          .Where(a => a.Value.ToDouble() > 0)
                         .Select(a => new Dataremark
                         {
                             ID = a.ID,
                             Value = a.Value,
                             Remark = a.Remark,
                             Week = a.Year,
                             ValueArray = new string[3] { a.Value, (a.Target.ToDouble() >= a.Value.ToDouble() ? false : true).ToString(), a.Target },
                         }).ToList();
                        if (start > 0 && end > 0)
                        {
                            dataremarks = dataremarks.Where(x => x.Week >= start && x.Week <= end).ToList();
                        }
                        var datasetsvm = new DatasetVM();
                        datasetsvm.KPIName = item.KPIName;
                        datasetsvm.Manager = obj.Manager;
                        datasetsvm.Owner = obj.Owner;
                        datasetsvm.Updater = obj.Updater;
                        datasetsvm.Sponsor = obj.Sponsor;
                        datasetsvm.Participant = obj.Participant;
                        datasetsvm.CategoryName = tblCategory.Name;
                        datasetsvm.CategoryCode = categorycode;
                        datasetsvm.KPILevelCode = item.KPILevelCode;

                        datasetsvm.Datasets = dataremarks;
                        datasetsvm.Period = "Yearly";

                        listDatasetVM.Add(datasetsvm);
                    }
                }
            }
            return listDatasetVM;
        }

        public async Task<List<ManagerOwnerUpdaterSponsorParticipantVM>> GetAllManagerOwnerUpdaterSponsorParticipant(int categoryID)
        {
            var managers = _dbContext.Managers;
            var owners = _dbContext.Owners;
            var updaters = _dbContext.Uploaders;
            var sponsors = _dbContext.Sponsors;
            var participant = _dbContext.Participants;
            var KPIs = _dbContext.KPIs;
            var users = _dbContext.Users;

            var data = await _dbContext.CategoryKPILevels.Where(x => x.CategoryID == categoryID)
                .Join(_dbContext.KPILevels,
                categoryKPILevel => categoryKPILevel.KPILevelID,
                kpilevel => kpilevel.ID,
                (categoryKPILevel, kpilevel) => new
                {
                    categoryKPILevel.KPILevelID,
                    categoryKPILevel.CategoryID,
                    kpilevel.KPIID,
                    kpilevel.KPILevelCode
                })
                 .Join(KPIs,
                categoryKPILevelKPI => categoryKPILevelKPI.KPIID,
                kpi => kpi.ID,
                (categoryKPILevelKPI, kpi) => new
                {
                    categoryKPILevelKPI.KPILevelID,
                    categoryKPILevelKPI.KPILevelCode,
                    categoryKPILevelKPI.CategoryID,
                    KPIName = kpi.Name
                })
                .Select(x => new
                {
                    x.CategoryID,
                    x.KPILevelID,
                    x.KPIName,
                    x.KPILevelCode
                }).ToListAsync();

            var model = data
                .Select(x => new ManagerOwnerUpdaterSponsorParticipantVM
                {
                    CategoryID = x.CategoryID,
                    KPILevelID = x.KPILevelID,
                    KPIName = x.KPIName,
                    KPILevelCode = x.KPILevelCode
                }).ToList();

            return model;
        }
        public async Task<Genaral> GetAllOwner(int categoryID, int kpilevelID)
        {

            var manager = await _dbContext.Managers
                        .Where(x => x.KPILevelID == kpilevelID && x.CategoryID == categoryID)
                        .Join(_dbContext.Users,
                        cat => cat.UserID,
                        user => user.ID,
                        (cat, user) => new
                        {
                            user.FullName
                        }).Select(x => x.FullName).ToArrayAsync();


            var owner = await _dbContext.Owners
                        .Where(x => x.KPILevelID == kpilevelID && x.CategoryID == categoryID)
                        .Join(_dbContext.Users,
                        cat => cat.UserID,
                        user => user.ID,
                        (cat, user) => new
                        {
                            user.FullName
                        }).Select(x => x.FullName).ToArrayAsync();
            var updater = await _dbContext.Uploaders
                         .Where(x => x.KPILevelID == kpilevelID && x.CategoryID == categoryID)
                        .Join(_dbContext.Users,
                        cat => cat.UserID,
                        user => user.ID,
                        (cat, user) => new
                        {
                            user.FullName
                        }).Select(x => x.FullName).ToArrayAsync();
            var sponsor = await _dbContext.Sponsors
                        .Where(x => x.KPILevelID == kpilevelID && x.CategoryID == categoryID)
                       .Join(_dbContext.Users,
                        cat => cat.UserID,
                        user => user.ID,
                        (cat, user) => new
                        {
                            user.FullName
                        }).Select(x => x.FullName).ToArrayAsync();
            var participant = await _dbContext.Participants
                        .Where(x => x.KPILevelID == kpilevelID && x.CategoryID == categoryID)
                       .Join(_dbContext.Users,
                        cat => cat.UserID,
                        user => user.ID,
                        (cat, user) => new
                        {
                            user.FullName
                        }).Select(x => x.FullName).ToArrayAsync();

            return new Genaral
            {
                Owner = owner.Count() != 0 ? string.Join(",", owner) : "N/A",
                Manager = manager.Count() != 0 ? string.Join(",", manager) : "N/A",
                Updater = updater.Count() != 0 ? string.Join(",", updater) : "N/A",
                Sponsor = sponsor.Count() != 0 ? string.Join(",", sponsor) : "N/A",
                Participant = participant.Count() != 0 ? string.Join(",", participant) : "N/A",
            };
        }
        /// <summary>
        /// Lấy dữ liệu cho chart js.
        /// </summary>
        /// <param name="kpiid"></param>
        /// <param name="kpilevel"></param>
        /// <param name="period"></param>
        /// <returns>Danh sách value theo thời gian</returns>
        public ChartVM ListDatas(string kpilevelcode, string period, int? year, int? start, int? end, int? catid)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();

            string url = string.Empty;
            var yearly = year ?? 0;
            var categoryid = catid ?? 0;

            if (!string.IsNullOrEmpty(kpilevelcode) && !string.IsNullOrEmpty(period))
            {
                //label chartjs
                var item = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);

                var PIC = Updaters(item.ID, categoryid);
                var Owner = Owners(item.ID, categoryid);
                var OwnerManagerment = Managers(item.ID, categoryid);
                var Sponsor = Sponsors(item.ID, categoryid);
                var Participant = Participants(item.ID, categoryid);

                var kpi = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID);
                var kpiname = string.Empty;
                if (kpi != null)
                    kpiname = kpi.Name;
                var label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name.ToSafetyString();
                //datasets chartjs
                var model = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Yearly == yearly);

                var unit = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID);
                if (unit == null) return new ChartVM();
                var unitName = _dbContext.Units.FirstOrDefault(x => x.ID == unit.Unit).Name.ToSafetyString();

                if (period == "W".ToUpper())
                {
                    var standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.WeeklyChecked == true).WeeklyStandard;
                    var statusFavourites = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true;

                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<string> listDatasets = new List<string>();

                    //labels của chartjs mặc định có 53 phần tử
                    List<string> listLabels = new List<string>();

                    //labels của chartjs mặc định có 53 phần tử
                    List<string> listTargets = new List<string>();

                    //labels của chartjs mặc định có 53 phần tử
                    List<int> listStandards = new List<int>();

                    var Dataremarks = new List<Dataremark>();
                    //Search range
                    if (start > 0 && end > 0)
                    {
                        model = model.Where(x => x.Yearly == year && x.Week >= start && x.Week <= end);

                        var listValues = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Value).ToArray();
                        var listLabelsW = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Week).ToArray();
                        var listtargetsW = model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Target).ToArray();
                        for (int i = 0; i < listValues.Length; i++)
                        {
                            listStandards.Add(standard);
                        }
                        //Convert sang list string
                        var listStringLabels = Array.ConvertAll(listLabelsW, x => x.ToSafetyString());

                        //Convert sang list string
                        var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());

                        listDatasets.AddRange(listValues);
                        listLabels.AddRange(listStringLabels);
                        listTargets.AddRange(listStringTargets);

                        Dataremarks = model
                           .Where(x => x.Period == "W")
                           .OrderBy(x => x.Week)
                           .Select(x => new Dataremark
                           {
                               ID = x.ID,
                               Value = x.Value,
                               Remark = x.Remark,
                               Week = x.Week
                           }).ToList();

                    }
                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = standard,
                        Dataremarks = Dataremarks,
                        datasets = listDatasets.ToArray(),
                        labels = listLabels.ToArray(),
                        label = label,
                        targets = listTargets.ToArray(),
                        standards = listStandards.ToArray(),
                        kpiname = kpiname,
                        period = "W",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = statusFavourites,
                        PIC = PIC,
                        Owner = Owner,
                        OwnerManagerment = OwnerManagerment,
                        Sponsor = Sponsor,
                        Participant = Participant
                    };

                }
                else if (period == "M".ToUpper())
                {
                    var standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.MonthlyChecked == true).MonthlyStandard;
                    var statusFavourites = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true;

                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<string> listDatasets = new List<string>();

                    //labels của chartjs mặc định có 12 phần tử = 0
                    List<string> listLabels = new List<string>();

                    //labels của chartjs mặc định có 12 phần tử
                    List<string> listTargets = new List<string>();
                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<int> listStandards = new List<int>();
                    var Dataremarks = new List<Dataremark>();


                    //Search range
                    if (start > 0 && end > 0)
                    {
                        model = model.Where(x => x.Yearly == year && x.Month >= start && x.Month <= end);

                        var listValues = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Value).ToArray();
                        var listLabelsW = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Month).ToArray();
                        var listtargetsW = model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Target).ToArray();
                        //Convert sang list string
                        var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());
                        listTargets.AddRange(listStringTargets);

                        for (int i = 0; i < listValues.Length; i++)
                        {
                            listStandards.Add(standard);
                        }
                        foreach (var a in listLabelsW)
                        {
                            switch (a)
                            {
                                case 1:
                                    listLabels.Add("Jan");
                                    break;
                                case 2:
                                    listLabels.Add("Feb"); break;
                                case 3:
                                    listLabels.Add("Mar"); break;
                                case 4:
                                    listLabels.Add("Apr"); break;
                                case 5:
                                    listLabels.Add("May");
                                    break;
                                case 6:
                                    listLabels.Add("Jun"); break;
                                case 7:
                                    listLabels.Add("Jul"); break;
                                case 8:
                                    listLabels.Add("Aug"); break;
                                case 9:
                                    listLabels.Add("Sep");
                                    break;
                                case 10:
                                    listLabels.Add("Oct"); break;
                                case 11:
                                    listLabels.Add("Nov"); break;
                                case 12:
                                    listLabels.Add("Dec"); break;
                            }
                        }

                        listDatasets.AddRange(listValues);

                        Dataremarks = model
                           .Where(x => x.Period == "M")
                           .OrderBy(x => x.Month)
                           .Select(x => new Dataremark
                           {
                               ID = x.ID,
                               Value = x.Value,
                               Remark = x.Remark,
                               Month = x.Month
                           }).ToList();
                    }

                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = standard,
                        Dataremarks = Dataremarks,
                        datasets = listDatasets.ToArray(),
                        labels = listLabels.ToArray(),
                        targets = listTargets.ToArray(),
                        standards = listStandards.ToArray(),
                        label = label,
                        kpiname = kpiname,
                        period = "M",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = statusFavourites,
                        PIC = PIC,
                        Owner = Owner,
                        OwnerManagerment = OwnerManagerment,
                        Sponsor = Sponsor,
                        Participant = Participant
                    };
                }
                else if (period == "Q".ToUpper())
                {
                    var standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.QuarterlyChecked == true).QuarterlyStandard;
                    var statusFavourites = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true;

                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<string> listDatasets = new List<string>();

                    //labels của chartjs mặc định có 53 phần tử = 0
                    List<string> listLabels = new List<string>();

                    //labels của chartjs mặc định có 12 phần tử
                    List<string> listTargets = new List<string>();
                    //labels của chartjs mặc định có 12 phần tử
                    List<int> listStandards = new List<int>();
                    var Dataremarks = new List<Dataremark>();


                    if (year > 0 && start > 0 && end > 0)
                    {
                        model = model.Where(x => x.Yearly == year && x.Quarter >= start && x.Quarter <= end);
                        var listValues = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Value).ToArray();
                        var listLabelsW = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Quarter).ToArray();
                        listDatasets.AddRange(listValues);
                        var listtargetsW = model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Target).ToArray();

                        //Convert sang list string
                        var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());
                        listTargets.AddRange(listStringTargets);
                        for (int i = 0; i < listValues.Length; i++)
                        {
                            listStandards.Add(standard);
                        }
                        foreach (var i in listLabelsW)
                        {
                            switch (i)
                            {
                                case 1:
                                    listLabels.Add("Quarter 1"); break;
                                case 2:
                                    listLabels.Add("Quarter 2"); break;
                                case 3:
                                    listLabels.Add("Quarter 3"); break;
                                case 4:
                                    listLabels.Add("Quarter 4"); break;
                            }
                        }
                        Dataremarks = model
                         .Where(x => x.Period == "Q")
                         .OrderBy(x => x.Quarter)
                         .Select(x => new Dataremark
                         {
                             ID = x.ID,
                             Value = x.Value,
                             Remark = x.Remark,
                             Quater = x.Quarter
                         }).ToList();
                    }

                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = standard,
                        Dataremarks = Dataremarks,
                        datasets = listDatasets.ToArray(),
                        labels = listLabels.ToArray(),
                        targets = listTargets.ToArray(),
                        standards = listStandards.ToArray(),
                        label = label,
                        kpiname = kpiname,
                        period = "Q",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = statusFavourites,
                        PIC = PIC,
                        Owner = Owner,
                        OwnerManagerment = OwnerManagerment,
                        Sponsor = Sponsor,
                        Participant = Participant
                    };
                }
                else if (period == "Y".ToUpper())
                {
                    if (start > 0 && end > 0)
                    {
                        model = model.Where(x => x.Year >= start && x.Year <= end);
                    }
                    var datasets = model.Where(x => x.Yearly == year && x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Value).ToArray();
                    var Dataremarks = model
                      .Where(x => x.Period == "Y")
                      .OrderBy(x => x.Year)
                      .Select(x => new Dataremark
                      {
                          ID = x.ID,
                          Value = x.Value,
                          Remark = x.Remark,
                          Year = x.Year
                      }).ToList();
                    //data: labels chartjs
                    var listlabels = model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Year).ToArray();
                    var labels = Array.ConvertAll(listlabels, x => x.ToSafetyString());
                    var listtargetsW = model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Target).ToArray();
                    //labels của chartjs mặc định có 12 phần tử
                    List<string> listTargets = new List<string>();
                    //Convert sang list string
                    var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());
                    listTargets.AddRange(listStringTargets);
                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.YearlyChecked == true).YearlyStandard,
                        Dataremarks = Dataremarks,
                        datasets = datasets,
                        labels = labels,
                        label = label,
                        targets = listTargets.ToArray(),
                        kpiname = kpiname,
                        period = "Y",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true,
                        PIC = PIC,
                        Owner = Owner,
                        OwnerManagerment = OwnerManagerment,
                        Sponsor = Sponsor,
                        Participant = Participant
                    };
                }
                else
                {
                    return new ChartVM();
                }
            }
            else
            {
                return new ChartVM();
            }
        }
        public async Task<ChartVM> ListDatas(string kpilevelcode, int? catid, string period, int? year, int? start, int? end)
        {
            var currentYear = DateTime.Now.Year;
            var currentWeek = DateTime.Now.GetIso8601WeekOfYear();
            var currentMonth = DateTime.Now.Month;
            var currentQuarter = DateTime.Now.GetQuarter();

            string url = string.Empty;
            var yearly = year ?? 0;
            var categoryid = catid ?? 0;

            if (!string.IsNullOrEmpty(kpilevelcode) && !string.IsNullOrEmpty(period))
            {
                //label chartjs
                var item = await _dbContext.KPILevels.FirstOrDefaultAsync(x => x.KPILevelCode == kpilevelcode);

                var PIC = Updaters(item.ID, categoryid);
                var Owner = Owners(item.ID, categoryid);
                var OwnerManagerment = Managers(item.ID, categoryid);
                var Sponsor = Sponsors(item.ID, categoryid);
                var Participant = Participants(item.ID, categoryid);

                var kpi = await _dbContext.KPIs.FirstOrDefaultAsync(x => x.ID == item.KPIID);
                var kpiname = string.Empty;
                if (kpi != null)
                    kpiname = kpi.Name;
                var label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name.ToSafetyString();
                //datasets chartjs
                var model = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Yearly == yearly);

                var unit = await _dbContext.KPIs.FirstOrDefaultAsync(x => x.ID == item.KPIID);
                if (unit == null) return new ChartVM();
                var unitName = _dbContext.Units.FirstOrDefault(x => x.ID == unit.Unit).Name.ToSafetyString();

                if (period == "W".ToUpper())
                {
                    var standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.WeeklyChecked == true).WeeklyStandard;
                    var statusFavourites = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true;

                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<string> listDatasets = new List<string>();

                    //labels của chartjs mặc định có 53 phần tử
                    List<string> listLabels = new List<string>();

                    //labels của chartjs mặc định có 53 phần tử
                    List<string> listTargets = new List<string>();

                    //labels của chartjs mặc định có 53 phần tử
                    List<int> listStandards = new List<int>();

                    var Dataremarks = new List<Dataremark>();
                    //Search range
                    if (start > 0 && end > 0)
                    {
                        model = model.Where(x => x.Yearly == year && x.Week >= start && x.Week <= end);

                        var listValues = await model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Value).ToArrayAsync();
                        var listLabelsW = await model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Week).ToArrayAsync();
                        var listtargetsW = await model.Where(x => x.Period == "W").OrderBy(x => x.Week).Select(x => x.Target).ToArrayAsync();
                        for (int i = 0; i < listValues.Length; i++)
                        {
                            listStandards.Add(standard);
                        }
                        //Convert sang list string
                        var listStringLabels = Array.ConvertAll(listLabelsW, x => x.ToSafetyString());

                        //Convert sang list string
                        var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());

                        listDatasets.AddRange(listValues);
                        listLabels.AddRange(listStringLabels);
                        listTargets.AddRange(listStringTargets);

                        Dataremarks = model
                           .Where(x => x.Period == "W")
                           .OrderBy(x => x.Week)
                           .Select(x => new Dataremark
                           {
                               ID = x.ID,
                               Value = x.Value,
                               Remark = x.Remark,
                               Week = x.Week,
                               Target =x.Target
                           }).ToList();

                    }
                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = standard,
                        Dataremarks = Dataremarks,
                        datasets = listDatasets.ToArray(),
                        labels = listLabels.ToArray(),
                        label = label,
                        targets = listTargets.ToArray(),
                        standards = listStandards.ToArray(),
                        kpiname = kpiname,
                        period = "W",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = statusFavourites,
                        PIC = PIC,
                        Owner = Owner,
                        OwnerManagerment = OwnerManagerment,
                        Sponsor = Sponsor,
                        Participant = Participant
                    };

                }
                else if (period == "M".ToUpper())
                {
                    var standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.MonthlyChecked == true).MonthlyStandard;
                    var statusFavourites = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true;

                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<string> listDatasets = new List<string>();

                    //labels của chartjs mặc định có 12 phần tử = 0
                    List<string> listLabels = new List<string>();

                    //labels của chartjs mặc định có 12 phần tử
                    List<string> listTargets = new List<string>();
                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<int> listStandards = new List<int>();
                    var Dataremarks = new List<Dataremark>();


                    //Search range
                    if (start > 0 && end > 0)
                    {
                        model = model.Where(x => x.Yearly == year && x.Month >= start && x.Month <= end);

                        var listValues = await model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Value).ToArrayAsync();
                        var listLabelsW = await model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Month).ToArrayAsync();
                        var listtargetsW = await model.Where(x => x.Period == "M").OrderBy(x => x.Month).Select(x => x.Target).ToArrayAsync();
                        //Convert sang list string
                        var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());
                        listTargets.AddRange(listStringTargets);

                        for (int i = 0; i < listValues.Length; i++)
                        {
                            listStandards.Add(standard);
                        }
                        foreach (var a in listLabelsW)
                        {
                            switch (a)
                            {
                                case 1:
                                    listLabels.Add("Jan");
                                    break;
                                case 2:
                                    listLabels.Add("Feb"); break;
                                case 3:
                                    listLabels.Add("Mar"); break;
                                case 4:
                                    listLabels.Add("Apr"); break;
                                case 5:
                                    listLabels.Add("May");
                                    break;
                                case 6:
                                    listLabels.Add("Jun"); break;
                                case 7:
                                    listLabels.Add("Jul"); break;
                                case 8:
                                    listLabels.Add("Aug"); break;
                                case 9:
                                    listLabels.Add("Sep");
                                    break;
                                case 10:
                                    listLabels.Add("Oct"); break;
                                case 11:
                                    listLabels.Add("Nov"); break;
                                case 12:
                                    listLabels.Add("Dec"); break;
                            }
                        }

                        listDatasets.AddRange(listValues);

                        Dataremarks = model
                           .Where(x => x.Period == "M")
                           .OrderBy(x => x.Month)
                           .Select(x => new Dataremark
                           {
                               ID = x.ID,
                               Value = x.Value,
                               Remark = x.Remark,
                               Month = x.Month,
                               Target = x.Target
                           }).ToList();
                    }

                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = standard,
                        Dataremarks = Dataremarks,
                        datasets = listDatasets.ToArray(),
                        labels = listLabels.ToArray(),
                        targets = listTargets.ToArray(),
                        standards = listStandards.ToArray(),
                        label = label,
                        kpiname = kpiname,
                        period = "M",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = statusFavourites,
                        PIC = PIC,
                        Owner = Owner,
                        OwnerManagerment = OwnerManagerment,
                        Sponsor = Sponsor,
                        Participant = Participant
                    };
                }
                else if (period == "Q".ToUpper())
                {
                    var standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.QuarterlyChecked == true).QuarterlyStandard;
                    var statusFavourites = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true;

                    //Tạo ra 1 mảng tuần mặc định bằng 0
                    List<string> listDatasets = new List<string>();

                    //labels của chartjs mặc định có 53 phần tử = 0
                    List<string> listLabels = new List<string>();

                    //labels của chartjs mặc định có 12 phần tử
                    List<string> listTargets = new List<string>();
                    //labels của chartjs mặc định có 12 phần tử
                    List<int> listStandards = new List<int>();
                    var Dataremarks = new List<Dataremark>();


                    if (year > 0 && start > 0 && end > 0)
                    {
                        model = model.Where(x => x.Yearly == year && x.Quarter >= start && x.Quarter <= end);
                        var listValues = await model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Value).ToArrayAsync();
                        var listLabelsW = await model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Quarter).ToArrayAsync();
                        listDatasets.AddRange(listValues);
                        var listtargetsW = await model.Where(x => x.Period == "Q").OrderBy(x => x.Quarter).Select(x => x.Target).ToArrayAsync();

                        //Convert sang list string
                        var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());
                        listTargets.AddRange(listStringTargets);
                        for (int i = 0; i < listValues.Length; i++)
                        {
                            listStandards.Add(standard);
                        }
                        foreach (var i in listLabelsW)
                        {
                            switch (i)
                            {
                                case 1:
                                    listLabels.Add("Quarter 1"); break;
                                case 2:
                                    listLabels.Add("Quarter 2"); break;
                                case 3:
                                    listLabels.Add("Quarter 3"); break;
                                case 4:
                                    listLabels.Add("Quarter 4"); break;
                            }
                        }
                        Dataremarks = model
                         .Where(x => x.Period == "Q")
                         .OrderBy(x => x.Quarter)
                         .Select(x => new Dataremark
                         {
                             ID = x.ID,
                             Value = x.Value,
                             Remark = x.Remark,
                             Quater = x.Quarter,
                             Target = x.Target
                         }).ToList();
                    }

                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = standard,
                        Dataremarks = Dataremarks,
                        datasets = listDatasets.ToArray(),
                        labels = listLabels.ToArray(),
                        targets = listTargets.ToArray(),
                        standards = listStandards.ToArray(),
                        label = label,
                        kpiname = kpiname,
                        period = "Q",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = statusFavourites,
                        PIC = PIC,
                        Owner = Owner,
                        OwnerManagerment = OwnerManagerment,
                        Sponsor = Sponsor,
                        Participant = Participant
                    };
                }
                else if (period == "Y".ToUpper())
                {
                    if (start > 0 && end > 0)
                    {
                        model = model.Where(x => x.Year >= start && x.Year <= end);
                    }
                    var datasets = await model.Where(x => x.Yearly == year && x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Value).ToArrayAsync();
                    var Dataremarks = model
                      .Where(x => x.Period == "Y")
                      .OrderBy(x => x.Year)
                      .Select(x => new Dataremark
                      {
                          ID = x.ID,
                          Value = x.Value,
                          Remark = x.Remark,
                          Year = x.Year,
                          Target = x.Target
                      }).ToList();
                    //data: labels chartjs
                    var listlabels = await model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Year).ToArrayAsync();
                    var labels = Array.ConvertAll(listlabels, x => x.ToSafetyString());
                    var listtargetsW = await model.Where(x => x.Period == "Y").OrderBy(x => x.Year).Select(x => x.Target).ToArrayAsync();
                    //labels của chartjs mặc định có 12 phần tử
                    List<string> listTargets = new List<string>();
                    //Convert sang list string
                    var listStringTargets = Array.ConvertAll(listtargetsW, x => x.ToSafetyString());
                    listTargets.AddRange(listStringTargets);
                    return new ChartVM
                    {
                        Unit = unitName,
                        Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.YearlyChecked == true).YearlyStandard,
                        Dataremarks = Dataremarks,
                        datasets = datasets,
                        labels = labels,
                        label = label,
                        targets = listTargets.ToArray(),
                        kpiname = kpiname,
                        period = "Y",
                        kpilevelcode = kpilevelcode,
                        statusfavorite = _dbContext.Favourites.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.Period == period) == null ? false : true,
                        PIC = PIC,
                        Owner = Owner,
                        OwnerManagerment = OwnerManagerment,
                        Sponsor = Sponsor,
                        Participant = Participant
                    };
                }
                else
                {
                    return new ChartVM();
                }
            }
            else
            {
                return new ChartVM();
            }
        }
        public ChartVM Compare(string kpilevelcode, string period)
        {

            var model2 = new DataCompareVM();

            var model = new ChartVM();

            var item = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);
            model.kpiname = _dbContext.KPIs.Find(item.KPIID).Name;
            model.label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name;
            model.kpilevelcode = kpilevelcode;

            var unit = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Unit;
            var unitName = _dbContext.Units.FirstOrDefault(x => x.ID == unit).Name;

            if (period == "W")
            {
                //Tạo ra 1 mảng tuần mặc định bằng 0
                List<string> listDatasets = new List<string>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> listLabels = new List<string>();

                var datas = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Week).Select(x => new { x.Value, x.Week }).ToList();
                foreach (var valueWeek in datas)
                {
                    listDatasets.Add(valueWeek.Value);
                    listLabels.Add(valueWeek.Week.ToString());
                }

                model.datasets = listDatasets.ToArray();
                model.labels = listLabels.ToArray();
                model.period = period;

            }
            if (period == "M")
            {
                //Tạo ra 1 mảng tuần mặc định bằng 0
                List<string> listDatasets = new List<string>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> listLabels = new List<string>();


                var datas = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Month).Select(x => new { x.Month, x.Value }).ToList();
                foreach (var monthly in datas)
                {
                    listDatasets.Add(monthly.Value);
                    switch (monthly.Month)
                    {
                        case 1:
                            listLabels.Add("Jan"); break;
                        case 2:
                            listLabels.Add("Feb"); break;
                        case 3:
                            listLabels.Add("Mar"); break;
                        case 4:
                            listLabels.Add("Apr"); break;
                        case 5:
                            listLabels.Add("May"); break;
                        case 6:
                            listLabels.Add("Jun"); break;
                        case 7:
                            listLabels.Add("Jul"); break;
                        case 8:
                            listLabels.Add("Aug"); break;
                        case 9:
                            listLabels.Add("Sep");
                            break;
                        case 10:
                            listLabels.Add("Oct"); break;
                        case 11:
                            listLabels.Add("Nov"); break;
                        case 12:
                            listLabels.Add("Dec"); break;
                    }
                }
                model.datasets = listDatasets.ToArray();
                model.labels = listLabels.ToArray();
                model.period = period;
            }
            if (period == "Q")
            {
                //Tạo ra 1 mảng tuần mặc định bằng 0
                List<string> listDatasets = new List<string>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> listLabels = new List<string>();
                var datas = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Quarter).Select(x => new { x.Quarter, x.Value }).ToList();
                foreach (var quarterly in datas)
                {
                    listDatasets.Add(quarterly.Value);
                    switch (quarterly.Quarter)
                    {
                        case 1:
                            listLabels.Add("Quarter 1"); break;
                        case 2:
                            listLabels.Add("Quarter 2"); break;
                        case 3:
                            listLabels.Add("Quarter 3"); break;
                        case 4:
                            listLabels.Add("Quarter 4"); break;
                    }
                }
                model.datasets = listDatasets.ToArray();
                model.labels = listLabels.ToArray();
                model.period = period;
                model.Unit = unitName;
                model.Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.QuarterlyChecked == true).QuarterlyStandard;
            }
            if (period == "Y")
            {
                var datasetsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Year).Select(x => x.Value).ToArray();
                var labelsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Year).Select(x => x.Year).ToArray();
                var labels1 = Array.ConvertAll(labelsKPILevel1, x => x.ToSafetyString());
                model.datasets = datasetsKPILevel1;
                model.labels = labels1;
                model.period = period;
            }
            return model;
        }

        public ChartVM2 Compare2(string kpilevelcode, string period)
        {

            var model2 = new DataCompareVM();

            var model = new ChartVM2();

            var item = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode);
            model.kpiname = _dbContext.KPIs.Find(item.KPIID).Name;
            model.label = _dbContext.Levels.FirstOrDefault(x => x.ID == item.LevelID).Name;
            model.kpilevelcode = kpilevelcode;

            var unit = _dbContext.KPIs.FirstOrDefault(x => x.ID == item.KPIID).Unit;
            var unitName = _dbContext.Units.FirstOrDefault(x => x.ID == unit).Name;

            if (period == "W")
            {
                //Tạo ra 1 mảng tuần mặc định bằng 0
                List<string> listDatasets = new List<string>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> listLabels = new List<string>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> targets = new List<string>();

                var datas = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Week).Select(x => new { x.Value, x.Week, x.Target }).ToList();
                foreach (var valueWeek in datas)
                {
                    listDatasets.Add(valueWeek.Value);
                    listLabels.Add(valueWeek.Week.ToString());
                    targets.Add(valueWeek.Target.ToString());
                }

                model.datasets = listDatasets.ToArray();
                model.labels = listLabels.ToArray();
                model.targets = targets.ToArray();
                model.period = period;

            }
            if (period == "M")
            {
                //Tạo ra 1 mảng tuần mặc định bằng 0
                List<string> listDatasets = new List<string>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> listLabels = new List<string>();
                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> targets = new List<string>();

                var datas = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Month).Select(x => new { x.Month, x.Value, x.Target }).ToList();
                foreach (var monthly in datas)
                {
                    listDatasets.Add(monthly.Value);
                    switch (monthly.Month)
                    {
                        case 1:
                            listLabels.Add("Jan"); break;
                        case 2:
                            listLabels.Add("Feb"); break;
                        case 3:
                            listLabels.Add("Mar"); break;
                        case 4:
                            listLabels.Add("Apr"); break;
                        case 5:
                            listLabels.Add("May"); break;
                        case 6:
                            listLabels.Add("Jun"); break;
                        case 7:
                            listLabels.Add("Jul"); break;
                        case 8:
                            listLabels.Add("Aug"); break;
                        case 9:
                            listLabels.Add("Sep");
                            break;
                        case 10:
                            listLabels.Add("Oct"); break;
                        case 11:
                            listLabels.Add("Nov"); break;
                        case 12:
                            listLabels.Add("Dec"); break;
                    }
                }
                model.datasets = listDatasets.ToArray();
                model.labels = listLabels.ToArray();
                model.period = period;
                model.targets = targets.ToArray();
            }
            if (period == "Q")
            {
                //Tạo ra 1 mảng tuần mặc định bằng 0
                List<string> listDatasets = new List<string>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> listLabels = new List<string>();

                //labels của chartjs mặc định có 53 phần tử = 0
                List<string> targets = new List<string>();
                var datas = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Quarter).Select(x => new { x.Quarter, x.Value, x.Target }).ToList();
                foreach (var quarterly in datas)
                {
                    listDatasets.Add(quarterly.Value);
                    switch (quarterly.Quarter)
                    {
                        case 1:
                            listLabels.Add("Quarter 1"); break;
                        case 2:
                            listLabels.Add("Quarter 2"); break;
                        case 3:
                            listLabels.Add("Quarter 3"); break;
                        case 4:
                            listLabels.Add("Quarter 4"); break;
                    }
                }
                model.datasets = listDatasets.ToArray();
                model.labels = listLabels.ToArray();
                model.period = period;
                model.Unit = unitName;
                model.targets = targets.ToArray();
                model.Standard = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == kpilevelcode && x.QuarterlyChecked == true).QuarterlyStandard;
            }
            if (period == "Y")
            {
                var datasetsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Year).Select(x => x.Value).ToArray();
                var labelsKPILevel1 = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Year).Select(x => x.Year).ToArray();
                var labels1 = Array.ConvertAll(labelsKPILevel1, x => x.ToSafetyString());
                var targets = _dbContext.Datas.Where(x => x.KPILevelCode == kpilevelcode && x.Period == period).OrderBy(x => x.Year).Select(x => x.Target).ToArray();

                model.datasets = datasetsKPILevel1;
                model.labels = labels1;
                model.period = period;
                model.targets = Array.ConvertAll(targets, x => x.ToSafetyString());
            }
            return model;
        }
        public async Task<object> Remark(int dataid)
        {
            var model = await _dbContext.Datas.FirstOrDefaultAsync(x => x.ID == dataid);
            return new
            {
                model = model,
                users = await _dbContext.Users.Where(x => x.Permission > 1).ToListAsync()
            };
        }

        public List<ChartVM2>  Compare2(string obj)
        {
            obj = obj.ToSafetyString();
            var listChartVM = new List<ChartVM2>();
            var value = obj.Split('-');

            var size = value.Length;
            foreach (var item in value)
            {
                var kpilevelcode = item.Split(',')[0];
                var period = item.Split(',')[1];
                listChartVM.Add(Compare2(kpilevelcode, period));
            }
            return listChartVM;
        }
        public DataCompareVM Compare(string obj)
        {
            var listChartVM = new List<ChartVM>();
            var model = new DataCompareVM();
            obj = obj.ToSafetyString();

            var value = obj.Split('-');
            model.Period = value[1].Split(',')[1];
            var size = value.Length;
            foreach (var item in value)
            {
                var kpilevelcode = item.Split(',')[0];
                var period = item.Split(',')[1];
                listChartVM.Add(Compare(kpilevelcode, period));
                model.list1 = Compare(kpilevelcode, period);
            }

            if (size == 2)
            {
                var kpilevelcode1 = value[0].Split(',')[0];
                var period1 = value[1].Split(',')[1];
                var kpilevelcode2 = value[1].Split(',')[0];
                var period2 = value[1].Split(',')[1];
                model.list1 = Compare(kpilevelcode1, period1);
                model.list2 = Compare(kpilevelcode2, period2);

                return model;
            }
            else if (size == 3)
            {
                var kpilevelcode1 = value[0].Split(',')[0];
                var period1 = value[1].Split(',')[1];

                var kpilevelcode2 = value[1].Split(',')[0];
                var period2 = value[1].Split(',')[1];

                var kpilevelcode3 = value[2].Split(',')[0];
                var period3 = value[2].Split(',')[1];
                model.list1 = Compare(kpilevelcode1, period1);
                model.list2 = Compare(kpilevelcode2, period2);
                model.list3 = Compare(kpilevelcode3, period3);
                return model;

            }
            else if (size == 4)
            {
                var kpilevelcode1 = value[0].Split(',')[0];
                var period1 = value[1].Split(',')[1];

                var kpilevelcode2 = value[1].Split(',')[0];
                var period2 = value[1].Split(',')[1];

                var kpilevelcode3 = value[2].Split(',')[0];
                var period3 = value[2].Split(',')[1];

                var kpilevelcode4 = value[3].Split(',')[0];
                var period4 = value[3].Split(',')[1];
                model.list1 = Compare(kpilevelcode1, period1);
                model.list2 = Compare(kpilevelcode2, period2);
                model.list3 = Compare(kpilevelcode3, period3);
                model.list4 = Compare(kpilevelcode4, period4);
                return model;
            }
            else
            {
                return new DataCompareVM();
            }
        }
        public async Task<bool> UpdateRemark(int dataid, string remark)
        {
            var model = await _dbContext.Datas.FirstOrDefaultAsync(x => x.ID == dataid);
            try
            {
                model.Remark = remark.ToSafetyString();
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<User> SearchUsers()
        {
            var listComment = _dbContext.Users.ToList();
            return listComment;
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
