
using System.Globalization;

namespace System
{
    public static partial class DateTimeExtender
    {
        #region to period "{0} Tahun, {1} Bulan and {2} Hari"



        public static string toPeriodIndo(this DateTime xFrom, DateTime xTo)
        {
            return toPeriodString(xFrom, xTo, "INDO", true);
        }

        public static string toPeriodEnglish(this DateTime xFrom, DateTime xTo)
        {
            return toPeriodString(xFrom, xTo, "ENG", true);
        }


        public static string toPeriodShortIndo(this DateTime xFrom, DateTime xTo)
        {
            return toPeriodString(xFrom, xTo, "INDO", false);
        }

        public static string toPeriodShortEnglish(this DateTime xFrom, DateTime xTo)
        {
            return toPeriodString(xFrom, xTo, "ENG", false);
        }




        private static string toPeriodString(DateTime xFrom, DateTime xTo, string lang, bool isLong)
        {
            DateTime dt = xTo;

            int days = dt.Day - xFrom.Day;
            if (days < 0)
            {
                dt = dt.AddMonths(-1);
                days += DateTime.DaysInMonth(dt.Year, dt.Month);
            }

            int months = dt.Month - xFrom.Month;
            if (months < 0)
            {
                dt = dt.AddYears(-1);
                months += 12;
            }

            int years = dt.Year - xFrom.Year;

            if (lang == "INDO")
            {
                if (isLong)
                {
                    return string.Format("{0} Tahun, {1} Bulan, {2} Hari",
                                         years,
                                         months,
                                         days);
                }
                else
                {
                    return string.Format("{0} Tahun, {1} Bulan",
                                       years,
                                       months
                                       );
                }
            }
            else
            {
                if (isLong)
                {
                    return string.Format("{0} Year{1}, {2} Month{3} and {4} Day{5}",
                                     years, (years == 1) ? "" : "s",
                                     months, (months == 1) ? "" : "s",
                                     days, (days == 1) ? "" : "s");
                }
                else
                {
                    return string.Format("{0} Year{1}, {2} Month{3}",
                                     years, (years == 1) ? "" : "s",
                                     months, (months == 1) ? "" : "s"
                                     );
                }
            }
        }



        #endregion




        /// <summary>
        /// format date as string in MMMM yyyy. ex : Januari 2013
        /// </summary>
        /// <param name="t_date">date to format.</param>        
        public static String toMonthNameYearIndo(this DateTime t_date)
        {
            return toMonthNameIndo(t_date) + " " + t_date.Year.ToString();
        }


        /// <summary>
        /// periode dalam kalimat bulan panjang
        /// </summary>
        /// <param name="periode"></param>
        /// <returns></returns>
        public static String periodToYearMonthLongNameIndo(string periode)
        {
            DateTime t_date = new DateTime(int.Parse(periode.Left(4)), int.Parse(periode.Right(2)), int.Parse("01"));

            return toMonthNameIndo(t_date) + " " + t_date.Year.ToString();
        }



        public static DateTime periodToDate(string periode)
        {
            DateTime t_date = new DateTime(int.Parse(periode.Left(4)), int.Parse(periode.Right(2)), int.Parse("01"));

            return t_date;
        }




        /// <summary>
        /// return Month Name in Bahasa
        /// </summary>
        /// <param name="t_date">date to format.</param>        
        public static String toMonthNameIndo(this DateTime t_date)
        {
            string retVal = "";

            switch (t_date.Month)
            {
                case 1:
                    retVal += "Januari";
                    break;
                case 2:
                    retVal += "Februari";
                    break;
                case 3:
                    retVal += "Maret";
                    break;
                case 4:
                    retVal += "April";
                    break;
                case 5:
                    retVal += "Mei";
                    break;
                case 6:
                    retVal += "Juni";
                    break;
                case 7:
                    retVal += "Juli";
                    break;
                case 8:
                    retVal += "Agustus";
                    break;
                case 9:
                    retVal += "September";
                    break;
                case 10:
                    retVal += "Oktober";
                    break;
                case 11:
                    retVal += "November";
                    break;
                case 12:
                    retVal += "Desember";
                    break;
                default:
                    retVal += "Invalid Month";
                    break;
            }
            return retVal;
        }

