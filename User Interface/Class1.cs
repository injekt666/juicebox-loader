using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skeet_Menu
{
    class SkeetForm : Control
    {
        public Color ColorRight = Color.FromArgb(204, 227, 53);
        public Color ColorLeft = Color.FromArgb(55, 177, 218);
        public Color ColorMiddle = Color.FromArgb(204, 96, 180);

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle area = new Rectangle(5, 7, Width - 12, 2);

            LinearGradientBrush gradient = new LinearGradientBrush(area, this.ColorLeft, this.ColorMiddle, 0.0f);

            ColorBlend cblend = new ColorBlend(3);
            cblend.Colors = new Color[3] { this.ColorLeft, this.ColorMiddle, this.ColorRight };
            cblend.Positions = new float[3] { 0f, 0.5f, 1f };
            gradient.InterpolationColors = cblend;

            Graphics g = e.Graphics;
            g.FillRectangle(gradient, area);
            base.OnPaint(e);

            for (int i = 1; i < 7; i++)
            {
                Rectangle rect = new Rectangle(i - 1, i - 1, Width - (i * 2), Height - (i * 2));
                Pen lightGrey = new Pen(Color.FromArgb(60, 60, 60));
                Pen darkGrey = new Pen(Color.FromArgb(40, 40, 40));

                if (i == 1 || i == 7)
                    e.Graphics.DrawRectangle(Pens.Black, rect);

                if (i == 2 || i == 6)
                    e.Graphics.DrawRectangle(lightGrey, rect);

                if (i == 3 || i == 4 || i == 5)
                    e.Graphics.DrawRectangle(darkGrey, rect);
            }
        }
    }
}