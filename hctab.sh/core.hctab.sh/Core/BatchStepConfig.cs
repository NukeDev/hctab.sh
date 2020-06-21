using core.hctab.sh.Batch;
using core.hctab.sh.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.hctab.sh.Core
{
    public class BatchStepConfig : IBatchStep
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string ClassName { get; set; }
        public List<Scheduler> Scheduling { get; set; }
        public bool isSchedulerActive { get; set; }
        public int? RunOrder { get; set; }
        public void SetStepData(BatchStep step)
        {
            step.Name = this.Name;
            step.Active = this.Active;
            step.ClassName = this.ClassName;
            step.Description = this.Description;
            step.Scheduling = this.Scheduling;
            step.isSchedulerActive = this.isSchedulerActive;
            step.ID = this.ID;
            step.RunOrder = this.RunOrder;
        }
    }
}
