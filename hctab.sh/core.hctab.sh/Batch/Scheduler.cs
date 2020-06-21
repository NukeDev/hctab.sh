using core.hctab.sh.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.hctab.sh.Batch
{
    public class Scheduler : IScheduler
    {
        public DayOfWeek Day { get; set; }
        public string hhmmFrom { get; set; }
        public string hhmmTo { get; set; }
    }
}
