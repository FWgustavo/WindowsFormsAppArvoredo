using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppArvoredo
{
    public class Madeira
    {
        public int id { get; set; }
        public string nome { get; set; }
        public bool ativo { get; set; }
        public int? fornecedorId { get; set; }
    }
}
