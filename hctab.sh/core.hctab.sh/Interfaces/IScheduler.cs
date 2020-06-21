using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.hctab.sh.Interfaces
{
    public interface IScheduler
    {
        DayOfWeek Day { get; set; }
        string hhmmFrom { get; set; }
        string hhmmTo { get; set; }
    }
}
