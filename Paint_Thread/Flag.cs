using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paint_Thread
{
    public class Flag
    {
        private static int inc = 0;
        public int Inc => inc;

        public void Increment()
        {
            inc++;
        }
        public void Check()
        {
            if (inc == 3)
            {
                lock (this)
                {
                    Monitor.PulseAll(this);
                    inc = 0;
                }
            }
        }
    }
}
