using core.hctab.sh.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.hctab.sh.Batch
{
    public abstract class BatchStep
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string ClassName { get; set; }
        public List<Scheduler> Scheduling { get; set; }
        public bool isSchedulerActive { get; set; }
        public abstract bool IsApplicable();

        public abstract void ReadData();

        public abstract void SaveData();

        public abstract void Verify();

        public bool isActive()
        {
            return this.Active;
        }
    }
}
