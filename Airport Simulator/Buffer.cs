using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Airport_Simulator
{
    public class Buffer
    {
        private Plane plane;
        private bool empty = true;

        public void read(ref Plane plane)
        {
            lock (this)
            {
                if (empty)
                {
                    Monitor.Wait(this);
                    empty = true;
                    plane = this.plane;
                    Monitor.Pulse(this);
                }
            }
        }

        public void write(Plane plane)
        {
            lock (this)
            {
                if (!empty)
                    Monitor.Wait(this);
                empty = false;
                this.plane = plane;
                Monitor.Pulse(this);

            }
        }
    }
}
