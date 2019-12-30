using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KPI.Model.helpers;
using KPI.Model.EF;
using System.Data.Entity.SqlServer;
using System.Data.Entity;

namespace KPI.Model.DAO
{
    public class LevelDAO
    {
        private KPIDbContext _dbContext = null;
        public LevelDAO()
        {
            this._dbContext = new KPIDbContext();
        }
        public List<Level> Gets()
        {
            return _dbContext.Levels.ToList();
        }
        public async Task<bool> AddOrUpdate(Level entity)
        {
            entity.Code = entity.Code.ToUpper();
            List<EF.KPILevel> kpiLevelList = new List<KPILevel>();
            if (entity.ID == 0)
            {
                if (await _dbContext.Levels.FirstOrDefaultAsync(x => x.Code == entity.Code) != null)
                {
                    return false;
                }
                try
                {
                    var level = new Level()
                    {
                        Name = entity.Name,
                        Code = entity.Code,
                        LevelNumber = entity.LevelNumber,
                        ParentCode = entity.ParentCode,
                        ParentID = entity.ParentID
                    };
                    _dbContext.Levels.Add(level);

                    await _dbContext.SaveChangesAsync();

                    var kpiVM = await _dbContext.KPIs.ToListAsync();
                    kpiVM.ForEach(x =>
                    {
                        var kpilevel = new EF.KPILevel();
                        kpilevel.LevelID = level.ID;
                        kpilevel.KPIID = x.ID;
                        kpiLevelList.Add(kpilevel);
                    });
                    //foreach (var kpi in kpiVM)
                    //{
                    //    var kpilevel = new EF.KPILevel();
                    //    kpilevel.LevelID = level.ID;
                    //    kpilevel.KPIID = kpi.KPIID;
                    //    kpiLevelList.Add(kpilevel);
                    //}
                    _dbContext.KPILevels.AddRange(kpiLevelList);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return false;
                }
            }
            else
            {
                try
                {
                    var item = await _dbContext.Levels.FirstOrDefaultAsync(x => x.ID == entity.ID);
                    item.Code = entity.Code;
                    item.Name = entity.Name;
                    item.LevelNumber = entity.LevelNumber;
                    item.ParentID = entity.ParentID;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    return false;
                }
            }
        }

        public async Task<Level> GetByID(int id)
        {
            return await _dbContext.Levels.FirstOrDefaultAsync(x => x.ID == id);

        }

        public async Task<KPITreeViewModel> GetListTreeForWorkplace(int userid)
        {
            var levels = new List<KPITreeViewModel>();
            List<KPITreeViewModel> hierarchy = new List<KPITreeViewModel>();

            var listLevels = await _dbContext.Levels.OrderBy(x => x.LevelNumber).ToListAsync();

            var user = _dbContext.Users.FirstOrDefault(x => x.ID == userid);


            var levelNumber = await _dbContext.Levels.FirstOrDefaultAsync(x => x.ID == user.LevelID);

            if (levelNumber == null)
            {
                return new KPITreeViewModel();
            }

            listLevels = listLevels.Where(x => x.LevelNumber >= levelNumber.LevelNumber).OrderBy(x => x.LevelNumber).ToList();
            foreach (var item in listLevels)
            {
                var levelItem = new KPITreeViewModel();
                levelItem.key = item.ID;
                levelItem.title = item.Name;
                levelItem.code = item.Code;
                levelItem.state = item.State;
                levelItem.levelnumber = item.LevelNumber;
                levelItem.parentid = item.ParentID;
                levels.Add(levelItem);
            }
            var itemLevel = await _dbContext.Levels.FirstOrDefaultAsync(x => x.ID == user.LevelID);
            hierarchy = levels.Where(c => c.parentid == itemLevel.ParentID)
                       .Select(c => new KPITreeViewModel()
                       {
                           key = c.key,
                           title = c.title,
                           code = c.code,
                           levelnumber = c.levelnumber,
                           parentid = c.parentid,
                           children = GetChildren(levels, c.key)
                       })
                       .ToList();

            HieararchyWalk(hierarchy);
            var obj = new KPITreeViewModel();
            foreach (var item in hierarchy)
            {
                if (item.key == itemLevel.ID)
                {
                    obj = item;
                    break;
                }
            }
            var model = hierarchy.Where(x => x.key == itemLevel.ID).FirstOrDefault();
            return model;
        }
        public object GetListTreeClient(int userid)
        {
            var levels = new List<KPITreeViewModel>();
            List<KPITreeViewModel> hierarchy = new List<KPITreeViewModel>();

            var listLevels = _dbContext.Levels.OrderBy(x => x.LevelNumber).ToList();

            var user = _dbContext.Users.FirstOrDefault(x => x.ID == userid);

