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
    public class KPIAdminDAO
    {
        KPIDbContext _dbContext = null;

        public KPIAdminDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public async Task<bool> Add(EF.KPI entity)
        {
            try
            {
                for (int i = 1; i < 10000; i++)
                {
                    string code = i.ToString("D4");
                    if (await _dbContext.KPIs.FirstOrDefaultAsync(x => x.Code == code) == null)
                    {
                        entity.Code = code;
                        break;
                    }
                }

                _dbContext.KPIs.Add(entity);
              await  _dbContext.SaveChangesAsync();

                List<EF.KPILevel> kpiLevelList = new List<EF.KPILevel>();
                var levels = _dbContext.Levels.ToList();

                foreach (var level in levels)
                {
                    var kpilevel = new EF.KPILevel();
                    kpilevel.LevelID = level.ID;
                    kpilevel.KPIID = entity.ID;
                    kpiLevelList.Add(kpilevel);
                }

                _dbContext.KPILevels.AddRange(kpiLevelList);
                _dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> AddKPILevel(EF.KPILevel entity)
        {
            _dbContext.KPILevels.Add(entity);
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
        public int Total()
        {
            return _dbContext.KPIs.ToList().Count();
        }
        public async Task<bool> Update(EF.KPI entity)
        {
            entity.Code = entity.Code.ToSafetyString().ToUpper();
            try
            {
                var iteam =await _dbContext.KPIs.FirstOrDefaultAsync(x => x.ID == entity.ID);
                iteam.Name = entity.Name;
                //iteam.Code = entity.Code;
                iteam.LevelID = entity.LevelID;
                iteam.CategoryID = entity.CategoryID;
                iteam.Unit = entity.Unit;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //logging
                return false;
            }

        }
        public List<EF.Category> GetCategoryCode()
        {
            return _dbContext.Categories.ToList();
        }
        public async Task<bool> Delete(int id)
        {

            try
            {
                var kpi =await _dbContext.KPIs.FindAsync(id);
                _dbContext.KPIs.Remove(kpi);

                var kpiLevel =await _dbContext.KPILevels.Where(x=>x.KPIID==id).ToListAsync();
                _dbContext.KPILevels.RemoveRange(kpiLevel);
               await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }
        public async Task<object> GetAll()
        {
            return await _dbContext.KPIs.Select(x=>new {
                x.ID,
                x.Code,
                x.Name,
                x.LevelID,
                CategoryName = _dbContext.Categories.FirstOrDefault(a=>a.ID==x.CategoryID),
                Unit = _dbContext.Units.FirstOrDefault(u=>u.ID==x.Unit)

            }).ToListAsync();
        }
        public async Task<EF.KPI>GetbyID(int ID)
        {
            return await _dbContext.KPIs.FirstOrDefaultAsync(x => x.ID == ID);
        }
        public async Task<List<Unit>> GetAllUnit()
        {
            return await _dbContext.Units.ToListAsync();
        }
        public async Task<object> ListCategory()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<object> LoadData(int? categoryID, string name, int page, int pageSize = 3)
        {
            categoryID = categoryID.ToInt();
            name = name.ToSafetyString();
            var model =await _dbContext.KPIs.Select(
                x => new KPIVM
                {
                    ID = x.ID,
                    Name = x.Name,
                    Code = x.Code,
                    LevelID = x.LevelID,
                    CategoryID = x.CategoryID,
                    CategoryName = _dbContext.Categories.FirstOrDefault(c => c.ID == x.CategoryID).Name,
                    Unit = _dbContext.Units.FirstOrDefault(u => u.ID == x.Unit).Name,
                    CreateTime = x.CreateTime
                }
                ).ToListAsync();
            if (!string.IsNullOrEmpty(name))
            {
                model = model.Where(x => x.Name.Contains(name)).ToList();
            }

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

        public async Task<object> Autocomplete(string search)
        {
            if (search != "")
                return await _dbContext.KPIs.Where(x => x.Name.Contains(search)).Select(x => x.Name).Take(5).ToListAsync();
            else
                return "";
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