        /// <summary>
        /// return Day Name in Bahasa
        /// </summary>
        /// <param name="t_date">date to format.</param>        
        public static String toDayNameIndo(this DateTime t_date)
        {
            string retVal = "";

            switch (t_date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    retVal += "Minggu";
                    break;
                case DayOfWeek.Monday:
                    retVal += "Senin";
                    break;
                case DayOfWeek.Tuesday:
                    retVal += "Selasa";
                    break;
                case DayOfWeek.Wednesday:
                    retVal += "Rabu";
                    break;
                case DayOfWeek.Thursday:
                    retVal += "Kamis";
                    break;
                case DayOfWeek.Friday:
                    retVal += "Jumat";
                    break;
                case DayOfWeek.Saturday:
                    retVal += "Sabtu";
                    break;
                default:
                    retVal += "Invalid Day";
                    break;
            }
            return retVal;
        }

        /// <summary>
        /// format date as string in DD MMMM yyyy HH:MM:SS
        /// </summary>
        /// <param name="t_date"></param>
        /// <returns></returns>
        public static String toLongIndoDateTime(this DateTime t_date)
        {
            string retVal = "";

            if (t_date.Day.ToString().Length < 2)
            {
                retVal += "0" + t_date.Day.ToString();
            }
            else
            {
                retVal += t_date.Day.ToString();
            }

            retVal += " " + toMonthNameYearIndo(t_date);

            retVal += " " + ("00" + t_date.Hour.ToString()).Right(2) + ":" + ("00" + t_date.Minute.ToString()).Right(2) + ":" + ("00" + t_date.Second.ToString()).Right(2);
            return retVal;

        }


