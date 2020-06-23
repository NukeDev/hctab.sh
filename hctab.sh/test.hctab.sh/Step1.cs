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
    class Step1 : BatchStep, IBatchStep
    {
        private List<Byte> Data { get; set; }

        public override bool IsApplicable()
        {
            return true;
        }

        public override void ReadData()
        {
            Logger.WriteError("DEADLOCK!");
            Thread.Sleep(2000);
        }

        public override void SaveData()
        {
           
            Thread.Sleep(2000);
        }

        public override void Verify()
        {
           
            Logger.WriteInformation("WOW!");
            Thread.Sleep(2000);

        }
    }
   
}
