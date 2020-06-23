using core.hctab.sh.Batch;
using core.hctab.sh.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.hctab.sh.Interfaces
{
    public interface IBatchStepConfig
    {
        int ID { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool Active { get; set; }
        string ClassName { get; set; }
        List<SchedulerConfig> Scheduling { get; set; }
        bool isSchedulerActive { get; set; }
        int? RunOrder { get; set; }
    }
}
