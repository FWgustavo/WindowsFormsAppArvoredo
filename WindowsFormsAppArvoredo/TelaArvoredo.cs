using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class TelaArvoredo : Form
    {
        public TelaArvoredo()
        {
            InitializeComponent();
        }

        private void SetBackColorDegrade(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            Rectangle gradient_rect = new Rectangle(0, 0, Width, Height);


            LinearGradientBrush br = new LinearGradientBrush
            (
                gradient_rect,
                Color.FromArgb(250, 230, 194),
                Color.FromArgb(180, 123, 57),
                90f
            );


            ColorBlend colorBlend = new ColorBlend(3);
            colorBlend.Colors = new Color[]
            {
               Color.FromArgb(250, 230, 194),
               Color.FromArgb(198, 143, 86),
               Color.FromArgb(180, 123, 57)
            };
            colorBlend.Positions = new float[] { 0f, 0.5f, 1f };


            br.InterpolationColors = colorBlend;


            graphics.FillRectangle(br, gradient_rect);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            SetBackColorDegrade(sender, e);
        }
    }
}
