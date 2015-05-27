using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Airport_Simulator
{
    public partial class Form1 : Form
    {
        // GUI 
        private Panel pnlTerminal1, pnlTerminal2, pnlTerminal3, pnlTaxiway1, pnlTaxiway2, pnlTaxiway3, pnlTaxiway4, pnlTaxiway5;
        private Panel pnlRunway, pnlArrivals, pnlButtons;
        private Button btnGeneratePlane, btnRelease1, btnRelease2, btnRelease3;
        private RadioButton rBtn0, rBtn1, rBtn2, rBtn3;


        private PanelController buttonArrival, buttonPanelTerminal1, buttonPanelTerminal2, buttonPanelTerminal3;

        private Semaphore semaphoreArrival;
        private Semaphore semaphoreRunway;
        private Semaphore semaphoreTaxiway1;
        private Semaphore semaphoreTaxiway2;
        private Semaphore semaphoreTaxiway3;
        private Semaphore semaphoreTaxiway4;
        private Semaphore semaphoreTaxiway5;
        private Semaphore semaphoreTerminal1;
        private Semaphore semaphoreTerminal2;
        private Semaphore semaphoreTerminal3;

        private Buffer bufferArrival;
        private Buffer bufferRunway;
        private Buffer bufferTaxiway1;
        private Buffer bufferTaxiway2;
        private Buffer bufferTaxiway3;
        private Buffer bufferTaxiway4;
        private Buffer bufferTaxiway5;
        private Buffer bufferTerminal1;
        private Buffer bufferTerminal2;
        private Buffer bufferTerminal3;

        private PanelController waitRunway;
        private PanelController waitTaxiway1;
        private PanelController waitTaxiway2;
        private PanelController waitTaxiway3;
        private PanelController waitTaxiway4;
        private PanelController waitTaxiway5;

        public Form1()
        {
            // Interface Design
            InitializeComponent();

            // Create Semaphore objects
            semaphoreArrival  = new Semaphore();
            semaphoreRunway   = new Semaphore();
            semaphoreTaxiway1 = new Semaphore();
            semaphoreTaxiway2 = new Semaphore();
            semaphoreTaxiway3 = new Semaphore();
            semaphoreTaxiway4 = new Semaphore();
            semaphoreTaxiway5 = new Semaphore();
            semaphoreTerminal1   = new Semaphore();
            semaphoreTerminal2   = new Semaphore();
            semaphoreTerminal3   = new Semaphore();

            // Create Buffer objects
            bufferArrival  = new Buffer(); 
            bufferRunway   = new Buffer();
            bufferTaxiway1 = new Buffer();
            bufferTaxiway2 = new Buffer();
            bufferTaxiway3 = new Buffer();
            bufferTaxiway4 = new Buffer();
            bufferTaxiway5 = new Buffer();
            bufferTerminal1   = new Buffer();
            bufferTerminal2   = new Buffer();
            bufferTerminal3   = new Buffer();

            // Create and initialize ButtonPanel objects for all the panels with a button
            buttonArrival = new ButtonPanelControlller(pnlArrivals, new Point(30, 20), 50, 4, false, true, true, semaphoreArrival, semaphoreRunway, semaphoreTaxiway1, null, bufferRunway, btnGeneratePlane, rBtn0, rBtn1, rBtn2, rBtn3);
            buttonPanelTerminal1 = new ButtonPanelControlller(pnlTerminal1, new Point(20, 20), 50, 16, true, false, false, semaphoreTerminal1, semaphoreTaxiway2, null, bufferTerminal1, bufferTaxiway2, btnRelease1, null, null, null, null);
            buttonPanelTerminal2 = new ButtonPanelControlller(pnlTerminal2, new Point(20, 20), 50, 16, true, false, false, semaphoreTerminal2, semaphoreTaxiway3, null, bufferTerminal2, bufferTaxiway3, btnRelease2, null, null, null, null);
            buttonPanelTerminal3 = new ButtonPanelControlller(pnlTerminal3, new Point(20, 20), 50, 16, true, false, false, semaphoreTerminal3, semaphoreTaxiway4, null, bufferTerminal3, bufferTaxiway4, btnRelease3, null, null, null, null);

            // Initialize WaitPanel objects for all waitpanels
            waitRunway   = new WaitPanelController(pnlRunway, new Point(725, 20), 50, 75, false, true, semaphoreRunway, semaphoreTaxiway1, null, bufferRunway, bufferTaxiway1, null, -1);
            waitTaxiway1 = new WaitPanelController(pnlTaxiway1, new Point(20, 180), 50, 17, false, false, semaphoreTaxiway1, semaphoreTaxiway2, semaphoreTerminal1, bufferTaxiway1, bufferTaxiway2, null, 0);
            waitTaxiway2 = new WaitPanelController(pnlTaxiway2, new Point(10, 20), 50, 17, true, true, semaphoreTaxiway2, semaphoreTaxiway3, semaphoreTerminal2, bufferTaxiway2, bufferTaxiway3, bufferTerminal1, 1);
            waitTaxiway3 = new WaitPanelController(pnlTaxiway3, new Point(10, 20), 50, 17, true, true, semaphoreTaxiway3, semaphoreTaxiway4, semaphoreTerminal3, bufferTaxiway3, bufferTaxiway4, bufferTerminal2, 2);
            waitTaxiway4 = new WaitPanelController(pnlTaxiway4, new Point(10, 20), 50, 17, true, true, semaphoreTaxiway4, semaphoreTaxiway5, null, bufferTaxiway4, bufferTaxiway5, bufferTerminal3, 3);
            waitTaxiway5 = new WaitPanelController(pnlTaxiway5, new Point(20, 0), 50, 23, true, false, semaphoreTaxiway5, semaphoreRunway, null, bufferTaxiway5, bufferRunway, null, 4);

            // Threads for buttonPanels
            new Thread(buttonArrival.Start).Start();
            new Thread(buttonPanelTerminal1.Start).Start();
            new Thread(buttonPanelTerminal2.Start).Start();
            new Thread(buttonPanelTerminal3.Start).Start();

            // Threads for waitPanels
            new Thread(waitRunway.Start).Start();
            new Thread(waitTaxiway1.Start).Start();
            new Thread(waitTaxiway2.Start).Start();
            new Thread(waitTaxiway3.Start).Start();
            new Thread(waitTaxiway4.Start).Start();
            new Thread(waitTaxiway5.Start).Start(); 
        }

        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "Airport Simulator - CB005083";

            this.pnlTerminal1 = new System.Windows.Forms.Panel();
            this.pnlTerminal2 = new System.Windows.Forms.Panel();
            this.pnlTerminal3 = new System.Windows.Forms.Panel();
            this.pnlTaxiway1 = new System.Windows.Forms.Panel();
            this.pnlTaxiway2 = new System.Windows.Forms.Panel();
            this.pnlTaxiway3 = new System.Windows.Forms.Panel();
            this.pnlTaxiway4 = new System.Windows.Forms.Panel();
            this.pnlTaxiway5 = new System.Windows.Forms.Panel();
            this.pnlRunway = new System.Windows.Forms.Panel();
            this.pnlArrivals = new System.Windows.Forms.Panel();
            this.btnGeneratePlane = new System.Windows.Forms.Button();
            this.btnRelease1 = new System.Windows.Forms.Button();
            this.btnRelease2 = new System.Windows.Forms.Button();
            this.btnRelease3 = new System.Windows.Forms.Button();
            this.rBtn0 = new System.Windows.Forms.RadioButton();
            this.rBtn1 = new System.Windows.Forms.RadioButton();
            this.rBtn2 = new System.Windows.Forms.RadioButton();
            this.rBtn3 = new System.Windows.Forms.RadioButton();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTerminal1
            // 
            this.pnlTerminal1.BackColor = System.Drawing.Color.DarkGray;
            this.pnlTerminal1.Location = new System.Drawing.Point(75, 75);
            this.pnlTerminal1.Name = "pnlTerminal1";
            this.pnlTerminal1.Size = new System.Drawing.Size(50, 200);
            this.pnlTerminal1.TabIndex = 0;
            // 
            // pnlTerminal2
            // 
            this.pnlTerminal2.BackColor = System.Drawing.Color.Gray;
            this.pnlTerminal2.Location = new System.Drawing.Point(275, 75);
            this.pnlTerminal2.Name = "pnlTerminal2";
            this.pnlTerminal2.Size = new System.Drawing.Size(50, 200);
            this.pnlTerminal2.TabIndex = 1;
            // 
            // pnlTerminal3
            // 
            this.pnlTerminal3.BackColor = System.Drawing.Color.DarkGray;
            this.pnlTerminal3.Location = new System.Drawing.Point(475, 75);
            this.pnlTerminal3.Name = "pnlTerminal3";
            this.pnlTerminal3.Size = new System.Drawing.Size(50, 200);
            this.pnlTerminal3.TabIndex = 2;
            // 
            // pnlTaxiway1
            // 
            this.pnlTaxiway1.BackColor = System.Drawing.Color.DarkGray;
            this.pnlTaxiway1.Location = new System.Drawing.Point(75, 325);
            this.pnlTaxiway1.Name = "pnlTaxiway1";
            this.pnlTaxiway1.Size = new System.Drawing.Size(50, 200);
            this.pnlTaxiway1.TabIndex = 3;
            // 
            // pnlTaxiway2
            // 
            this.pnlTaxiway2.BackColor = System.Drawing.Color.Gray;
            this.pnlTaxiway2.Location = new System.Drawing.Point(75, 275);
            this.pnlTaxiway2.Name = "pnlTaxiway2";
            this.pnlTaxiway2.Size = new System.Drawing.Size(200, 50);
            this.pnlTaxiway2.TabIndex = 4;
            // 
            // pnlTaxiway3
            // 
            this.pnlTaxiway3.BackColor = System.Drawing.Color.DarkGray;
            this.pnlTaxiway3.Location = new System.Drawing.Point(275, 275);
            this.pnlTaxiway3.Name = "pnlTaxiway3";
            this.pnlTaxiway3.Size = new System.Drawing.Size(200, 50);
            this.pnlTaxiway3.TabIndex = 5;
            // 
            // pnlTaxiway4
            // 
            this.pnlTaxiway4.BackColor = System.Drawing.Color.Gray;
            this.pnlTaxiway4.Location = new System.Drawing.Point(475, 275);
            this.pnlTaxiway4.Name = "pnlTaxiway4";
            this.pnlTaxiway4.Size = new System.Drawing.Size(200, 50);
            this.pnlTaxiway4.TabIndex = 6;
            // 
            // pnlTaxiway5
            // 
            this.pnlTaxiway5.BackColor = System.Drawing.Color.DarkGray;
            this.pnlTaxiway5.Location = new System.Drawing.Point(675, 275);
            this.pnlTaxiway5.Name = "pnlTaxiway5";
            this.pnlTaxiway5.Size = new System.Drawing.Size(50, 250);
            this.pnlTaxiway5.TabIndex = 7;
            // 
            // pnlRunway
            // 
            this.pnlRunway.BackColor = System.Drawing.Color.Gray;
            this.pnlRunway.Location = new System.Drawing.Point(0, 525);
            this.pnlRunway.Name = "pnlRunway";
            this.pnlRunway.Size = new System.Drawing.Size(725, 50);
            this.pnlRunway.TabIndex = 8;
            // 
            // pnlArrivals
            // 
            this.pnlArrivals.BackColor = System.Drawing.Color.DarkGray;
            this.pnlArrivals.Location = new System.Drawing.Point(725, 525);
            this.pnlArrivals.Name = "pnlArrivals";
            this.pnlArrivals.Size = new System.Drawing.Size(150, 50);
            this.pnlArrivals.TabIndex = 9;
            // 
            // btnGeneratePlane
            // 
            this.btnGeneratePlane.BackColor = System.Drawing.Color.Pink;
            this.btnGeneratePlane.Location = new System.Drawing.Point(875, 525);
            this.btnGeneratePlane.Name = "btnGeneratePlane";
            this.btnGeneratePlane.Size = new System.Drawing.Size(50, 50);
            this.btnGeneratePlane.TabIndex = 10;
            this.btnGeneratePlane.UseVisualStyleBackColor = false;
            // 
            // btnRelease1
            // 
            this.btnRelease1.BackColor = System.Drawing.Color.Green;
            this.btnRelease1.Location = new System.Drawing.Point(75, 25);
            this.btnRelease1.Name = "btnRelease1";
            this.btnRelease1.Size = new System.Drawing.Size(50, 50);
            this.btnRelease1.TabIndex = 11;
            this.btnRelease1.UseVisualStyleBackColor = false;
            // 
            // btnRelease2
            // 
            this.btnRelease2.BackColor = System.Drawing.Color.Green;
            this.btnRelease2.Location = new System.Drawing.Point(275, 25);
            this.btnRelease2.Name = "btnRelease2";
            this.btnRelease2.Size = new System.Drawing.Size(50, 50);
            this.btnRelease2.TabIndex = 12;
            this.btnRelease2.UseVisualStyleBackColor = false;
            // 
            // btnRelease3
            // 
            this.btnRelease3.BackColor = System.Drawing.Color.Green;
            this.btnRelease3.Location = new System.Drawing.Point(475, 25);
            this.btnRelease3.Name = "btnRelease3";
            this.btnRelease3.Size = new System.Drawing.Size(50, 50);
            this.btnRelease3.TabIndex = 13;
            this.btnRelease3.UseVisualStyleBackColor = false;
            // 
            // rBtn0
            // 
            this.rBtn0.Checked = true;
            this.rBtn0.Location = new System.Drawing.Point(0, 0);
            this.rBtn0.Name = "rBtn0";
            this.rBtn0.Size = new System.Drawing.Size(104, 24);
            this.rBtn0.TabIndex = 0;
            this.rBtn0.TabStop = true;
            this.rBtn0.Text = "0";
            // 
            // rBtn1
            // 
            this.rBtn1.Location = new System.Drawing.Point(0, 25);
            this.rBtn1.Name = "rBtn1";
            this.rBtn1.Size = new System.Drawing.Size(104, 24);
            this.rBtn1.TabIndex = 1;
            this.rBtn1.Text = "1";
            // 
            // rBtn2
            // 
            this.rBtn2.Location = new System.Drawing.Point(0, 50);
            this.rBtn2.Name = "rBtn2";
            this.rBtn2.Size = new System.Drawing.Size(104, 24);
            this.rBtn2.TabIndex = 2;
            this.rBtn2.Text = "2";
            // 
            // rBtn3
            // 
            this.rBtn3.Location = new System.Drawing.Point(0, 75);
            this.rBtn3.Name = "rBtn3";
            this.rBtn3.Size = new System.Drawing.Size(104, 24);
            this.rBtn3.TabIndex = 3;
            this.rBtn3.Text = "3";
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.Silver;
            this.pnlButtons.Controls.Add(this.rBtn0);
            this.pnlButtons.Controls.Add(this.rBtn1);
            this.pnlButtons.Controls.Add(this.rBtn2);
            this.pnlButtons.Controls.Add(this.rBtn3);
            this.pnlButtons.Location = new System.Drawing.Point(875, 400);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(50, 100);
            this.pnlButtons.TabIndex = 14;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(984, 641);
            this.Controls.Add(this.pnlTerminal1);
            this.Controls.Add(this.pnlTerminal2);
            this.Controls.Add(this.pnlTerminal3);
            this.Controls.Add(this.pnlTaxiway1);
            this.Controls.Add(this.pnlTaxiway2);
            this.Controls.Add(this.pnlTaxiway3);
            this.Controls.Add(this.pnlTaxiway4);
            this.Controls.Add(this.pnlTaxiway5);
            this.Controls.Add(this.pnlRunway);
            this.Controls.Add(this.pnlArrivals);
            this.Controls.Add(this.btnGeneratePlane);
            this.Controls.Add(this.btnRelease1);
            this.Controls.Add(this.btnRelease2);
            this.Controls.Add(this.btnRelease3);
            this.Controls.Add(this.pnlButtons);
            this.Name = "Form1";
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    } // Form

}
