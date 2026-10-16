using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSR.Models
{
    public class DocumentoPdf
    {
        public string Nome { get; set; }
        public string Percorso { get; set; }
        public string Sottocartella { get; set; }   // relativa alla cartella del Marsau
        public DateTime DataModifica { get; set; }
    }
}
