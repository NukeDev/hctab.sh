using core.hctab.sh.Batch;
using core.hctab.sh.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.hctab.sh.Core
{
    public class ReadConfig
    {
        public string Name { get; set; }
        public int LoggingType { get; set; }
        public List<BatchStepConfig> StepList { get; set; }

        public ReadConfig Read(string configPath)
        {
            try
            {
                var fileData = File.ReadAllText(configPath);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ReadConfig>(fileData);
                return obj;
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR WHILE LOADING CONFIG: " + ex.Message);
                Console.ReadKey();
                Environment.Exit(0);
                return null;
            }
          
        }

    }
}
