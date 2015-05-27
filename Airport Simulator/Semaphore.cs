using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Airport_Simulator
{
    public class Semaphore
    {
        private int count = 0;

        public void wait()
        {
            lock (this)
            {
                while (count == 0)
                    Monitor.Wait(this);

                count = 0;
            }
        }

        public void signal()
        {
            lock (this)
            {
                count = 1;
                Monitor.Pulse(this);
            }
        }
    }
}
