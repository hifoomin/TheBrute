using BaseLib.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute
{
    internal class Config : SimpleModConfig
    {
        public static bool EnableTrashHeapAdditions { get; set; } = true;
        public static bool EnableColorfulPhilosophersAdditions { get; set; } = true;
    }
}