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
    public class CategoryKPILevelDAO
    {
        private KPIDbContext _dbContext = null;

        public CategoryKPILevelDAO()
        {
            _dbContext = new KPIDbContext();
        }

        public bool CheckExistsData(string code, string period)
        {
            //Kiem tra period hien tai trong bang data
            var currenMonth = DateTime.Now.Month;
            var current = _dbContext.Datas
                                            .FirstOrDefault(x => x.KPILevelCode == code
                                            && x.Period == period
                                            && x.Month == currenMonth);
            if (current == null)
                return false;
            if (current.Value == null || current.Value == "" || current.Value == "0")
                return true;
            return false;

        }
        public object LoadKPILevel(int categoryID, int page, int pageSize = 3)
        {
            //Lấy các tuần tháng quý năm hiện tại
            var weekofyear = DateTime.Now.GetIso8601WeekOfYear();
            var monthofyear = DateTime.Now.Month;
            var quarterofyear = DateTime.Now.GetQuarterOfYear();
            var year = DateTime.Now.Year;
            var currentweekday = DateTime.Now.DayOfWeek.ToSafetyString().ToUpper().ConvertStringDayOfWeekToNumber();
            var currentdate = DateTime.Today.Add(new TimeSpan(00, 00, 00, 00));
            var dt = new DateTime(2019, 8, 1);
            var value = DateTime.Compare(currentdate, dt);
            try
            {
                var categoryKPILevels = _dbContext.CategoryKPILevels;
                //Lấy ra danh sách data từ trong db
                var datas = _dbContext.Datas;
                var model = from cat in _dbContext.CategoryKPILevels.Where(x => x.Status == true && x.CategoryID == categoryID)
                            join kpiLevel in _dbContext.KPILevels on cat.KPILevelID equals kpiLevel.ID
                            join kpi in _dbContext.KPIs on kpiLevel.KPIID equals kpi.ID
                            where kpiLevel.Checked == true
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
                                StatusUploadDataW = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.WeeklyChecked == true ? "W" : "")) == null ? false : kpiLevel.Weekly == null ? false : kpiLevel.Weekly < currentweekday ? true : false,
                                StatusUploadDataM = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.MonthlyChecked == true ? "M" : "")) == null ? false : kpiLevel.Monthly == null ? false : DateTime.Compare(currentdate, kpiLevel.Monthly ?? DateTime.MinValue) < 0 ? true : false,
                                StatusUploadDataQ = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.QuarterlyChecked == true ? "Q" : "")) == null ? false : kpiLevel.Quarterly == null ? false : DateTime.Compare(currentdate, kpiLevel.Quarterly ?? DateTime.MinValue) < 0 ? true : false,
                                StatusUploadDataY = datas.FirstOrDefault(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == (kpiLevel.YearlyChecked == true ? "Y" : "")) == null ? false : kpiLevel.Yearly == null ? false : DateTime.Compare(currentdate, kpiLevel.Yearly ?? DateTime.MinValue) < 0 ? true : false,

                                //StatusUploadDataW = datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == weekofyear).Value != "0" || datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == weekofyear).Value != null || datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == weekofyear).Value != "" ? false : true,
                                //StatusUploadDataM = datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "M" && a.Month == monthofyear).Value != "0" || datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == monthofyear).Value != null || datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == monthofyear).Value != "" ? false : true,
                                //StatusUploadDataQ = datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Q" && a.Week == quarterofyear).Value != "0" || datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == quarterofyear).Value != null || datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == quarterofyear).Value != "" ? false : true,
                                //StatusUploadDataY = datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Y" && a.Week == year).Value != "0" || datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == year).Value != null || datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == year).Value != "" ? false : true,
                                //dataW = datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == "W").Select(x=>x.Value).ToList(),
                                //dataM = datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period =="M").Select(x => x.Value).ToList(),
                                //dataQ = datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == "Q").Select(x => x.Value).ToList(),
                                //dataY = datas.Where(x => x.KPILevelCode == kpiLevel.KPILevelCode && x.Period == "Y").Select(x => x.Value).ToList(),



                                //StatusUploadDataW = datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == weekofyear).Value != "0" || datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == weekofyear).Value != null || datas.FirstOrDefault(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "W" && a.Week == weekofyear).Value != "" ?  true : false ,

                                //StatusUploadDataM = monthofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "M").Max(x => x.Month) > 1 ? false : monthofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "M").Max(x => x.Month) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Monthly.Value) < 0 ? true : false) : false,

                                //StatusUploadDataQ = quarterofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Q").Max(x => x.Quarter) > 1 ? false : quarterofyear - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Q").Max(x => x.Quarter) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Quarterly.Value) < 0 ? true : false) : false, //true dung han flase tre han

                                //StatusUploadDataY = year - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Y").Max(x => x.Year) > 1 ? false : year - datas.Where(a => a.KPILevelCode == kpiLevel.KPILevelCode && a.Period == "Y").Max(x => x.Year) == 1 ? (DateTime.Compare(currentdate, kpiLevel.Yearly.Value) < 0 ? true : false) : false,
                                //CheckCategory = categoryKPILevels.FirstOrDefault(a => a.CategoryID == categoryID && a.KPILevelID == kpiLevel.ID) != null ? categoryKPILevels.FirstOrDefault(a => a.CategoryID == categoryID && a.KPILevelID == kpiLevel.ID).Status : false
                                CheckCategory = cat.Status
                            };



                int totalRow = model.Count();

                model = model.Where(x => x.CheckCategory == true).OrderByDescending(x => x.CreateTime)
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
