using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppArvoredo
{
    public partial class TelaOrcamento : Form
    {
        public TelaOrcamento()
        {
            InitializeComponent();
        }

        private void TelaOrcamento_Load(object sender, EventArgs e)
        {
            DateTime lbldata = DateTime.Now;
            string data = lbldata.ToString("dd/MM/yyyy");
        }
    }
}
