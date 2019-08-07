using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AillieoUtils
{
    public class TimeUtils
    {

        static readonly DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

        public static long TimeStampNow()
        {
            return TimeStamp(DateTime.Now);
        }

        public static long TimeStamp(DateTime dateTime)
        {
            return Convert.ToInt64((dateTime - startTime).TotalSeconds);
        }

        public static DateTime GetDateTime(long timeStamp)
        {
            return startTime.AddSeconds(timeStamp);
        }

        public static string FormatTime(long timeStamp, string format = "{0:yyyy/MM/dd dddd HH:mm}")
        {
            return string.Format(format, GetDateTime(timeStamp));
        }
    }
}


