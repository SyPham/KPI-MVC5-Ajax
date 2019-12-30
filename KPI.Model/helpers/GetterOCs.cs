using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KPI.Model.helpers
{
  public class GetterOCs
    {
        // Trả về Getter ứng với code.
        // (Phương thức này có thể trả về null).
        public static Getter? GetGetterByCode(string code)
        {

            // Lấy hết tất cả các phần tử của Enum.
            Array allGetters = Enum.GetValues(typeof(Getter));

            foreach (Getter getter in allGetters)
            {
                string c = GetCode(getter);
                if (c == code)
                {
                    return getter;
                }
            }
            return null;
        }

        public static int GetLevelNumber(Getter getter)
        {
            Getter getterAttr = GetAttr(getter);
            return getterAttr.LevelNumber;
        }

        public static string GetCode(Getter getter)
        {
            Getter getterAttr = GetAttr(getter);
            return getterAttr.Code;
        }

        private static Getter GetAttr(Getter getter)
        {
            MemberInfo memberInfo = GetMemberInfo(getter);
            return (Getter)Attribute.GetCustomAttribute(memberInfo, typeof(Getter));
        }

        private static MemberInfo GetMemberInfo(Getter getter)
        {
            MemberInfo memberInfo
                = typeof(Getter).GetField(Enum.GetName(typeof(Getter), getter));

            return memberInfo;
        }
    }
}
