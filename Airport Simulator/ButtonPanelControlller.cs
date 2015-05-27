
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Airport_Simulator
{
    public class ButtonPanelControlller: PanelController
    {
        private bool isArrival;
        private Button mainBtn;
        private RadioButton rbtnTakeoff;
        private RadioButton rbtnTerminal1;
        private RadioButton rbtnTerminal2;
        private RadioButton rbtnTerminal3;
        private bool isLocked = true;

        public ButtonPanelControlller(Panel panel, Point originPoint, int delay, int length, bool isMovingPositiveAxis, bool isHorizontal, bool isArrival,
            Semaphore semaphoreThis, Semaphore semaphoreNext, Semaphore semaphoreWait, Buffer bufferThis, Buffer bufferNext, Button mainBtn,
            RadioButton rbtnTakeoff, RadioButton rbtnTerminal1, RadioButton rbtnTerminal2, RadioButton rbtnTerminal3):
            base(panel, originPoint, delay, length, isMovingPositiveAxis, isHorizontal, semaphoreThis, semaphoreNext, semaphoreWait, bufferThis, bufferNext)
        {

            // Assign the value of instance variables from constructor args
            this.isArrival = isArrival;
            this.mainBtn = mainBtn;
            this.rbtnTakeoff = rbtnTakeoff;
            this.rbtnTerminal1 = rbtnTerminal1;
            this.rbtnTerminal2 = rbtnTerminal2;
            this.rbtnTerminal3 = rbtnTerminal3;

            // If panel is assigned to arrivals, create a new instance of plane obj
            if (this.isArrival)
                this.plane = new Plane(new Point(0, 0), 0, this.getRandomColor());

            // Assign a method to mainBtn click action and panel paint method
            this.mainBtn.Click += new System.EventHandler(this.mainBtn_Click);
            this.panel.Paint += new PaintEventHandler(this.panelPaint);
        }

        public override void Start()
        {
            Thread.Sleep(delay);
            for (int k = 1; k <= 200; k++)
            {
                semaphoreThis.signal();

                // Check the availability of next panel
                if (this.plane != null)
                    semaphoreThis.wait();

                // Move plane on terminal (to park)
                if (this.plane == null && !isArrival)
                {
                    bufferThis.read(ref this.plane);    // Read the plane in terminal's buffer and set reference to this.plane

                    this.plane.setPosition(this.panel.Size.Width - 30, this.panel.Size.Height - 15); // Move plane to bottom of the terminal
                    panel.Invalidate();

                    for (int i = 1; i < length; i++)
                    {
                        this.plane.movePlane(this.xDelta, -this.yDelta);
                        Thread.Sleep(delay);
                        panel.Invalidate();
                    }

                    // After parking, set plane's next detination to take off
                    this.plane.setDestination(0);

                    this.isLocked = true; // Lock the tertminal
                    this.mainBtn.BackColor = this.isLocked ? Color.Pink : Color.Green;
                    lock (this)
                    {
                        if (!this.isLocked)
                            Monitor.Pulse(this);
                    }
                }

                this.plane.setPosition(this.originPoint.X, this.originPoint.Y); // Set plane to initial position of panel
                panel.Invalidate();

                lock (this)
                {
                    while (this.isLocked)
                    {
                        Monitor.Wait(this);
                    }
                }

                // Move the plane from the terminal once the terminal lock has been released
                for (int i = 1; i < length; i++)
                {
                    this.plane.movePlane(this.xDelta, this.yDelta);
                    Thread.Sleep(delay);
                    panel.Invalidate();
                }


                // When a new plane is generated and it's not a direct take off
                // Check the availability of taxiway 1 (to avoid blocking the runway)
                if (this.plane.getDestination() != 0 && isArrival)
                    this.semaphoreWait.wait();

                this.semaphoreNext.wait();
                bufferNext.write(this.plane);
                this.plane = null;
                panel.Invalidate();
                
                // Genereating new plane
                if (this.isArrival)
                {
                    this.isLocked = true;
                    this.mainBtn.BackColor = this.isLocked ? Color.Pink : Color.Green;
                    this.plane = new Plane(this.originPoint, 0, this.getRandomColor());
                    panel.Invalidate();
                }
            }
        }

        private void mainBtn_Click(object sender, System.EventArgs e)
        {
            this.isLocked = !this.isLocked;   // Unlock the resource only when the program wants to use it
            if (this.plane != null) // Toggle the button's color only If there's a plane occupying the panel
                this.mainBtn.BackColor = this.isLocked ? Color.Pink : Color.Green;

            // Check if button has been clicked from arrivals section 
            if (this.isArrival && this.plane.getPositionX() == this.originPoint.X)
            {
                // Read destination from radiobutton and set to plan
                this.plane.setDestination(0);
                if (rbtnTerminal1.Checked) this.plane.setDestination(1);
                if (rbtnTerminal2.Checked) this.plane.setDestination(2);
                if (rbtnTerminal3.Checked) this.plane.setDestination(3);
            }

            // Lock the resource while using
            lock (this)
                if (!isLocked) Monitor.Pulse(this);
        }

        protected override void panelPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (plane != null)
            {
                SolidBrush brush = new SolidBrush(plane.getColor());
                g.FillRectangle(brush, plane.getPositionX(), plane.getPositionY(), 10, 10);
                SolidBrush blackBrush = new SolidBrush(Color.White);
                try
                {
                    g.DrawString(plane.getDestination().ToString(), new Font("Helvetica", 7), blackBrush, new PointF((float)plane.getPositionX(), (float)plane.getPositionY()));
                }
                catch (Exception ae) { }
                brush.Dispose();
            }
            g.Dispose();
        }

        private Color getRandomColor()
        {
            Random random = new Random();
            return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }
    }
}
