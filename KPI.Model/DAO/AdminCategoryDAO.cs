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
    public class AdminCategoryDAO
    {
        KPIDbContext _dbContext = null;

        public AdminCategoryDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public async Task<bool> Add(EF.Category entity)
        {
            entity.Code = entity.Code.ToUpper();

            try
            {
                _dbContext.Categories.Add(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }

        public async Task<int> Total()
        {
            return await _dbContext.Categories.CountAsync();
        }
        public async Task<bool> Update(EF.Category entity)
        {
            entity.Code = entity.Code.ToUpper();
            try
            {
                var iteam = await _dbContext.Categories.FirstOrDefaultAsync(x => x.ID == entity.ID);
                iteam.Name = entity.Name;
                iteam.Code = entity.Code;
                iteam.LevelID = entity.LevelID;
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
                var category = await _dbContext.Categories.FindAsync(id);
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }
        public async Task<IEnumerable<EF.Category>> GetAll()
        {
            return await _dbContext.Categories.ToListAsync();
        }
        public async Task<EF.Category> GetbyID(int ID)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(x => x.ID == ID);
        }
        public async Task<object> ListCategory()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<object> LoadData(string name, int page, int pageSize = 3)
        {
            name = name.ToSafetyString();
            var model = await _dbContext.Categories.ToListAsync();
            if (!string.IsNullOrEmpty(name))
            {
                model = model.Where(x => x.Name.Contains(name)).ToList();
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
        public async Task<object> GetAllCategory(int page, int pageSize, int level, int ocID)
        {

            var ocCategories = _dbContext.OCCategories;
            var model = await _dbContext.Categories
                .Select(x => new
                {
                    x.Name,
                    x.ID,
                    x.LevelID,
                    x.CreateTime,
                    Total = _dbContext.CategoryKPILevels.Join(_dbContext.KPILevels,
                                cat => cat.KPILevelID,
                                kpil => kpil.ID,
                                (cat,kpil) =>new { cat.CategoryID,cat.Status,kpil.Checked}
                            ).Where(a => a.CategoryID == x.ID && a.Status == true && a.Checked == true).Count(),
                    Status = ocCategories.FirstOrDefault(a => a.CategoryID == x.ID && a.OCID == ocID) == null ? false : ocCategories.FirstOrDefault(a => a.CategoryID == x.ID && a.OCID == ocID).Status
                }).Where(x => x.Status == true && x.Total > 0).ToListAsync();

            if (level > 0)
            {
                model = model.Where(x => x.LevelID == level).ToList();
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

            //var ocCategories = _dbContext.OCCategories;
            //var model = (await _dbContext.Categories
            //    .Select(x => new
            //    {
            //        x.Name,
            //        x.ID,
            //        x.LevelID,
            //        x.CreateTime,
            //        Total = _dbContext.CategoryKPILevels.Where(a => a.CategoryID == x.ID && a.Status == true).Count(),
            //        Status = ocCategories.FirstOrDefault(a => a.CategoryID == x.ID && a.OCID == ocID) == null ? false : ocCategories.FirstOrDefault(a => a.CategoryID == x.ID && a.OCID == ocID).Status
            //    }).ToListAsync());
            //if (level > 0)
            //{
            //    model = model.Where(x => x.LevelID == level && x.Status == true && x.Total > 0).ToList();
            //}
            //int totalRow = model.Count();

            //model = model.OrderByDescending(x => x.CreateTime)

            //  .Skip((page - 1) * pageSize)
            //  .Take(pageSize).ToList();
            //return new
            //{
            //    data = model,
            //    total = totalRow,
            //    status = true,
            //    page,
            //    pageSize
            //};
        }
        public async Task<object> GetAll(int page, int pageSize, int level)
        {
            var ocategories = _dbContext.OCCategories;
            var model = (await _dbContext.Categories
                .Select(x => new
                {
                    x.Name,
                    x.ID,
                    x.LevelID,
                    Total = _dbContext.CategoryKPILevels.Where(a => a.CategoryID == x.ID && a.Status == true).Count(),
                    x.CreateTime,
                    Status = ocategories.FirstOrDefault(a => a.CategoryID == x.ID) != null ? ocategories.FirstOrDefault(a => a.CategoryID == x.ID).Status == true : false
                }).ToListAsync()).Where(x => x.LevelID == level && x.Status == true && x.Total > 0);
            if (level > 0)
            {
                model = model.Where(x => x.LevelID == level && x.Total > 0).ToList();
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
                return await _dbContext.Categories.Where(x => x.Name.Contains(search)).Select(x => x.Name).Take(5).ToListAsync();
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
