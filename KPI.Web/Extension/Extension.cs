using KPI.Model.EF;
using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI.Web.Extension
{
    public static class Extension
    {
        public static IEnumerable<string> GetNode(this List<Level> list, int nodeChild)
        {
            var id = list.First(x => x.ID == nodeChild).ParentID ?? 0;
            foreach (var item in list)
            {
                yield return item.ID == id ? item.Name : "";
            }

        }
        
    }
}