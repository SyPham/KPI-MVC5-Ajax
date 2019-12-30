using KPI.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KPI.Model.helpers
{
    public static class CodeUtility
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
   (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public static string ToStringDateTime(this DateTime? dt, string format)
        => dt == null ? "n/a" : ((DateTime)dt).ToString(format);
        public static bool IsDouble(this object value)
        {
            var flag = false;
            if (value == null || value.ToString() == string.Empty)

                return flag;

            double result = 0;

            double.TryParse(value.ToString(), out result);

            return false;
        }

       

        /// <summary>
        /// Cắt chuỗi bắt đầu từ bên trái
        /// </summary>
        /// <param name="value">Giá trị</param>
        /// <param name="length">Chiều dài cần cắt</param>
        /// <returns></returns>
        public static string ToLeft(this string value,int length)
        {
            string somestring = value;
            if(somestring.IsNullOrEmpty())
            {
                return string.Empty;
            }
            if(length > somestring.Length)
            {
                return string.Empty;
            }

            string newstring = somestring.Substring(0, length);
            return newstring;
        }
        /// <summary>
        /// Cắt chuỗi bắt đầu từ bên phải
        /// </summary>
        /// <param name="value">Giá trị</param>
        /// <param name="length">Chiều dài chuỗi</param>
        /// <returns></returns>
        public static string ToRight(this string value, int length)
        {
            string somestring = value;
            var stringLength = somestring.Length;

            if (somestring.IsNullOrEmpty())
            {
                return string.Empty;
            }
            if (length > stringLength)
            {
                return string.Empty;
            }
            string newstring = somestring.Substring(stringLength - length, length);
            return newstring;
        }
        /// <summary>
        /// Cắt chuỗi theo khoảng
        /// </summary>
        /// <param name="value">Giá trị</param>
        /// <param name="From">Là giá trị bắt đầu (thuộc kiểu int)</param>
        /// <param name="End">Là giá trị kết thúc (thuộc kiểu int)</param>
        /// <returns></returns>
        public static string ToMid(this string value, int From, int End)
        {
            string somestring = value;
            if (somestring.IsNullOrEmpty())
            {
                return string.Empty;
            }
            if (From > End || From > somestring.Length || End > somestring.Length)
            {
                return string.Empty;
            }
            string newstring = somestring.Substring(From, End);
            return newstring;
        }
        public static DateTime ToGetMondayOfWeek(this int year, int week)
        {
            if (week == 0)
                week = 1;
            // 4 January is always in week 1 (see http://en.wikipedia.org/wiki/ISO_week_date)
            DateTime jan4 = new DateTime(year, 1, 4);

            // get a day in the requested week
            DateTime day = jan4.AddDays((week - 1) * 7);

            // get day of week, with [mon = 0 ... sun = 6] instead of [sun = 0 ... sat = 6]
            int dayOfWeek = ((int)day.DayOfWeek + 6) % 7;

            return day.AddDays(-dayOfWeek);
        }
        public static DateTime ToGetSaturdayOfWeek(int year, int week)
        {
            if (week == 0)
                week = 1;
            // 4 January is always in week 1 (see http://en.wikipedia.org/wiki/ISO_week_date)
            DateTime jan4 = new DateTime(year, 1, 4);

            // get a day in the requested week
            DateTime day = jan4.AddDays((week - 1) * 7);

            // get day of week, with [mon = 0 ... sun = 6] instead of [sun = 0 ... sat = 6]
            int dayOfWeek = ((int)day.DayOfWeek + 6) % 7;

            return day.AddDays(-dayOfWeek).AddDays(6);
        }

        public static DateTime ToGetStartDayOfWeek(int year, int dayofweek)
        {
            var fromDate = new DateTime(year, 1, 1);
            var fw = fromDate.Millisecond + 518400000 + (dayofweek - 1);
            return new DateTime();
        }

        public static DateTime ToGetStartDateOfMonth(int year, int month)
        {
            var firstDayOfMonth = new DateTime(year, month, 1);
            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            return firstDayOfMonth;
        }
        public static DateTime ToGetEndDateOfMonth(int year, int month)
        {
            var lastDayOfMonth = ToGetStartDateOfMonth(year, month).AddMonths(1).AddDays(-1);
            return lastDayOfMonth;
        }
        public static QuarterVM ToGetStartAndEndDateOfQuarter(int year, int quarter)
        {
            if (quarter == 1)
                return new QuarterVM
                {
                    start = ToGetStartDateOfMonth(year, 1),
                    end = ToGetEndDateOfMonth(year, 2)
                };
            else if (quarter == 2)
                return new QuarterVM
                {
                    start = ToGetStartDateOfMonth(year, 3),
                    end = ToGetEndDateOfMonth(year, 5)
                };
            else if (quarter == 3)
                return new QuarterVM
                {
                    start = ToGetStartDateOfMonth(year, 6),
                    end = ToGetEndDateOfMonth(year, 9)
                };
            else if (quarter == 4)
                return new QuarterVM
                {
                    start = ToGetStartDateOfMonth(year, 10),
                    end = ToGetEndDateOfMonth(year, 12)
                };
            else return new QuarterVM
            {
                start = new DateTime(),
                end = new DateTime()
            };
        }

        public static YearVM ToGetStartAndEndDateOfYear(int year)
        {

            return new YearVM
            {
                start = ToGetStartDateOfMonth(year, 1),
                end = ToGetEndDateOfMonth(year, 12)
            };

        }
        /// <summary>
        /// Kiểm tra giá trị value có phải là định dạng email không?
        /// Nếu là định dạng email, trả về true; Ngược lại, trả về false.
        /// VD đúng: henry@gmail.com; hello@zing.vn; henry@viettel.vn.
        /// VD sai: demo@demo@.com; a@gmail.com; toan.gmail.com; gmail.com@; dung   @gmail.com
        /// </summary>
        /// <param name="value">Giá trị cần kiểm tra</param>
        /// <returns>Nếu là định dạng email, trả về true; Ngược lại, trả về false.</returns>
        public static bool IsEmailFormat(this object value)
        {
            if (value == null)
                return false;

            //Khai báo một mẫu công thức dùng để kiểm tra
            string pattern = @"^[a-zA-Z0-9_\.]+@[a-zA-Z]+\.[a-zA-Z]+(\.[a-zA-Z]+)*$";

            //Khai báo một item thuộc lớp Regex
            Regex item = new Regex(pattern);

            //Kiểm tra giá trị value có khớp mẫu công thức không?
            if (item.IsMatch(value.ToString()) == true)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Gets the end of quarter.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime GetEndOfQuarter(this DateTime date)
        {
            int daysInYear = GetDaysInYear(date);
            double quarter = ((double)date.DayOfYear) / ((double)daysInYear);

            if (quarter < 0.25)
            {
                return new DateTime(new DateTime(date.Year, 4, 1).Ticks - 1);
            }

            else if (quarter < 0.5)
            {
                return new DateTime(new DateTime(date.Year, 7, 1).Ticks - 1);
            }

            else if (quarter < 0.75)
            {
                return new DateTime(new DateTime(date.Year, 10, 1).Ticks - 1);
            }

            else
            {
                return new DateTime(new DateTime(date.Year + 1, 1, 1).Ticks - 1);
            }
        }
        /// <summary>
        /// Gets the days in year.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static int GetDaysInYear(DateTime date)
        {
            if (date.Equals(DateTime.MinValue))
            {
                return -1;
            }

            DateTime thisYear = new DateTime(date.Year, 1, 1);
            DateTime nextYear = new DateTime(date.Year + 1, 1, 1);

            return (nextYear - thisYear).Days;
        }
        public static DateTime QuarterEnddate(this DateTime curDate, int whichQtr)
        {
            int tQtr = (curDate.Month - 1) / 3 + 1 + whichQtr;
            return new DateTime(curDate.Year, (tQtr * 3) + 1, 1).AddDays(-1);
        }

        public static DateTime QuarterStartDate(this DateTime curDate, int whichQtr)
        {
            return QuarterEnddate(curDate, whichQtr).AddDays(1).AddMonths(-3);
        }
        public static int ConvertStringDayOfWeekToNumber(this string dayofweek)
        {
            var value = dayofweek.ToSafetyString().ToUpper();
            int result;
            switch (value)
            {
                case "MON":
                    result = 2;
                    break;
                case "TUE":
                    result = 3;
                    break;
                case "WED":
                    result = 4;
                    break;
                case "THU":
                    result = 5;
                    break;
                case "FRI":
                    result = 6;
                    break;
                case "SAT":
                    result = 7;
                    break;
                case "SUN":
                    result = 8;
                    break;
                case "MONDAY":
                    result = 2;
                    break;
                case "TUESDAY":
                    result = 3;
                    break;
                case "WEDNESDAY":
                    result = 4;
                    break;
                case "THURSDAY":
                    result = 5;
                    break;
                case "FRIDAY":
                    result = 6;
                    break;
                case "SATURDAY":
                    result = 7;
                    break;
                case "SUNDAY":
                    result = 8;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
        public static string ConvertNumberDayOfWeekToString(this int? dayofweek)
        {
            var value = dayofweek;
            string result;
            switch (value)
            {
                case 2:
                    result = "Monday";
                    break;
                case 3:
                    result = "Tuesday";
                    break;
                case 4:
                    result = "Wednesday";
                    break;
                case 5:
                    result = "Thursday";
                    break;
                case 6:
                    result = "Friday";
                    break;
                case 7:
                    result = "Saturday";
                    break;
                case 8:
                    result = "Sunday";
                    break;
                default:
                    result = "Not found";
                    break;
            }
            return result;
        }
        public static int GetQuarterOfYear(this DateTime date)
        {
            if (date.Month >= 4 && date.Month <= 6)
                return 1;
            else if (date.Month >= 7 && date.Month <= 9)
                return 2;
            else if (date.Month >= 10 && date.Month <= 12)
                return 3;
            else
                return 4;
        }
        /// <summary>
        /// Trả về tuần trong năm.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Trả về tuần trong năm.</returns>
        public static int GetIso8601WeekOfYear(this DateTime date)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo
                .InvariantCulture
                .Calendar
                .GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static int GetQuarter(this DateTime dt)
        {
            return (dt.Month - 1) / 3 + 1;
        }
        //public static int GetQuarter(this DateTime date)
        //{
        //    if (date.Month >= 1 && date.Month <= 3)
        //        return 1;
        //    else if (date.Month >= 4 && date.Month <= 6)
        //        return 2;
        //    else if (date.Month >= 7 && date.Month <= 9)
        //        return 3;
        //    else
        //        return 4;
        //}
        /// <summary>
        /// Chuyển value về dạng chuỗi.
        /// Trả về dạng chuỗi của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng chuỗi của value.</returns>
        public static string ToSafetyString(this object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString();

        }
        public static bool IsDate(this String date)

        {

            try

            {

                DateTime dt = DateTime.Parse(date);

                return true;

            }
            catch

            {

                return false;

            }

        }
        /// <summary>
        /// Chuyển value về dạng chuỗi.
        /// Trả về dạng chuỗi của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng chuỗi của value.</returns>
        public static string ToSafetyString(this object value, string format)
        {
            if (value == null)
                return string.Empty;
            DateTime date;
            // date = DateTime.ParseExact(value.ToString(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            DateTime.TryParseExact(value.ToString(), format, CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out date);
            return date.ToString();

        }
        /// <summary>
        /// Chuyển value về dạng số nguyên(byte).
        /// Trả về dạng số nguyên(byte) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (byte) của value.</returns>
        public static byte ToByte(this object value)
        {
            if (value == null)
                return 0;
            //Khai báo giá trị chứa kết quả mặ định, mặc định là 0
            byte result = 0;

            //Thử ép value thành kiểu byte
            byte.TryParse(value.ToString(), out result);

            //Trả về kết quả đã ép kiểu
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(SByte).
        /// Trả về dạng số nguyên(SByte) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (SByte) của value.</returns>
        public static SByte ToSByte(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            sbyte result = 0;
            sbyte.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(Short).
        /// Trả về dạng số nguyên(Short) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (Short) của value.</returns>
        public static short ToShort(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            short result = 0;
            short.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(ToUInt).
        /// Trả về dạng số nguyên(ToUInt) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (ToUInt) của value.</returns>
        public static uint ToUInt(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;

            ushort result = 0;

            ushort.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(Ushort).
        /// Trả về dạng số nguyên(Ushort) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (Ushort) của value.</returns>
        public static ushort ToUShort(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;

            ushort result = 0;

            ushort.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số nguyên(int).
        /// Trả về dạng số nguyên(int) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số nguyên (int) của value.</returns>
        public static int ToInt(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            int result = 0;
            int.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số thực(Float).
        /// Trả về dạng số thực(Float) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số thực (Float) của value.</returns>
        public static float ToFloat(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;
            float result = 0;
            float.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số thực(Double).
        /// Trả về dạng số thực (Double) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số thực (double) của value.</returns>
        public static double ToDouble(this object value)
        {
            if (value == null || value.ToString() == string.Empty)

                return 0;

            double result = 0;

            double.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số thực(Long).
        /// Trả về dạng số thực (Long) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số thực (Long) của value.</returns>
        public static long ToLong(this object value)
        {
            if (value == null || value.ToString() == string.Empty)

                return 0;

            long result = 0;

            long.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số thực(Long).
        /// Trả về dạng số thực (Long) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số thực (Long) của value.</returns>
        public static ulong ToULong(this object value)
        {
            if (value == null || value.ToString() == string.Empty)

                return 0;

            ulong result = 0;

            ulong.TryParse(value.ToString(), out result);

            return result;
        }

        /// <summary>
        /// Chuyển value về dạng số thực(decimal).
        /// Trả về dạng số thực (decimal) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng số thực (decimal) của value.</returns>
        public static decimal ToDecimal(this object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return 0;

            decimal result = 0;

            decimal.TryParse(value.ToString(), out result);

            return result;
        }
        public static string ToTitleCase(this string value)
        {
            string title = value.ToSafetyString();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            title = textInfo.ToTitleCase(title);
            return title;
        }
        /// <summary>
        /// Chuyển value về dạng kí tự (char).
        /// Trả về dạng kí tự (char) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng kí tự (char) của value.</returns>
        public static char ToChar(this object value)
        {
            //Tối ưu hơn phân cách khi dùng hàm ||
            if (value == null || value.ToString() == string.Empty || (value.ToString().Length > 1))
            {
                return ' ';
            }
            char result = ' ';
            char.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng luận lý (bool).
        /// Trả về dạng luận lý (bool) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng luận lý (bool) của value.</returns>
        public static bool ToBool(this object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value.ToInt() == 1)
            {
                return true;
            }

            bool result = false;
            bool.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Chuyển value về dạng ngày giờ (DateTime).
        /// Trả về dạng ngày giờ (DateTime) của value
        /// </summary>
        /// <param name="value">Giá trị cần chuyển đổi. </param>
        /// <returns>Trả về dạng ngày giờ (DateTime) của value.</returns>
        public static DateTime ToDateTime(this object value)
        {
            if (value == null || value.ToString() == string.Empty || value.ToString() == " ")
                return DateTime.MinValue;

            DateTime result = DateTime.MinValue;

            string[] formats = {"d/M/yyyy", "dd/MM/yyyy", "d/M/yyyy HH:mm:ss", "d/M/yyyy HH:mm", "dd/MM/yyyy HH:mm", "HH:mm:ss", "HH:mm",
"d-M-yyyy", "dd-MM-yyyy", "MM-dd-yyyy","MM-dd-yy", "yyyy-MM-dd", "d-M-yyyy HH:mm:ss", "d-M-yyyy HH:mm", "dd-MM-yyyy HH:mm", "HH:mm:ss", "HH:mm"};//HH phủ cả từ 1-24h còn hh chỉ phủ từ 1-12h

            string[] dateStrings = {"5/1/2009 6:32 PM", "05/01/2009 6:32:05 PM",
                              "5/1/2009 6:32:00", "05/01/2009 06:32",
                              "05/01/2009 06:32:00 PM", "05/01/2009 06:32:00"};
            DateTime.TryParseExact(value.ToString(), formats,
                                    //new CultureInfo("en-US"),//Lấy văn hóa của Mỹ
                                    CultureInfo.CurrentCulture,//Lấy văn hóa của máy tính đang dùng
                                    DateTimeStyles.None,
                                    out result);
            return result;

        }

        public static bool IsNullOrEmpty(this object value)
        {
            if (value == null)
            {
                return true;
            }
            return string.IsNullOrEmpty(value.ToString());
        }

        public static string SHA256Hash(this string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value.ToSafetyString()));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToSafetyString();
        }

        public static string ReplaceSpecial(this string value)
        {
            if (value != string.Empty)
            {
                value = Regex.Replace(value, @"(\s+|@|&|'|\(|\)|<|>|#|+|-)", "");
            }
            return value;
        }

    }
}