            if (user.Permission == 1)
            {
                listLevels = listLevels.OrderBy(x => x.LevelNumber).ToList();
                foreach (var item in listLevels)
                {
                    var levelItem = new KPITreeViewModel();
                    levelItem.key = item.ID;
                    levelItem.title = item.Name;
                    levelItem.code = item.Code;
                    levelItem.state = item.State;
                    levelItem.levelnumber = item.LevelNumber;
                    levelItem.parentid = item.ParentID;
                    levels.Add(levelItem);
                }

                hierarchy = levels.Where(c => c.parentid == 0)
                           .Select(c => new KPITreeViewModel()
                           {
                               key = c.key,
                               title = c.title,
                               code = c.code,
                               levelnumber = c.levelnumber,
                               parentid = c.parentid,
                               children = GetChildren(levels, c.key)
                           })
                           .ToList();

                HieararchyWalk(hierarchy);
                return hierarchy;
            }
            else
            {
                var levelNumber = _dbContext.Levels.FirstOrDefault(x => x.ID == user.LevelID);

                if (levelNumber == null)
                {
                    return new List<KPITreeViewModel>();
                }

                listLevels = listLevels.Where(x => x.LevelNumber >= levelNumber.LevelNumber).OrderBy(x => x.LevelNumber).ToList();
                foreach (var item in listLevels)
                {
                    var levelItem = new ViewModel.KPITreeViewModel();
                    levelItem.key = item.ID;
                    levelItem.title = item.Name;
                    levelItem.code = item.Code;
                    levelItem.state = item.State;
                    levelItem.levelnumber = item.LevelNumber;
                    levelItem.parentid = item.ParentID;
                    levels.Add(levelItem);
                }
                var itemLevel = _dbContext.Levels.FirstOrDefault(x => x.ID == user.LevelID);
                hierarchy = levels.Where(c => c.parentid == itemLevel.ParentID)
                           .Select(c => new ViewModel.KPITreeViewModel()
                           {
                               key = c.key,
                               title = c.title,
                               code = c.code,
                               levelnumber = c.levelnumber,
                               parentid = c.parentid,
                               children = GetChildren(levels, c.key)
                           })
                           .ToList();

                HieararchyWalk(hierarchy);
                var obj = new KPITreeViewModel();
                foreach (var item in hierarchy)
                {
                    if (item.key == itemLevel.ID)
                    {
                        obj = item;
                        break;
                    }
                }
                var model = hierarchy.Where(x => x.key == itemLevel.ID).ToList();
                return model;
            }


        }


        public async Task<List<KPITreeViewModel>> GetListTree()
        {
            var listLevels = await _dbContext.Levels.OrderBy(x => x.LevelNumber).ToListAsync();
            var levels = new List<KPITreeViewModel>();
            foreach (var item in listLevels)
            {
                var levelItem = new KPITreeViewModel();
                levelItem.key = item.ID;
                levelItem.title = item.Name;
                levelItem.code = item.Code;
                levelItem.state = item.State;
                levelItem.levelnumber = item.LevelNumber;
                levelItem.parentid = item.ParentID;
                levels.Add(levelItem);
            }

            List<KPITreeViewModel> hierarchy = new List<KPITreeViewModel>();

            hierarchy = levels.Where(c => c.parentid == 0)
                            .Select(c => new KPITreeViewModel()
                            {
                                key = c.key,
                                title = c.title,
                                code = c.code,
                                levelnumber = c.levelnumber,
                                parentid = c.parentid,
                                children = GetChildren(levels, c.key)
                            })
                            .ToList();


            HieararchyWalk(hierarchy);

            return hierarchy;
        }
        //public async IEnumerable<int> Root(int id)
        //{
        //    var model =await _dbContext.Levels.FirstOrDefaultAsync(x => x.ID == id);
        //    if (model.ParentID.Value != 0)
        //        yield return await GetNode(model.ParentID.Value);

        //}

