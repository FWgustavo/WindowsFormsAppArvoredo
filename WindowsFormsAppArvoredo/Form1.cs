using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
            (
               int nLeft,
               int nTop,
               int nRight,
               int nBottom,
               int nWidthEllipse,
               int nHeightEllipse
            );
        public Form1()
        {
            InitializeComponent();
        }

        private void SetBackColorDegrade(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            Rectangle gradient_rect = new Rectangle(0, 0, Width, Height);

            Brush br = new LinearGradientBrush(gradient_rect, Color.FromArgb(250, 230, 194), Color.FromArgb(198, 143, 86), 90f);

            graphics.FillRectangle(br, gradient_rect);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            SetBackColorDegrade(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Btn_Login.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Btn_Login.Width, Btn_Login.Height, 100, 100));
            Btn_Config.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Btn_Config.Width, Btn_Config.Height, 100, 100));
        }

        private void Btn_Login_Click(object sender, EventArgs e)
        {
            
        }
    }
}
