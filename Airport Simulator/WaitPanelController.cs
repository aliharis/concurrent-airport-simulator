using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Airport_Simulator
{
    public class WaitPanelController: PanelController
    {
        private Buffer bufferTerminal;
        private int terminalId;

        public WaitPanelController(Panel panel, Point originPoint, int delay, int length, bool isMovingPositiveAxis, bool isHorizontal, Semaphore semaphoreThis, 
            Semaphore semaphoreNext, Semaphore semaphoreWait, Buffer bufferThis, Buffer bufferNext, Buffer bufferTerminal, int terminalId) :
            base(panel, originPoint, delay, length, isMovingPositiveAxis, isHorizontal, semaphoreThis, semaphoreNext, semaphoreWait, bufferThis, bufferNext)
        {
            // Assign the value of instance variables from constructor args
            this.bufferTerminal = bufferTerminal;
            this.terminalId = terminalId;

            // Panel paint function
            this.panel.Paint += new PaintEventHandler(this.panelPaint);       
        }

        public override void Start()
        {
            Thread.Sleep(delay);
            for (int k = 1; k <= 200; k++)
            {
                semaphoreThis.signal(); // Lock the current panel
                bufferThis.read(ref this.plane);

                // Set plane position to initial point of the panel
                if (this.plane != null)
                    this.plane.setPosition(this.originPoint.X, this.originPoint.Y);

                // Plane reaches the taxiway to which the destination terminal is attached to
                if (this.plane != null && new int[] {1,2,3}.Contains(this.terminalId) && this.terminalId == this.plane.getDestination())
                {
                    this.plane.movePlane(xDelta, yDelta);
                    panel.Invalidate();
                    Thread.Sleep(delay);
                    bufferTerminal.write(this.plane);
                    this.plane = null;
                    panel.Invalidate();
                }
                else
                {
                    try
                    {
                        // Move plane on runway
                        if (this.plane.getDestination() == 0 && terminalId == -1)
                        {
                            for (int i = 1; i <= this.length; i++)
                            {
                                this.plane.movePlane(xDelta, yDelta);
                                panel.Invalidate();
                                Thread.Sleep(delay);
                            }
                            this.plane = null;
                            panel.Invalidate();
                        }
                        else
                        {
                            if (terminalId == -1)
                            {
                                // Move panel on runway (on the way to taxiway)
                                int turningPoint = this.length - 12; // calculate turning point
                                for (int i = 1; i <= turningPoint; i++)
                                {

                                    this.plane.movePlane(xDelta, yDelta);
                                    panel.Invalidate();
                                    Thread.Sleep(delay);
                                }
                            }
                            else
                            {
                                // Move plane on wait panels
                                for (int i = 1; i < length; i++)
                                {
                                    panel.Invalidate();
                                    this.plane.movePlane(xDelta, yDelta);
                                    Thread.Sleep(delay);
                                }
                            }

                            // Write plane to terminal
                            if (this.plane.getDestination() != this.terminalId)
                            {
                                if (this.terminalId == 0 && this.plane.getDestination() == 1)
                                    semaphoreWait.wait();

                                if (this.terminalId == 1 && this.plane.getDestination() == 2)
                                    semaphoreWait.wait();

                                if (this.terminalId == 2 && this.plane.getDestination() == 3)
                                    semaphoreWait.wait();

                                if (this.terminalId != -1)
                                    semaphoreNext.wait();

                                bufferNext.write(this.plane);
                                this.plane = null;
                                panel.Invalidate();
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Caught exception: " + e.Message);
                    }
                }
            }
        }

        protected override void panelPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (plane != null)
            {
                SolidBrush brush = new SolidBrush(plane.getColor());
                g.FillRectangle(brush, plane.getPositionX(), plane.getPositionY(), 10, 10);
                SolidBrush blackBrush = new SolidBrush(Color.White);
                g.DrawString(plane.getDestination().ToString(), new Font("Arial", 7), blackBrush, new PointF((float)plane.getPositionX(), (float)plane.getPositionY()));
                brush.Dispose();
            }
            g.Dispose();
        }
    }
}
