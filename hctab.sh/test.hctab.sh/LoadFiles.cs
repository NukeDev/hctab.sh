using core.hctab.sh.Batch;
using core.hctab.sh.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test.hctab.sh
{
    class LoadFiles : BatchStep, IBatchStep
    {
        private List<string> Data = new List<string>();

        public override bool IsApplicable()
        {
            return true;
        }

        public override void ReadData()
        {
            WebClient wb = new WebClient();
            for (int x = 0; x < 1000; x++)
            {
                Data.Add(wb.DownloadString($"https://reqres.in/api/users?page={x}"));
                Logger.WriteInformation($"Doing request {x}");
            }
        }

        public override void SaveData()
        {
           
            Thread.Sleep(2000);
        }

        public override void Verify()
        {
            foreach (var str in Data)
            {
                Logger.WriteInformation($"Lenght {str.Length}");
            }

        }
    }
   
}
