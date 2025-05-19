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
    public partial class TelaArvoredo : Form
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
        public TelaArvoredo()
        {
            InitializeComponent();

            
            this.Paint += new PaintEventHandler(Form1_Paint);

            
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);



            
            this.Text = "Sistema Arvoredo";
        }

        private void SetBackColorDegrade(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Rectangle gradient_rect = new Rectangle(0, 0, Width, Height);

            using (LinearGradientBrush br = new LinearGradientBrush(
                gradient_rect,
                Color.FromArgb(250, 230, 194),
                Color.FromArgb(180, 123, 57),
                90f))
            {
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
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            SetBackColorDegrade(sender, e);
        }

        private void TelaArvoredo_Load(object sender, EventArgs e)
        {
            btnEstoque.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnEstoque.Width, btnEstoque.Height, 50, 100));
            btnOrcamento.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnOrcamento.Width, btnOrcamento.Height, 50, 100));
            btnPedidos.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnPedidos.Width, btnPedidos.Height, 50, 100));
            btnTitulos.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnTitulos.Width, btnTitulos.Height, 50, 100));
        }
    }
}
