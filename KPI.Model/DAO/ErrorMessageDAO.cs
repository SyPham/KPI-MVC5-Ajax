using KPI.Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.DAO
{
   public class ErrorMessageDAO
    {
        KPIDbContext _dbContext = null;
        public ErrorMessageDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public long Add(ErrorMessage error)
        {
            var item = new ErrorMessage();
            _dbContext.ErrorMessages.Add(error);
            _dbContext.SaveChanges();
            return error.ID;

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