        public static DateTime? parseExact(string s, string format, IFormatProvider provider)
        {
            if (s == "")
            {
                return null;
            }
            else
            {
                try
                {
                    DateTime x = DateTime.ParseExact(s, format, provider);
                    return x;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// format date as string in dd-MM-YYYY
        /// </summary>
        /// <param name="t_date">date to format.</param>        
        public static String toIndoDate(this DateTime t_date)
        {
            return t_date.ToString("dd-MM-yyyy");
        }


        /// <summary>
        /// format date as string in ddMMYYYY
        /// </summary>
        /// <param name="t_date">date to format.</param>        
        public static String toIndoDateWithoutSeperator(this DateTime t_date)
        {
            return t_date.ToString("ddMMyyyy");
        }


        /// <summary>
        /// format date as string in dd-MM-YYYY HH:MM:SS
        /// </summary>
        /// <param name="t_date">date to format.</param>        
        public static String toIndoDateTime(this DateTime t_date)
        {
            return t_date.ToString("dd-MM-yyyy HH:mm:ss");
        }

        /// <summary>
        /// Format date as string in dd-MM-YYYY HH:MM
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string toIndoDateTimeHourMinute(this DateTime date)
        {
            return date.ToString("dd-MM-yyyy HH:mm");
        }

        /// <summary>
        /// Format date as string in HH:MM
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string toIndoDateHourMinuteOnly(this DateTime date)
        {
            return date.ToString("HH:mm");
        }


        /// <summary>
        /// format date as string in dd MMMM yyyy
        /// </summary>
        /// <param name="t_date">date to format.</param>        
        public static String toLongIndoDate(this DateTime t_date)
        {
            string retVal = "";

            retVal = ("00" + t_date.Day.ToString()).Right(2) + " " + toMonthNameYearIndo(t_date);

            return retVal;
        }

        /// <summary>
        /// Format date as string in HH:MM:SS
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static String toLongIndoTime(this DateTime t_date)
        {
            string retVal = "";

            retVal += ("00" + t_date.Hour.ToString()).Right(2) + ":" + ("00" + t_date.Minute.ToString()).Right(2) + ":" + ("00" + t_date.Second.ToString()).Right(2);
            return retVal;

        }

        /// <summary>
        /// format date as string in YYYY-MM-DD
        /// </summary>
        /// <param name="t_date">date to format.</param>
        public static String date_to_UnivDate(this DateTime t_date)
        {
            return String.Format("{0}-{1}-{2}", t_date.Year, t_date.Month, t_date.Day);
        }

        /// <summary>
        /// format date as YYYY-MM-DD HH:mm:ss.sss
        /// </summary>
        /// <param name="t_date">date to format.</param>
        public static String datetime_to_UnivDateTime(this DateTime t_dt)
        {
            return t_dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //return String.Format("{0}-{1}-{2} {3}:{4}:{5}.{6}", t_dt.Year, t_dt.Month, t_dt.Day, t_dt.Hour, t_dt.Minute, t_dt.Second, t_dt.Millisecond);
        }

        /// <summary>
        /// format date as string in YYYYMM
        /// </summary>
        /// <param name="t_date">date to format.</param>
        public static String date_to_yearMonth(this DateTime t_dt)
        {
            string retVal = t_dt.Year.ToString();

            if (t_dt.Month.ToString().Length < 2)
            {
                retVal += "0" + t_dt.Month.ToString();
            }
            else
            {
                retVal += t_dt.Month.ToString();
            }
            return retVal;
        }

        /// <summary>
        /// format date as string in YYYYMMDD
        /// </summary>
        /// <param name="t_date">date to format.</param>
        public static String date_to_yearMonthDay(this DateTime t_dt)
        {
            string retVal = ("0000" + t_dt.Year.ToString()).Right(4) + ("00" + t_dt.Month.ToString()).Right(2) + ("00" + t_dt.Day.ToString()).Right(2);
            return retVal;
        }

        /// <summary>
        /// format date as string in yyyyMMDD hh:mm:ss
        /// </summary>
        /// <param name="t_date">date to format.</param>
        public static String date_to_yearMonthDayTime(this DateTime t_dt)
        {
            string retVal = t_dt.ToString("yyyyMMdd HH:mm:ss");
            return retVal;
        }


        /// <summary>
        /// format date as string in yyyyMMdd_HHmmss
        /// </summary>
        /// <param name="t_date">date to format.</param>
        public static String date_to_yearMonthDayTimeString(this DateTime t_dt)
        {
            string retVal = t_dt.ToString("yyyyMMdd_HHmmss");
            return retVal;
        }

        /// <summary>
        /// return date in 00:00:00
        /// </summary>
        /// <param name="t_date">date to format.</param>
        public static DateTime date_toZeroDate(this DateTime t_dt)
        {
            return new DateTime(t_dt.Year, t_dt.Month, t_dt.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// return date in 23:59:59.998
        /// </summary>
        /// <param name="t_date">date to format.</param>
        public static DateTime date_toEndDate(this DateTime t_dt)
        {
            return new DateTime(t_dt.Year, t_dt.Month, t_dt.Day, 23, 59, 59, 998);
        }


        /// <summary>
        /// parse YYYYMMDD to datetime
        /// </summary>
        /// <param name="t_date">date to format.</param>
        public static DateTime parseYearMonthDay(string YMD)
        {
            try
            {
                if (YMD.Length == 8)
                {
                    DateTime x = new DateTime(int.Parse(YMD.Substring(0, 4)), int.Parse(YMD.Substring(4, 2)), int.Parse(YMD.Substring(6, 2)));
                    return x;
                }
                else
                {
                    throw new InvalidCastException("invalid YearMonthDay format. the format should be YYYYMMDD");
                }
            }
            catch (Exception)
            {
                throw;// new InvalidCastException("invalid YearMonthDay format. the format should be YYYYMMDD");                
            }

        }

        public static DateTime FirstDayOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }
        public static DateTime FirstDayOfYear(this DateTime dt, DayOfWeek dow)
        {
            return dt.FirstDayOfYear().NextDay(dow, true);
        }
        public static DateTime LastDayOfYear(this DateTime dt)
        {
            return dt.FirstDayOfYear().AddYears(1).AddDays(-1);
        }
        public static DateTime LastDayOfYear(this DateTime dt, DayOfWeek dow)
        {
            return dt.LastDayOfYear().PreviousDay(dow, true);
        }
        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
        public static DateTime FirstDayOfLastMonth(this DateTime dt)
        {
            return dt.FirstDayOfMonth().AddMonths(-1);
        }
        public static DateTime FirstDayOfMonth(this DateTime dt, DayOfWeek dow)
        {
            return dt.FirstDayOfMonth().NextDay(dow, true);
        }
        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            return dt.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }
        public static DateTime LastDayOfMonth(this DateTime dt, DayOfWeek dow)
        {
            return dt.LastDayOfMonth().PreviousDay(dow, true);
        }
        public static DateTime PreviousDay(this DateTime dt)
        {
            return dt.Date.AddDays(-1);
        }
        public static DateTime PreviousDay(this DateTime dt, DayOfWeek dow)
        {
            return dt.PreviousDay(dow, false);
        }
        public static DateTime PreviousDay(this DateTime dt, DayOfWeek dow, bool includeThis)
        {
            int diff = dt.DayOfWeek - dow;
            if ((includeThis && diff < 0) || (!includeThis && diff <= 0)) diff += 7;
            return dt.Date.AddDays(-diff);
        }
        public static DateTime NextDay(this DateTime dt)
        {
            return dt.Date.AddDays(1);
        }
        public static DateTime NextDay(this DateTime dt, DayOfWeek dow)
        {
            return dt.NextDay(dow, false);
        }
        public static DateTime NextDay(this DateTime dt, DayOfWeek dow, bool includeThis)
        {
            int diff = dow - dt.DayOfWeek;
            if ((includeThis && diff < 0) || (!includeThis && diff <= 0)) diff += 7;
            return dt.Date.AddDays(diff);
        }
        public static int DaysInYear(this DateTime dt)
        {
            return (dt.LastDayOfYear() - dt.FirstDayOfYear()).Days + 1;
        }
        public static int DaysInYear(this DateTime dt, DayOfWeek dow)
        {
            return (dt.LastDayOfYear(dow).DayOfYear - dt.FirstDayOfYear(dow).DayOfYear) / 7 + 1;
        }
        public static int DaysInMonth(this DateTime dt)
        {
            return (dt.LastDayOfMonth() - dt.FirstDayOfMonth()).Days + 1;
        }
        public static int DaysInMonth(this DateTime dt, DayOfWeek dow)
        {
            return (dt.LastDayOfMonth(dow).Day - dt.FirstDayOfMonth(dow).Day) / 7 + 1;
        }
        public static bool IsLeapYear(this DateTime dt)
        {
            return dt.DaysInYear() == 366;
        }
        public static DateTime AddWeeks(this DateTime dt, int weeks)
        {
            return dt.AddDays(7 * weeks);
        }

        private static int DateValue(this DateTime dt)
        {
            return dt.Year * 372 + (dt.Month - 1) * 31 + dt.Day - 1;
        }

        public static int YearsBetween(this DateTime dt, DateTime dt2)
        {
            return dt.MonthsBetween(dt2) / 12;
        }
        public static int YearsBetween(this DateTime dt, DateTime dt2, bool includeLastDay)
        {
            return dt.MonthsBetween(dt2, includeLastDay) / 12;
        }
        public static int YearsBetween(this DateTime dt, DateTime dt2, bool includeLastDay, out int excessMonths)
        {
            int months = dt.MonthsBetween(dt2, includeLastDay);
            excessMonths = months % 12;
            return months / 12;
        }
        public static int MonthsBetween(this DateTime dt, DateTime dt2)
        {
            int months = (dt2.DateValue() - dt.DateValue()) / 31;
            return Math.Abs(months);
        }
        public static int MonthsBetween(this DateTime dt, DateTime dt2, bool includeLastDay)
        {
            if (!includeLastDay) return dt.MonthsBetween(dt2);
            int days;
            if (dt2 >= dt)
                days = dt2.AddDays(1).DateValue() - dt.DateValue();
            else
                days = dt.AddDays(1).DateValue() - dt2.DateValue();
            return days / 31;
        }
        public static int WeeksBetween(this DateTime dt, DateTime dt2)
        {
            return dt.DaysBetween(dt2) / 7;
        }
        public static int WeeksBetween(this DateTime dt, DateTime dt2, bool includeLastDay)
        {
            return dt.DaysBetween(dt2, includeLastDay) / 7;
        }
        public static int WeeksBetween(this DateTime dt, DateTime dt2, bool includeLastDay, out int excessDays)
        {
            int days = dt.DaysBetween(dt2, includeLastDay);
            excessDays = days % 7;
            return days / 7;
        }
        public static int DaysBetween(this DateTime dt, DateTime dt2)
        {
            return (dt2.Date - dt.Date).Duration().Days;
        }
        public static int DaysBetween(this DateTime dt, DateTime dt2, bool includeLastDay)
        {
            int days = dt.DaysBetween(dt2);
            if (!includeLastDay) return days;
            return days + 1;
        }

        public static int MonthNameToIntEng(string MonthName)
        {
            int ouwt = 0;

            if (string.Equals(MonthName, "January", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 1; }
            else if (string.Equals(MonthName, "February", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 2; }
            else if (string.Equals(MonthName, "March", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 3; }
            else if (string.Equals(MonthName, "April", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 4; }
            else if (string.Equals(MonthName, "May", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 5; }
            else if (string.Equals(MonthName, "June", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 6; }
            else if (string.Equals(MonthName, "July", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 7; }
            else if (string.Equals(MonthName, "August", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 8; }
            else if (string.Equals(MonthName, "September", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 9; }
            else if (string.Equals(MonthName, "October", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 10; }
            else if (string.Equals(MonthName, "November", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 11; }
            else if (string.Equals(MonthName, "December", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 12; }
            return ouwt;
        }


        public static int MonthNameToIntInd(string MonthName)
        {
            int ouwt = 0;

            if (string.Equals(MonthName, "Januari", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 1; }
            else if (string.Equals(MonthName, "Februari", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 2; }
            else if (string.Equals(MonthName, "Maret", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 3; }
            else if (string.Equals(MonthName, "April", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 4; }
            else if (string.Equals(MonthName, "Mei", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 5; }
            else if (string.Equals(MonthName, "Juni", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 6; }
            else if (string.Equals(MonthName, "Juli", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 7; }
            else if (string.Equals(MonthName, "Agustus", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 8; }
            else if (string.Equals(MonthName, "September", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 9; }
            else if (string.Equals(MonthName, "Oktober", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 10; }
            else if (string.Equals(MonthName, "November", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 11; }
            else if (string.Equals(MonthName, "Desember", StringComparison.CurrentCultureIgnoreCase)) { ouwt = 12; }
            return ouwt;
        }



        public static DateTime AddBusinessDays(this DateTime startDate,
                                         int businessDays)
        {
            int direction = Math.Sign(businessDays);
            if (direction == 1)
            {
                if (startDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    startDate = startDate.AddDays(2);
                    businessDays = businessDays - 1;
                }
                else if (startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    startDate = startDate.AddDays(1);
                    businessDays = businessDays - 1;
                }
            }
            else
            {
                if (startDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    startDate = startDate.AddDays(-1);
                    businessDays = businessDays + 1;
                }
                else if (startDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    startDate = startDate.AddDays(-2);
                    businessDays = businessDays + 1;
                }
            }

            int initialDayOfWeek = Convert.ToInt32(startDate.DayOfWeek);

            int weeksBase = Math.Abs(businessDays / 5);
            int addDays = Math.Abs(businessDays % 5);

            if ((direction == 1 && addDays + initialDayOfWeek > 5) ||
                 (direction == -1 && addDays >= initialDayOfWeek))
            {
                addDays += 2;
            }

            int totalDays = (weeksBase * 7) + addDays;
            return startDate.AddDays(totalDays * direction);
        }


        public static DateTime AddBusinessDays2(this DateTime date, int days)
        {
            if (days < 0)
            {
                throw new ArgumentException("days cannot be negative", "days");
            }

            if (days == 0) return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                days -= 1;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                days -= 1;
            }

            date = date.AddDays(days / 5 * 7);
            int extraDays = days % 5;

            if ((int)date.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }

            return date.AddDays(extraDays);
        }


        public static int GetBusinessDays(DateTime start, DateTime end)
        {
            if (start.DayOfWeek == DayOfWeek.Saturday)
            {
                start = start.AddDays(2);
            }
            else if (start.DayOfWeek == DayOfWeek.Sunday)
            {
                start = start.AddDays(1);
            }

            if (end.DayOfWeek == DayOfWeek.Saturday)
            {
                end = end.AddDays(-1);
            }
            else if (end.DayOfWeek == DayOfWeek.Sunday)
            {
                end = end.AddDays(-2);
            }

            int diff = (int)end.Subtract(start).TotalDays;

            int result = diff / 7 * 5 + diff % 7;

            if (end.DayOfWeek < start.DayOfWeek)
            {
                return result - 2;
            }
            else
            {
                return result;
            }
        }

        public static DateTime FirstDayOfWeek(this DateTime dateTime, DayOfWeek startOfWeek)
        {
            int different = dateTime.DayOfWeek - startOfWeek;
            if (different < 0)
            {
                different += 7;
            }
            return dateTime.AddDays(-1 * different).Date;
        }

        /// <summary>
        /// Determines whether the input <see cref="DateTime"/> is inclusively between the lower and upper bound
        /// </summary>
        /// <param name="input">The <see cref="DateTime"/> to compare</param>
        /// <param name="left">The lower bound <see cref="DateTime"/> of comparison</param>
        /// <param name="right">The upper bound <see cref="DateTime"/> of comparison</param>
        /// <returns>True if the input is inclusively between the left and right bounds</returns>
        public static bool InclusiveBetween(this DateTime input, DateTime left, DateTime right)
        {
            return input >= left && input <= right;
        }

        /// <summary>
        /// Determines whether the input <see cref="DateTime"/> is exclusively between the lower and upper bound
        /// </summary>
        /// <param name="input">The <see cref="DateTime"/> to compare</param>
        /// <param name="left">The lower bound <see cref="DateTime"/> of comparison</param>
        /// <param name="right">The upper bound <see cref="DateTime"/> of comparison</param>
        /// <returns>True if the input is inclusively between the left and right bounds</returns>
        public static bool ExclusiveBetween(this DateTime input, DateTime left, DateTime right)
        {
            return input > left && input < right;
        }

        //sample : UTC+7  atau  UTC+5:30
        public static string UTC_to_OffSet(string xUTC)
        {
            xUTC = xUTC.Replace("UTC", "");

            int a = xUTC.Length;
            int b = xUTC.IndexOf(":");

            if (xUTC.IndexOf(":") > 0)
            {
                xUTC = xUTC.Left(1)
                    + ("00" + xUTC.Substring(1, xUTC.IndexOf(":") - 1)).Right(2)
                    + ":"
                    + xUTC.Right(2)
                   ;
            }
            else
            {
                //asumsi gk ada jam nya. bikin jadi bentuk hh:mm
                xUTC = xUTC.Left(1)
                    + ("00" + xUTC.Right(xUTC.Length - 1)).Right(2) + ":00";
            }
            return xUTC;
        }


        public static TimeSpan UTC_to_TimeSpan(string xUTC)
        {
            xUTC = xUTC.Replace("UTC", "");

            int xHour = 0;
            int xMinute = 0;
            
            if (xUTC.IndexOf(":") > 0)
            {
                xHour = int.Parse(("00" + xUTC.Substring(1, xUTC.IndexOf(":") - 1)).Right(2));
                xMinute = int.Parse(xUTC.Right(2));
            }
            else
            {
                //asumsi gk ada jam nya. bikin jadi bentuk hh:mm                
                xHour = int.Parse(("00" + xUTC.Right(xUTC.Length - 1)).Right(2));
            }

            if (xUTC.Left(1) == "-")
            {
                xHour = -1 * xHour;
            }
            
            TimeSpan xRetVal = new TimeSpan(xHour, xMinute, 0);
            
            return xRetVal;
        }





    }
}
