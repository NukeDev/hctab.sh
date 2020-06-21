using core.hctab.sh.Batch;
using core.hctab.sh.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test.hctab.sh
{
    class LoadFiles : BatchStep, IBatchStep
    {
        private List<Byte> Data { get; set; }

        public override bool IsApplicable()
        {
            return true;
        }

        public override void ReadData()
        {
            Console.WriteLine("Reading data...");
            Thread.Sleep(2000);
        }

        public override void SaveData()
        {
            Console.WriteLine("Saving data...");
            Thread.Sleep(2000);
        }

        public override void Verify()
        {
            Console.WriteLine("Verifing data...");
            Logger.WriteInformation("WOW!");
            Thread.Sleep(2000);

        }
    }
   
}
