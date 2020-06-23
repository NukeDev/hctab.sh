using core.hctab.sh.Core;
using core.hctab.sh.Interfaces;
using core.hctab.sh.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace core.hctab.sh.Batch
{
    public class Batch : AppLogger
    {
        private List<int> waitingList = new List<int>();

        private string Name { get; set; }
        private DateTimeOffset StartupTime = DateTimeOffset.Now;
        private List<BatchStep> StepList = new List<BatchStep>();
        private List<BatchStepConfig> ConfigStepList = new List<BatchStepConfig>();
        public AppLogger Logger = new AppLogger();

        public void Init(string ConfigPath)
        {
            try
            {
                Logger.WriteInformation($"Loading Config file... -> {ConfigPath}");
                var cfg = new ReadConfig().Read(ConfigPath);
                this.Name = cfg.Name;
                this.ConfigStepList = cfg.StepList;
                Logger.WriteInformation($"Config loaded!");
                Logger.WriteInformation($"Loading Batch Step List...");
                LoadSteps();
                CheckStepList();
            }
            catch(Exception ex)
            {
                Logger.WriteError(ex.Message);
            }
           
        }

        private void LoadSteps()
        {
            foreach(var step in ConfigStepList)
            {
                var myStep = ((BatchStep)GetInstance(step.ClassName));
                step.SetStepData(myStep);
                Logger.WriteInformation($"{step.Name} Loaded! - Active: {step.Active}, ID: {step.ID}");
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
                            Logger.WriteWarning($"{step.Name} Scheduled! - DayOfWeek: {scheduler.Day}, hhmmFrom: {scheduler.hhmmFrom}, hhmmTo: {scheduler.hhmmTo}");
                            break;
                        }
                        else if (!step.isSchedulerActive)
                        {
                            waitingList.Add(step.ID);
                            Logger.WriteWarning($"{step.Name} Scheduled! - Scheduler disabled, but step active!");
                            break;
                        }
                    }
                }
            }

            if(waitingList.Count > 0)
            {
                foreach(var id in waitingList)
                {
                    var step = StepList.Where(x => x.ID == id).FirstOrDefault();

                    Logger.WriteWarning($"{step.Name} Started!");

                    Execute(step);
                }

                Logger.WriteWarning($"All steps finished! Run time: {(DateTimeOffset.Now.DateTime - StartupTime.DateTime).TotalMinutes.ToString() } mins");

            }
            else
            {
                Logger.WriteWarning($"Nothing to run.. :(");
            }

        }

        private void Execute(BatchStep Step)
        {
            try
            {
                if (Step.IsApplicable())
                {
                    Logger.WriteInformation($"{Step.Name}: Is Applicable");
                    Logger.WriteInformation($"{Step.Name}: Reading Data..");
                    Step.ReadData();
                    Logger.WriteInformation($"{Step.Name}: Verifing..");
                    Step.Verify();
                    Logger.WriteInformation($"{Step.Name}: Saving data...");
                    Step.SaveData();
                    Logger.WriteInformation($"{Step.Name}: Finishing...");
                }
            }
            catch(Exception ex)
            {
                Logger.WriteError($"Message: {ex.Message}, InnerMessage: {ex.ToString()}");
                Logger.WriteWarning("The latest STEP got an error, skipping to the next one...");
            }
            
        }

    }
}
