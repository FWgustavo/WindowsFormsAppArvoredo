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

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.Text = "Sistema Arvoredo";
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

        private void CentralizarControles()
        {
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;
            pictureBox1.Left = (this.ClientSize.Width - pictureBox1.Width) / 2;
            Btn_Login.Left = (this.ClientSize.Width - Btn_Login.Width) / 2;
            Btn_Config.Left = (this.ClientSize.Width - Btn_Config.Width) / 2;
            pictureBox2.Left = (this.ClientSize.Width - pictureBox2.Width) / 2;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            CentralizarControles();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Btn_Login.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Btn_Login.Width, Btn_Login.Height, 100, 100));
            Btn_Config.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Btn_Config.Width, Btn_Config.Height, 100, 100));

        }

        private void Btn_Login_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin login = new FormLogin();
            login.ShowDialog();
        }
    }
}
