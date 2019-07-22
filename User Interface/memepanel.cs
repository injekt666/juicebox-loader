﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace asshook_loader
{
    public partial class memepanel : UserControl
    {
        Color cl0 = Color.BlueViolet, cl1 = Color.Teal;
        Color cl2 = Color.Aqua;
        int wh = 20;
        float ang = 45;

        Timer timer = new Timer();

        public Color color0
        {
            get { return cl0; }
            set { cl0 = value; Invalidate(); }
        }

        public Color color1
        {
            get { return cl1; }
            set { cl1 = value; Invalidate(); }
        }


        public Color color2
        {
            get { return cl2; }
            set { cl2 = value; Invalidate(); }
        }


        public memepanel()
        {
            DoubleBuffered = true;
            timer.Interval = 100;
            timer.Start();
            timer.Tick += (s, e) => { angle = angle % 360 + 1; };
        }

        public float angle
        {
            get { return ang; }
            set { ang = value; Invalidate(); }
        }

        public int borderRadius
        {
            get { return wh; }
            set { wh = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            GraphicsPath gp = new GraphicsPath();

            gp.AddArc(new Rectangle(0, 0, wh, wh), 180, 90);
            gp.AddArc(new Rectangle(Width - wh, 0, wh, wh), -90, 90);
            gp.AddArc(new Rectangle(Width - wh, Height - wh, wh, wh), 0, 90);
            gp.AddArc(new Rectangle(0, Height - wh, wh, wh), 90, 90);
            e.Graphics.FillPath(new LinearGradientBrush(ClientRectangle, cl0, cl1, ang), gp);
            base.OnPaint(e);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // memepanel
            // 
            this.Name = "memepanel";
            this.Load += new System.EventHandler(this.Memepanel_Load_1);
            this.ResumeLayout(false);

        }

        private void Memepanel_Load_1(object sender, EventArgs e)
        {

        }

        private void memepanel_Load(object sender, EventArgs e)
        {

        }
    }
}
