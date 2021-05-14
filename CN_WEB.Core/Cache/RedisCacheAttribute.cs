using System;

namespace CN_WEB.Core.Cache
{
    public class RedisCacheAttribute : Attribute
    {
        public double Duration { get; set;  }
        public TimeMeasure Measure { get; set; }
        public RedisCacheAttribute()
        {
        }
    }

    public enum TimeMeasure
    {
        Second = 0,
        Minute = 1,
        Hour = 2,
        Day = 3
    }
}
