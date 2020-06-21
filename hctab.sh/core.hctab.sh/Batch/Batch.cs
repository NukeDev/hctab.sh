using core.hctab.sh.Core;
using core.hctab.sh.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace core.hctab.sh.Batch
{
    public class Batch
    {
        private List<int> waitingList = new List<int>();

        private string Name { get; set; }
        private int LoggingType { get; set; }
        private DateTimeOffset StartupTime = DateTimeOffset.Now;
        private List<BatchStep> StepList = new List<BatchStep>();
        private List<BatchStepConfig> ConfigStepList = new List<BatchStepConfig>();
        public void Init(string ConfigPath)
        {
            var cfg = new ReadConfig().Read(ConfigPath);
            this.Name = cfg.Name;
            this.ConfigStepList = cfg.StepList;
            LoadSteps();
            CheckStepList();
        }

        private void LoadSteps()
        {
            foreach(var step in ConfigStepList)
            {
                var myStep = ((BatchStep)GetInstance(step.ClassName));
                step.SetStepData(myStep);
                StepList.Add(myStep);
            }
            

        }

        private object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }

        private void CheckStepList()
        {
            foreach(var step in StepList)
            {
                if (step.isActive())
                {
                    foreach(var scheduler in step.Scheduling)
                    {
                        var hhmmFrom = DateTime.Today.Add(TimeSpan.Parse(scheduler.hhmmFrom));
                        var hhmmTo = DateTime.Today.Add(TimeSpan.Parse(scheduler.hhmmTo));

                        if (scheduler.Day == DateTime.Now.DayOfWeek && hhmmFrom <= DateTime.Now && DateTime.Now <= hhmmTo && step.isSchedulerActive)
                        {
                            waitingList.Add(step.ID);
                            break;
                        }
                        else if (!step.isSchedulerActive)
                        {
                            waitingList.Add(step.ID);
                            break;
                        }
                    }
                }
            }

            if(waitingList.Count > 0)
            {
                foreach(var id in waitingList)
                {
                    Execute(StepList.Where(x => x.ID == id).FirstOrDefault());
                }
            }
            else
            {
                Console.WriteLine("Nothing to be scheduled!");
            }

        }

        private void Execute(BatchStep Step)
        {
            Console.WriteLine($"Executing step ID: {Step.ID}");
            if (Step.IsApplicable())
            {
                Console.WriteLine($"step ID: {Step.ID} applicable");
                Step.ReadData();
                Console.WriteLine($"step ID: {Step.ID} Read Data");
                Step.Verify();
                Console.WriteLine($"step ID: {Step.ID} Verify");
            }
        }

    }
}
