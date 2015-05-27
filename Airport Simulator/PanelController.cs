using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Airport_Simulator
{
    public abstract class PanelController
    {
        protected Panel panel;
        protected Point originPoint;
        protected int delay;
        protected int length;
        protected bool isMovingPositiveAxis;
        protected Semaphore semaphoreThis;
        protected Semaphore semaphoreNext;
        protected Semaphore semaphoreWait;
        protected Buffer bufferThis;
        protected Buffer bufferNext;
        protected int xDelta = 0;
        protected int yDelta = 0;
        protected Plane plane = null;

        public PanelController(Panel panel, Point originPoint, int delay, int length, bool isMovingPositiveAxis, bool isHorizontal,
            Semaphore semaphoreThis, Semaphore semaphoreNext, Semaphore semaphoreWait, Buffer bufferThis, Buffer bufferNext)
        {
            // Set the instance variables from constructor args
            this.panel = panel;
            this.originPoint = originPoint;
            this.delay = delay;
            this.length = length;
            this.isMovingPositiveAxis = isMovingPositiveAxis;
            this.semaphoreThis = semaphoreThis;
            this.semaphoreNext = semaphoreNext;
            this.semaphoreWait = semaphoreWait;
            this.bufferThis = bufferThis;
            this.bufferNext = bufferNext;

            // Calculate delta based on panel's orientation and moving direction
            if (isHorizontal)
                this.xDelta = this.isMovingPositiveAxis ? +10 : -10;
            else
                this.yDelta = this.isMovingPositiveAxis ? +10 : -10;
        }

        public abstract void Start();
        protected abstract void panelPaint(object sender, PaintEventArgs e);
    }
}
