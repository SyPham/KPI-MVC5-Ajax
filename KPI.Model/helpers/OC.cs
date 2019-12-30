using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.helpers
{
    public enum OC
    {
        [Getter(1, "GR")]
        GR = 1,
        [Getter(2, "DI")]
        DI = 2,
        [Getter(3, "FA")]
        FA = 3,
        [Getter(4, "CE")]
        CE = 4,
        [Getter(5, "DE")]
        DE = 5,
        [Getter(6, "BU")]
        BU = 6,
        [Getter(7, "TE")]
        TE = 7,
        [Getter(8, "CEll")]
        CEll = 8
    }
}
