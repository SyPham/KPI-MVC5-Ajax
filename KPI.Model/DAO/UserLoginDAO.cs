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
    public class UserLoginDAO
    {
        KPIDbContext _dbContext = null;

        public UserLoginDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public async Task<UserProfileVM> GetUserProfile(string Username, string Password)
        {
            Password = Password.SHA256Hash();
            var userProfileVM = new UserProfileVM();
            var model = await (from u in _dbContext.Users
                               where u.Username == Username && u.Password == Password
                               select u)
                             .FirstOrDefaultAsync();
            userProfileVM.User = model;
            var menus = new UserAdminDAO().Sidebars(model.Permission, "en");
            if (menus == null)
                return userProfileVM;
            userProfileVM.Menus = menus;
            return userProfileVM;
        }

        public async Task<bool> CheckExistsUser(string userName, string passWord, bool isLoginAdmin = false)
        {
            passWord = passWord.SHA256Hash();
            return await _dbContext.Users.AnyAsync(x => x.Username == userName && x.Password == passWord);
        }

        public async Task<int> Login(string userName, string passWord, bool isLoginAdmin = false)
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginAdmin == true)
                {
                    if (result.Role == 1 || result.Role == 2)
                    {
                        if (result.IsActive == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (result.Password == passWord)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.IsActive == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (result.Password == passWord)
                            return 1;
                        else
                            return -2;
                    }
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
