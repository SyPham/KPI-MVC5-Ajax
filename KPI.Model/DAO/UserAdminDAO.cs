using KPI.Model.EF;
using KPI.Model.helpers;
using KPI.Model.ViewModel;
using KPI.Model.ViewModel.Menu;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
    public class UserAdminDAO
    {
        KPIDbContext _dbContext = null;

        public UserAdminDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public async Task<int> Add(EF.User entity)
        {
            entity.Code = entity.Code.ToSafetyString().ToUpper();
            List<EF.KPILevel> kpiLevelList = new List<EF.KPILevel>();

            try
            {
                entity.Password = entity.Password.SHA256Hash();
                entity.State = true;
                entity.IsActive = true;
                _dbContext.Users.Add(entity);
                await _dbContext.SaveChangesAsync();
                var kpiVM = await (from kpi in _dbContext.KPIs
                                   join cat in _dbContext.Categories on kpi.CategoryID equals cat.ID
                                   select new KPIViewModel
                                   {
                                       KPIID = kpi.ID,
                                   }).ToListAsync();
                foreach (var kpi in kpiVM)
                {
                    var kpilevel = new EF.KPILevel();
                    kpilevel.LevelID = entity.ID;
                    kpilevel.KPIID = kpi.KPIID;
                    kpiLevelList.Add(kpilevel);
                }

                _dbContext.KPILevels.AddRange(kpiLevelList);
                await _dbContext.SaveChangesAsync();

                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public List<MenuViewModel> Sidebars(int permisison, string langBy)
        {
            return _dbContext.Menus
                 .Where(x => x.Permission == permisison)
                 .Include(x => x.MenuLangs)
                 .Select(x => new MenuViewModel
                 {
                     ID = x.ID,
                     LangName = x.MenuLangs.FirstOrDefault(x => x.LangID == langBy).Name,
                     Link = x.Link,
                     FontAwesome = x.FontAwesome,
                     BackgroudColor = x.BackgroudColor,
                     Position = x.Position,
                     Permission = x.Permission
                 }).OrderBy(x => x.Position).ToList();

        }
        public object GetListAllPermissionsForUser(int userid)
        {
            var model = (from u in _dbContext.Users
                         join p in _dbContext.Permissions on u.Permission equals p.ID
                         join m in _dbContext.Menus on u.Permission equals m.Permission
                         where u.ID == userid
                         select new
                         {
                             UserID = u.ID,
                             PermissionID = p.ID,
                             MenuID = m.ID
                         }).AsEnumerable().ToList();
            return model;
        }
        public User GetByID(int id)
        {
            try
            {
                return _dbContext.Users.Find(id);

            }
            catch (Exception)
            {
                return new User();

            }
        }
        public async Task<object> GetListAllUser()
        {
            var model = await (from u in _dbContext.Users
                               where u.Permission > 1
                               select u).ToListAsync();
            return model;
        }
        public async Task<bool> Checkpermisson(int userid)
        {
            var model = await (_dbContext.Permissions.Join(
                _dbContext.Users,
               p => p.ID,
               u => u.Permission,
               (p, u) => new
               {
                   UserID = u.ID,
                   PermissionID = p.ID,

               })).Where(x => x.UserID == userid).FirstOrDefaultAsync();

            return model != null ? true : false;
        }
        public async Task<object> GetListAllPermissions(int userid)
        {
            var model = await _dbContext.Permissions.Select(x => new
            {
                x.ID,
                x.PermissionName,
                State = _dbContext.Users.FirstOrDefault(a => a.ID == userid && a.Permission == x.ID) != null ? true : false
            }).ToListAsync();
            return model;
        }
        public object GetAllMenusByPermissionID(int id)
        {
            return _dbContext.Menus.Where(x => x.Permission == id).Select(x => new
            {
                x.ID,
                x.Link,
                x.Name,
                x.Permission,
                State = _dbContext.Resources.FirstOrDefault(a => a.Menu == x.ID) != null ? true : false
            }).ToList();
        }
        public async Task<object> GetAllMenus()
        {
            return await _dbContext.Menus.Select(x => new
            {
                x.ID,
                x.Link,
                x.Name,
                x.Permission
            }).ToListAsync();
        }
        public object LoadDetailMenu(int id)
        {
            return _dbContext.Menus.Where(x => x.Permission == id).ToList();
        }

        public async Task<bool> Update(EF.User entity)
        {
            var code = entity.Code.ToSafetyString().ToUpper();
            var item = await _dbContext.Users.FirstOrDefaultAsync(x => x.ID == entity.ID);

            item.Username = entity.Username;
            item.Code = code;
            item.FullName = entity.FullName;
            item.Email = entity.Email;
            item.Skype = entity.Skype;
            item.Permission = entity.Permission;
            item.Alias = entity.Alias;
            try
            {
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

        public async Task<bool> LockUser(int id)
        {
            var item = await _dbContext.Users.FirstOrDefaultAsync(x => x.ID == id);

            item.IsActive = !item.IsActive;
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

        public async Task<bool> ChangePassword(string username, string newpass)
        {
            var item = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            try
            {
                var pass = newpass.ToSafetyString().SHA256Hash();
                item.Password = pass;
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
        public async Task<bool> AddUserToLevel(int id, int levelid)
        {
            var itemUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.ID == id);
            if (itemUser != null)
            {
                if (itemUser.LevelID == levelid)
                {
                    itemUser.LevelID = 0;
                }
                else
                {
                    itemUser.LevelID = levelid;
                }
            }


            try
            {
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
        public async Task<bool> Delete(int ID)
        {
            var findUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.ID == ID);
            try
            {
                _dbContext.Users.Remove(findUser);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }

        }
        public async Task<IEnumerable<EF.User>> GetAll()
        {
            return await _dbContext.Users.Where(x => x.State == true).ToListAsync();
        }
        public async Task<EF.User> GetbyID(int ID)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.ID == ID);
        }


        public object GetPaggedData(int pageNumber = 1, int pageSize = 20)
        {
            List<EF.User> listData = _dbContext.Users.ToList();
            var pagedData = Pagination.PagedResult(listData, pageNumber, pageSize);
            return pagedData;
        }
        public async Task<object> LoadData(string search, int page, int pageSize)
        {
            var model = await _dbContext.Users.Select(x => new
            {
                x.ID,
                x.Username,
                x.FullName,
                x.Code,
                PermissionName = _dbContext.Permissions.FirstOrDefault(a => a.ID == x.Permission).PermissionName,
                x.State,
                x.Email,
                x.Skype,
                x.Permission,
                x.Alias

            }).ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().ToSafetyString();
                model = model.Where(x => x.Code.Contains(search) || x.Username.Contains(search)).ToList();
            }

            int totalRow = model.Count();

            model = model.OrderBy(x => x.Permission)
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
        public async Task<object> LoadDataUser(string code, int page, int pageSize)
        {
            var model = await _dbContext.Users.Where(x => x.State == true).Select(x => new
            {
                x.Username,
                x.LevelID,
                x.Role,
                x.TeamID,
                FullName = x.Alias,
                Status = x.LevelID == 0 ? false : true
            }).ToListAsync();
            if (!string.IsNullOrEmpty(code))
            {
                model = model.Where(a => a.Username.Contains(code)).ToList();
            }
            int totalRow = model.Count();

            model = model.OrderBy(x => x.LevelID)
              .Skip((page - 1) * pageSize)
              .Take(pageSize).ToList();

            return new
            {
                data = model,
                total = totalRow,
                page,
                pageSize,
                status = true
            };
        }
        public int Total()
        {
            return _dbContext.Users.ToList().Count();
        }
        public async Task<object> LoadDataUser(int levelid, string code, int page, int pageSize)
        {
            var model = await _dbContext.Users.Where(x => x.State == true).Select(x => new
            {
                x.ID,
                x.Username,
                x.LevelID,
                x.Role,
                x.TeamID,
                FullName = x.Alias,
                Status = x.LevelID == levelid ? true : false
            }).ToListAsync();
            if (!string.IsNullOrEmpty(code))
            {
                model = model.Where(a => a.Username.Contains(code)).ToList();
            }
            int totalRow = model.Count();

            model = model.OrderBy(x => x.LevelID)
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
        public List<ViewModel.MenuVM> GetListMenuTree()
        {
            var listLevels = _dbContext.Menus.OrderBy(x => x.Name).ToList();
            var menus = new List<ViewModel.MenuVM>();
            foreach (var item in listLevels)
            {
                var menuItem = new ViewModel.MenuVM();
                menuItem.ID = item.ID;
                menuItem.Name = item.Name;
                menuItem.Link = item.Link;
                menuItem.FontAwesome = item.FontAwesome;
                menuItem.BackgroudColor = item.BackgroudColor;
                menuItem.ParentID = item.ParentID;
                menus.Add(menuItem);
            }

            List<ViewModel.MenuVM> hierarchy = new List<ViewModel.MenuVM>();

            hierarchy = menus.Where(c => c.ParentID == 0)
                            .Select(c => new ViewModel.MenuVM()
                            {
                                ID = c.ID,
                                Link = c.Link,
                                Name = c.Name,
                                BackgroudColor = c.BackgroudColor,
                                FontAwesome = c.FontAwesome,
                                children = GetChildren(menus, c.ID)
                            })
                            .ToList();


            HieararchyWalk(hierarchy);

            return hierarchy;
        }
        private List<ViewModel.MenuVM> GetChildren(List<ViewModel.MenuVM> menus, int parentid)
        {
            return menus
                    .Where(c => c.ParentID == parentid)
                    .Select(c => new ViewModel.MenuVM()
                    {
                        ID = c.ID,
                        Link = c.Link,
                        Name = c.Name,
                        BackgroudColor = c.BackgroudColor,
                        FontAwesome = c.FontAwesome,
                        children = GetChildren(menus, c.ID)
                    })
                    .ToList();
        }
        private void HieararchyWalk(List<ViewModel.MenuVM> hierarchy)
        {
            if (hierarchy != null)
            {
                foreach (var item in hierarchy)
                {
                    //Console.WriteLine(string.Format("{0} {1}", item.Id, item.Text));
                    HieararchyWalk(item.children);
                }
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