        public string GetNode(int id)
        {
            var list = new List<Level>();
            list = _dbContext.Levels.ToList();
            var list2 = new List<Level>();
            list2.Add(list.FirstOrDefault(x => x.ID == id));
            var parentID = list.FirstOrDefault(x => x.ID == id).ParentID.Value;
            foreach (var item in list)
            {
                if (parentID == 0)
                    break;
                if (parentID != 0)
                {
                    //add vao list1
                    list2.Add(list.FirstOrDefault(x => x.ID == parentID));
                }
                //cap nhat lai parentID
                parentID = list.FirstOrDefault(x => x.ID == parentID).ParentID.Value;

            }
            return string.Join("->", list2.OrderBy(x => x.ParentID).Select(x => x.Name).ToArray());
        }
        public string GetNode(string code)
        {
            try
            {
                var id = _dbContext.KPILevels.FirstOrDefault(x => x.KPILevelCode == code).LevelID;
                var list = new List<Level>();
                list = _dbContext.Levels.ToList();
                var list2 = new List<Level>();
                list2.Add(list.FirstOrDefault(x => x.ID == id));
                var parentID = list.FirstOrDefault(x => x.ID == id).ParentID.Value;
                foreach (var item in list)
                {
                    if (parentID == 0)
                        break;
                    if (parentID != 0)
                    {
                        //add vao list1
                        list2.Add(list.FirstOrDefault(x => x.ID == parentID));
                    }
                    //cap nhat lai parentID
                    parentID = list.FirstOrDefault(x => x.ID == parentID).ParentID.Value;

                }
                return string.Join("->", list2.OrderBy(x => x.ParentID).Select(x => x.Name).ToArray());
            }
            catch (Exception)
            {

                return "#N/A";
            }
            
        }
        public List<KPITreeViewModel> GetChildren(List<KPITreeViewModel> levels, int parentid)
        {
            return levels
                    .Where(c => c.parentid == parentid)
                    .Select(c => new KPITreeViewModel()
                    {
                        key = c.key,
                        title = c.title,
                        code = c.code,
                        levelnumber = c.levelnumber,
                        parentid = c.parentid,
                        children = GetChildren(levels, c.key)
                    })
                    .ToList();
        }
        public int Total()
        {
            return _dbContext.Levels.ToList().Count();
        }
        public IEnumerable<Level> GetItemFromLevel(int parentID)
        {
            foreach (var item in _dbContext.Levels.ToList())
            {
                if (item.ParentID == parentID)
                    yield return item;
            }
        }
        public async Task<bool> Remove(int key)
        {
            var item = await _dbContext.Levels.FindAsync(key);
            List<int> lsoc = new List<int>();
            try
            {
                _dbContext.Levels.RemoveRange(GetItemFromLevel(item.ID));
                _dbContext.Levels.Remove(item);

                foreach (var oc in GetItemFromLevel(item.ID))
                {
                    lsoc.Add(oc.ID);
                }
                var listkpilevel2 = await _dbContext.KPILevels.Where(x => x.LevelID == item.ID).ToListAsync();
                var listkpilevel = await _dbContext.KPILevels.Where(x => lsoc.Contains(x.LevelID)).ToListAsync();
                _dbContext.KPILevels.RemoveRange(listkpilevel2);
                _dbContext.KPILevels.RemoveRange(listkpilevel);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;

            }
        }

        public async Task<bool> IsExistsCode(string code)
        {
            return await _dbContext.Levels.AnyAsync(x => x.Code == code);
        }

        public async Task<string> GenarateCode(string code)
        {
            var value = code;
            for (int i = 1; i < 9999; i++)
            {
                if (!await IsExistsCode(value))
                {
                    value += i.ToString("D4");
                    break;
                }
            }
            return value;
        }

        public async Task<string> CheckLevelNumberAndGenarateCode(int levelNumber)
        {
            var code = string.Empty;
            switch (levelNumber)
            {
                case (int)OC.GR:
                    code = OC.GR.ToString();
                    code = await GenarateCode(code);
                    break;
                case (int)OC.DI:
                    code = OC.DI.ToString();
                    code = await GenarateCode(code);
                    break;
                case (int)OC.FA:
                    code = OC.FA.ToString();
                    code = await GenarateCode(code);
                    break;
                case (int)OC.CE:
                    code = OC.CE.ToString();
                    code = await GenarateCode(code);
                    break;
                case (int)OC.DE:
                    code = OC.DE.ToString();
                    code = await GenarateCode(code);
                    break;
                case (int)OC.BU:
                    code = OC.BU.ToString();
                    code = await GenarateCode(code);
                    break;
                case (int)OC.TE:
                    code = OC.TE.ToString();
                    code = await GenarateCode(code);
                    break;
                case (int)OC.CEll:
                    code = OC.CEll.ToString();
                    code = await GenarateCode(code);
                    break;
                default:
                    break;
            }
            return code;
        }
        public async Task<bool> Add(Level level)
        {
            List<EF.KPILevel> kpiLevelList = new List<EF.KPILevel>();
            if (level.LevelNumber == 0)
                return false;
            try
            {
                level.Code = await CheckLevelNumberAndGenarateCode(level.LevelNumber ?? 0);
                _dbContext.Levels.Add(level);
                await _dbContext.SaveChangesAsync();

                var kpiVM = await _dbContext.KPIs.ToListAsync();
                kpiVM.ForEach(x =>
                {
                    var kpilevel = new EF.KPILevel();
                    kpilevel.LevelID = level.ID;
                    kpilevel.KPIID = x.ID;
                    kpiLevelList.Add(kpilevel);
                });
                _dbContext.KPILevels.AddRange(kpiLevelList);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Rename(LevelActionVM level)
        {
            var item = await _dbContext.Levels.FindAsync(level.key);
            item.Name = level.title;
            item.Code = level.code;
            var kpilevels = await _dbContext.KPILevels.Where(x => x.KPILevelCode.Contains(item.Code)).ToListAsync();
            kpilevels.ForEach(x => x.KPILevelCode = x.KPILevelCode.Replace(item.Code, level.code));

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
        private void HieararchyWalk(List<ViewModel.KPITreeViewModel> hierarchy)
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
        public bool Update(int key, string title, string code)
        {
            var item = _dbContext.Levels.FirstOrDefault(x => x.ID == key);

            if (item == null)
                return false;
            else
            {
                item.Code = code;
                item.Name = title;
                try
                {
                    _dbContext.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
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
