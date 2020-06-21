using core.hctab.sh.Batch;
using core.hctab.sh.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.hctab.sh
{
    class Program
    {
        static void Main(string[] args)
        {
            var batch = new core.hctab.sh.Batch.Batch();
            batch.Init("Config.json");
            Console.Read();
        }
    }
}
