using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.helpers;
namespace KPI.Model.DAO
{
    public class SettingDAO
    {
        KPIDbContext _dbContext = null;
        public SettingDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public async Task<bool> IsSendMail(string code)
        {
            try
            {
                var item = await _dbContext.Settings.FirstOrDefaultAsync(x => x.Code.Equals(code));
                return item.State;
            }
            catch (Exception)
            {
                throw;
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
