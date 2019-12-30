using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI.Web.Extension
{
    public interface IExtension<T>
    {
        string GetNode(List<T> list);
    }
}