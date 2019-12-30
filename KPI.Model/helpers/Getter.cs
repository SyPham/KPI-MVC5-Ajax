using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.helpers
{
   public class Getter: Attribute
    {
        // code: M, text = Male
        // code: F, text = Female
        internal Getter(int levelNumber, string code)
        {
            this.Code = code;
            this.LevelNumber = levelNumber;
        }

        public int LevelNumber { get; private set; }

        public string Code { get; private set; }

    }
}
