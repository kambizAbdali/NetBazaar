// NetBazaar.Domain/Extensions/DateTimeExtensions.cs
using System;
using System.Globalization;
using System.Text;

namespace NetBazaar.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// دریافت تاریخ شمسی فعلی با فرمت دلخواه
        /// </summary>
        /// <param name="format">فرمت تاریخ (پیش‌فرض: yyyy/MM/dd)</param>
        /// <param name="persianNumber">آیا از اعداد فارسی استفاده شود؟</param>
        /// <returns>تاریخ شمسی فعلی به صورت رشته</returns>
        public static string GetCurrentPersianDate(string format = "yyyy/MM/dd", bool persianNumber = false)
        {
            return DateTime.Now.ToPersianDateString(format, persianNumber);
        }

        /// <summary>
        /// دریافت تاریخ و زمان شمسی فعلی با فرمت دلخواه
        /// </summary>
        /// <param name="format">فرمت تاریخ و زمان (پیش‌فرض: yyyy/MM/dd HH:mm:ss)</param>
        /// <param name="persianNumber">آیا از اعداد فارسی استفاده شود؟</param>
        /// <returns>تاریخ و زمان شمسی فعلی به صورت رشته</returns>
        public static string GetCurrentPersianDateTime(string format = "yyyy/MM/dd HH:mm:ss", bool persianNumber = false)
        {
            return DateTime.Now.ToPersianDateString(format, persianNumber);
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به رشته تاریخ شمسی با فرمت دلخواه
        /// </summary>
        public static string ToPersianDateString(this DateTime dateTime, string format = "yyyy/MM/dd", bool persianNumber = false)
        {
            var persianCalendar = new PersianCalendar();

            int year = persianCalendar.GetYear(dateTime);
            int month = persianCalendar.GetMonth(dateTime);
            int day = persianCalendar.GetDayOfMonth(dateTime);
            int hour = persianCalendar.GetHour(dateTime);
            int minute = persianCalendar.GetMinute(dateTime);
            int second = persianCalendar.GetSecond(dateTime);
            double millisecond = persianCalendar.GetMilliseconds(dateTime);

            var result = new StringBuilder(format);

            // جایگزینی patternها
            ReplaceFormatPart(result, "yyyy", year.ToString("0000"));
            ReplaceFormatPart(result, "yy", (year % 100).ToString("00"));
            ReplaceFormatPart(result, "MM", month.ToString("00"));
            ReplaceFormatPart(result, "M", month.ToString());
            ReplaceFormatPart(result, "dd", day.ToString("00"));
            ReplaceFormatPart(result, "d", day.ToString());
            ReplaceFormatPart(result, "HH", hour.ToString("00"));
            ReplaceFormatPart(result, "H", hour.ToString());
            ReplaceFormatPart(result, "hh", (hour % 12 == 0 ? 12 : hour % 12).ToString("00"));
            ReplaceFormatPart(result, "h", (hour % 12 == 0 ? 12 : hour % 12).ToString());
            ReplaceFormatPart(result, "mm", minute.ToString("00"));
            ReplaceFormatPart(result, "m", minute.ToString());
            ReplaceFormatPart(result, "ss", second.ToString("00"));
            ReplaceFormatPart(result, "s", second.ToString());
            ReplaceFormatPart(result, "fff", millisecond.ToString("000"));
            ReplaceFormatPart(result, "ff", (millisecond / 10).ToString("00"));
            ReplaceFormatPart(result, "f", (millisecond / 100).ToString());

            // روز هفته
            var dayOfWeek = persianCalendar.GetDayOfWeek(dateTime);
            ReplaceFormatPart(result, "dddd", GetPersianDayOfWeekName(dayOfWeek));
            ReplaceFormatPart(result, "ddd", GetPersianDayOfWeekShortName(dayOfWeek));

            // نام ماه
            ReplaceFormatPart(result, "MMMM", GetPersianMonthName(month));
            ReplaceFormatPart(result, "MMM", GetPersianMonthShortName(month));

            // AM/PM
            ReplaceFormatPart(result, "tt", hour < 12 ? "ق.ظ" : "ب.ظ");
            ReplaceFormatPart(result, "t", hour < 12 ? "ق" : "ب");

            var formattedDate = result.ToString();

            // تبدیل به اعداد فارسی اگر درخواست شده باشد
            if (persianNumber)
            {
                formattedDate = ToPersianNumbers(formattedDate);
            }

            return formattedDate;
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به رشته تاریخ و زمان شمسی
        /// </summary>
        public static string ToPersianDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToPersianDateString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// تبدیل رشته تاریخ شمسی به تاریخ میلادی
        /// </summary>
        public static DateTime FromPersianDateString(string persianDate)
        {
            if (string.IsNullOrWhiteSpace(persianDate))
                throw new ArgumentException("تاریخ نمی‌تواند خالی باشد");
            persianDate = ToEnglishNumbers(persianDate);
            var parts = persianDate.Split('/', '-', ' ');
            if (parts.Length < 3)
                throw new ArgumentException("فرمت تاریخ صحیح نیست");

            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            var persianCalendar = new PersianCalendar();
            return persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
        }

        /// <summary>
        /// تبدیل رشته تاریخ شمسی به تاریخ میلادی (با قابلیت fallback)
        /// </summary>
        public static DateTime FromPersianDateString(string persianDate, DateTime fallbackDate)
        {
            if (string.IsNullOrWhiteSpace(persianDate))
                return fallbackDate;

            try
            {
                var parts = persianDate.Split('/', '-', ' ');
                if (parts.Length < 3)
                    return fallbackDate;

                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);

                var persianCalendar = new PersianCalendar();
                return persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            catch
            {
                return fallbackDate;
            }
        }

        /// <summary>
        /// بررسی می‌کند آیا تاریخ معتبر شمسی است یا نه
        /// </summary>
        public static bool IsValidPersianDate(string persianDate)
        {
            try
            {
                FromPersianDateString(persianDate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// دریافت سال شمسی از تاریخ میلادی
        /// </summary>
        public static int GetPersianYear(this DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            return persianCalendar.GetYear(dateTime);
        }

        /// <summary>
        /// دریافت ماه شمسی از تاریخ میلادی
        /// </summary>
        public static int GetPersianMonth(this DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            return persianCalendar.GetMonth(dateTime);
        }

        /// <summary>
        /// دریافت روز شمسی از تاریخ میلادی
        /// </summary>
        public static int GetPersianDay(this DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            return persianCalendar.GetDayOfMonth(dateTime);
        }

        /// <summary>
        /// دریافت نام ماه شمسی
        /// </summary>
        public static string GetPersianMonthName(this DateTime dateTime)
        {
            var persianCalendar = new PersianCalendar();
            int month = persianCalendar.GetMonth(dateTime);

            return GetPersianMonthName(month);
        }

        /// <summary>
        /// دریافت نام ماه شمسی از شماره ماه
        /// </summary>
        private static string GetPersianMonthName(int month)
        {
            var monthNames = new string[]
            {
                "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
                "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
            };

            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "ماه باید بین 1 تا 12 باشد.");

            return monthNames[month - 1];
        }

        /// <summary>
        /// دریافت نام کوتاه ماه شمسی
        /// </summary>
        private static string GetPersianMonthShortName(int month)
        {
            var monthShortNames = new string[]
            {
                "فرو", "ارد", "خرد", "تیر", "مرد", "شهر",
                "مهر", "آبا", "آذر", "دی", "بهم", "اسف"
            };

            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month), "ماه باید بین 1 تا 12 باشد.");

            return monthShortNames[month - 1];
        }

        /// <summary>
        /// دریافت نام روز هفته به فارسی
        /// </summary>
        private static string GetPersianDayOfWeekName(DayOfWeek dayOfWeek)
        {
            var dayNames = new string[]
            {
                "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه"
            };

            return dayNames[(int)dayOfWeek];
        }

        /// <summary>
        /// دریافت نام کوتاه روز هفته به فارسی
        /// </summary>
        private static string GetPersianDayOfWeekShortName(DayOfWeek dayOfWeek)
        {
            var dayShortNames = new string[]
            {
                "ی", "د", "س", "چ", "پ", "ج", "ش"
            };

            return dayShortNames[(int)dayOfWeek];
        }

        /// <summary>
        /// جایگزینی بخشی از فرمت
        /// </summary>
        private static void ReplaceFormatPart(StringBuilder builder, string pattern, string replacement)
        {
            builder.Replace(pattern, replacement);
        }

        /// <summary>
        /// تبدیل اعداد انگلیسی به فارسی
        /// </summary>
        public static string ToPersianNumbers(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                result.Append(c switch
                {
                    '0' => '۰',
                    '1' => '۱',
                    '2' => '۲',
                    '3' => '۳',
                    '4' => '۴',
                    '5' => '۵',
                    '6' => '۶',
                    '7' => '۷',
                    '8' => '۸',
                    '9' => '۹',
                    _ => c
                });
            }

            return result.ToString();
        }

        /// <summary>
        /// تبدیل اعداد فارسی به انگلیسی
        /// </summary>
        public static string ToEnglishNumbers(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                result.Append(c switch
                {
                    '۰' => '0',
                    '۱' => '1',
                    '۲' => '2',
                    '۳' => '3',
                    '۴' => '4',
                    '۵' => '5',
                    '۶' => '6',
                    '۷' => '7',
                    '۸' => '8',
                    '۹' => '9',
                    _ => c
                });
            }

            return result.ToString();
        }

        /// <summary>
        /// دریافت اولین روز سال شمسی جاری
        /// </summary>
        public static DateTime GetFirstDayOfCurrentPersianYear()
        {
            var now = DateTime.Now;
            var persianCalendar = new PersianCalendar();
            int currentYear = persianCalendar.GetYear(now);

            return persianCalendar.ToDateTime(currentYear, 1, 1, 0, 0, 0, 0);
        }

        /// <summary>
        /// دریافت آخرین روز سال شمسی جاری
        /// </summary>
        public static DateTime GetLastDayOfCurrentPersianYear()
        {
            var now = DateTime.Now;
            var persianCalendar = new PersianCalendar();
            int currentYear = persianCalendar.GetYear(now);
            bool isLeapYear = persianCalendar.IsLeapYear(currentYear);

            int lastDay = isLeapYear ? 30 : 29;
            return persianCalendar.ToDateTime(currentYear, 12, lastDay, 23, 59, 59, 999);
        }
    }
}