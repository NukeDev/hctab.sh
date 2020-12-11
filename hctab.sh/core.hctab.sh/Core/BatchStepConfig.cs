using core.hctab.sh.Batch;
using core.hctab.sh.Interfaces;
using core.hctab.sh.Log;
using core.hctab.sh.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.hctab.sh.Core
{
    public class BatchStepConfig : IBatchStepConfig
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string ClassName { get; set; }
        public List<SchedulerConfig> Scheduling { get; set; }
        public bool isSchedulerActive { get; set; }
        public int? RunOrder { get; set; }
        private AppLogger Logger { get; set; }
        public void SetStepData(BatchStep step, AppLogger logger)
        {
            this.Logger = logger;
            var checks = CheckConfigValues();
            if (checks.Count() > 0)
            {
                Logger.WriteError("Please check your config file!");
                foreach (var str in checks)
                {
                    Logger.WriteError($"Error on variable: {str}");
                }
            }
            step.Name = this.Name;
            step.Active = this.Active;
            step.ClassName = this.ClassName;
            step.Description = this.Description;
            step.Scheduling = GetScheduling(this.Scheduling);
            step.isSchedulerActive = this.isSchedulerActive;
            step.ID = this.ID;
            step.RunOrder = this.RunOrder;
        }

        private List<string> CheckConfigValues()
        {
            var toRet = new List<string>();
            if (string.IsNullOrEmpty(this.Name))
                toRet.Add(nameof(this.Name));
            if (string.IsNullOrEmpty(this.ClassName))
                toRet.Add(nameof(this.ClassName));
            if (string.IsNullOrEmpty(this.Description))
                toRet.Add(nameof(this.Description));
            if(this.isSchedulerActive == true && this.Scheduling.Count() == 0)
                toRet.Add(nameof(this.Scheduling));
            if(this.ID < 0)
                toRet.Add(nameof(this.ID));

            return toRet;

        }

        private List<Scheduler> GetScheduling(List<SchedulerConfig> sched) 
        {
            var toReturn = new List<Scheduler>();

            foreach(var s in sched)
            {

                var days = s.DaysOfWeek.Split(',');

                for(int x = 0; x < days.Length; x++)
                {
                    var toReturnScheduler = new Scheduler()
                    {
                        hhmmFrom = s.hhmmFrom,
                        hhmmTo = s.hhmmTo
                    };

                    var dayOfWeek = GetDayOfWeekFromString(days[x].ToLower());

                    if(dayOfWeek == null)
                    {
                        throw new ArgumentException($"Invalid DayOfWeek value Schedulation! {days[x]}");
                    }

                    toReturnScheduler.Day = dayOfWeek.Value;
                    toReturn.Add(toReturnScheduler);
                }

            }

            if (CheckSchedulingOverlap(toReturn))
            {
                throw new Exception("There was an error while validating schedulations ranges! Please check your config file! Some time slots overlap!!");
            }

            return toReturn;

        }

        private DayOfWeek? GetDayOfWeekFromString(string dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case "sun":
                    return DayOfWeek.Sunday;
                case "mon":
                    return DayOfWeek.Monday;
                case "tue":
                    return DayOfWeek.Tuesday;
                case "wed":
                    return DayOfWeek.Wednesday;
                case "thu":
                    return DayOfWeek.Thursday;
                case "fri":
                    return DayOfWeek.Friday;
                case "sat":
                    return DayOfWeek.Saturday;
                default:
                    return null;
            }
        }

        private bool CheckSchedulingOverlap(List<Scheduler> schedulers)
        {            
            foreach(var day in schedulers.Select(x => x.Day).Distinct())
            {
                if(schedulers.Where(x=> x.Day == day).Select(x => day).Count() > 1)
                {
                    
                    DateTime[] hhmmFrom = new DateTime[1000];
                    DateTime[] hhmmTo = new DateTime[1000];
                    int count = 0;
                    foreach(var sched in schedulers.Where(x => x.Day == day))
                    {
                        hhmmFrom[count] = DateTime.Today.Add(TimeSpan.Parse(sched.hhmmFrom));
                        hhmmTo[count] = DateTime.Today.Add(TimeSpan.Parse(sched.hhmmTo));
                        count++;
                    }
                    for (int x = 0; x < count; x++)
                    {
                        for (int y = x+1; y < count+1; y++)
                        {
                            var checker = new DateTimeRange()
                            {
                                Start = hhmmFrom[x],
                                End = hhmmTo[x]
                            };

                            var check = checker.Intersects(new DateTimeRange()
                            {
                                Start = hhmmFrom[y],
                                End = hhmmTo[y]
                            });

                            if (check)
                            {
                                Logger.WriteError($"hhmmFrom: {hhmmFrom[x].ToShortTimeString()} - hhmmTo: {hhmmTo[x].ToShortTimeString()} <-- overlaps with --> hhmmFrom: {hhmmFrom[y].ToShortTimeString()} - hhmmTo: {hhmmTo[y].ToShortTimeString()}");
                                return true;
                            }
                                
                        }

                    }
                }
            }

            return false;

        }
    }
}
