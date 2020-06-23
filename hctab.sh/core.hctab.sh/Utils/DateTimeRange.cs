using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.hctab.sh.Utils
{
    public class DateTimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool Intersects(DateTimeRange test)
        {
            if (this.Start > this.End || test.Start > test.End)
                return true;

            if (this.Start == this.End || test.Start == test.End)
                return false; 

            if (this.Start == test.Start || this.End == test.End)
                return true; 

            if (this.Start < test.Start)
            {
                if (this.End > test.Start && this.End < test.End)
                    return true; 

                if (this.End > test.End)
                    return true; 
            }
            else
            {
                if (test.End > this.Start && test.End < this.End)
                    return true; 

                if (test.End > this.End)
                    return true;
            }

            return false;
        }
    }
}
