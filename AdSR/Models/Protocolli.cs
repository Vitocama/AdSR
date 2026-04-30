using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AdSR.Models
{
    public partial class Protocolli
    {
        // ═══════════════════════════════════════════════════════
        // DATI IDENTIFICATIVI
        // ═══════════════════════════════════════════════════════
        public string Ditta { get; set; }
        public string Marsau { get; set; }
        public string MarsauData { get; set; }       // MARSAU_DATA
        public string Rdo { get; set; }
        public string Argomento { get; set; }        // ARGOMENTO (NUOVO)
        public string Atto { get; set; }
        public string Contratto { get; set; }
        public string DataIntroduzione { get; set; }

        // ═══════════════════════════════════════════════════════
        // RICHIESTA E IMPORTI
        // ═══════════════════════════════════════════════════════
        public string Richiesta { get; set; }
        public string Lotto { get; set; }
        public string Importo { get; set; }
        public string Giorni { get; set; }
        public string Ric { get; set; }              // RIC (era Ric)

        // ═══════════════════════════════════════════════════════
        // FORNITURE E SISTEMA
        // ═══════════════════════════════════════════════════════
        public string Forniture { get; set; }
        public string Siga45 { get; set; }

        // ═══════════════════════════════════════════════════════
        // CRE / DDT / JOSH
        // ═══════════════════════════════════════════════════════
        public string Cre { get; set; }
        public string DataCre { get; set; }
        public string Josh { get; set; }
        public string Ddt { get; set; }              // DDT (era Ddt)
        public string DataDdt { get; set; }          // DATA_DDT (era DataDdt)

        // ═══════════════════════════════════════════════════════
        // FATTURAZIONE
        // ═══════════════════════════════════════════════════════
        public string Fattura { get; set; }
        public string DataFattura { get; set; }
        public string ImportoFattura { get; set; }

        // ═══════════════════════════════════════════════════════
        // BDC / COLLAUDO
        // ═══════════════════════════════════════════════════════
        public string Bdc { get; set; }
        public string DataBdc { get; set; }
        public string Collaudo { get; set; }

        // ═══════════════════════════════════════════════════════
        // ALTRO
        // ═══════════════════════════════════════════════════════
        public string PdC { get; set; }
        public string Cartella { get; set; }
        public string Note { get; set; }
    }
}