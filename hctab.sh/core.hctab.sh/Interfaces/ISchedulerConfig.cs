using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.hctab.sh.Interfaces
{
    public interface ISchedulerConfig
    {
        string DaysOfWeek { get; set; }
        string hhmmFrom { get; set; }
        string hhmmTo { get; set; }
    }
}
