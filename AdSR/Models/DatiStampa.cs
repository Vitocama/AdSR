using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdSR.Models
{
    public class DatiStampa
    {
        // Foglio Collaudo
        public string NumeroColli { get; set; }
        public bool Parziale { get; set; }
        public bool Completo { get; set; }
        public string DataIntroduzione { get; set; }

        // Foglio Trasmissione
        public string Indirizzo { get; set; }
        public string Giorni { get; set; }
        public string NomeFirmatario { get; set; }
    }
}
