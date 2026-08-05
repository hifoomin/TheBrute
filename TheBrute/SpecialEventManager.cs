using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute
{
    public static class SpecialEventManager
    {
        public static bool IsNewYears(DateTime date)
        {
            return date.Month == 1 && date.Day == 1;
            // return date.Month == 8 && date.Day == 1;
        }

        internal static bool IsAprilFools(DateTime date)
        {
            return date.Month == 4 && date.Day == 1;
            // return date.Month == 8 && date.Day == 1;
        }

        internal static bool IsChristmas(DateTime date)
        {
            return date.Month == 12 && date.Day >= 24 && date.Day <= 26;
            // return date.Month == 8 && date.Day == 1;
        }
    }
}