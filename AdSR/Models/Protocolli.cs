using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdSR.Models
{
    [Table("PROTOCOLLI")]
    public class Protocolli
    {
        // ══════════ DATI IDENTIFICATIVI ══════════
        [Key]
        [Column("MARSAU")]
        public string Marsau { get; set; }

        [Column("MARSAU_DATA")]
        public string MarsauData { get; set; }

        [Column("DITTA")]
        public string Ditta { get; set; }

        [Column("RDO")]
        public string Rdo { get; set; }

        [Column("ARGOMENTO")]
        public string Argomento { get; set; }

        [Column("ATTO")]
        public string Atto { get; set; }

        [Column("CONTRATTO")]
        public string Contratto { get; set; }

        [Column("DATA_INTRODUZIONE")]
        public string DataIntroduzione { get; set; }

        // ══════════ ATTI DI RIFERIMENTO ══════════
        [Column("AA")]
        public string AA { get; set; }

        [Column("DEC")]
        public string Dec { get; set; }

        [Column("RUP")]
        public string Rup { get; set; }

        [Column("AD")]
        public string Ad { get; set; }

        [Column("CAPITOLO")]
        public string Capitolo { get; set; }

        // ══════════ RICHIESTA E IMPORTI ══════════
        [Column("RICHIESTA")]
        public string Richiesta { get; set; }

        [Column("LOTTO")]
        public string Lotto { get; set; }

        [Column("IMPORTO")]
        public string Importo { get; set; }

        [Column("GIORNI")]
        public string Giorni { get; set; }

        [Column("RIC")]
        public string Ric { get; set; }

        // ══════════ FORNITURE E SISTEMA ══════════
        [Column("FORNITURE")]
        public string Forniture { get; set; }

        [Column("SIGA45")]
        public string Siga45 { get; set; }

        // ══════════ CRE / DDT / JOSH ══════════
        [Column("CRE")]
        public string Cre { get; set; }

        [Column("DATA_CRE")]
        public string DataCre { get; set; }

        [Column("JOSH")]
        public string Josh { get; set; }

        [Column("DDT")]
        public string Ddt { get; set; }

        [Column("DATA_DDT")]
        public string DataDdt { get; set; }

        // ══════════ FATTURAZIONE ══════════
        [Column("FATTURA")]
        public string Fattura { get; set; }

        [Column("DATA_FATTURA")]
        public string DataFattura { get; set; }

        [Column("IMPORTO_FATTURA")]
        public string ImportoFattura { get; set; }

        // ══════════ BDC / COLLAUDO ══════════
        [Column("BDC")]
        public string Bdc { get; set; }

        [Column("DATA_BDC")]
        public string DataBdc { get; set; }

        [Column("COLLAUDO")]
        public string Collaudo { get; set; }

        // ══════════ ALTRO ══════════
        [Column("PdC")]
        public string PdC { get; set; }

        [Column("CARTELLA")]
        public string Cartella { get; set; }

        [Column("NOTE")]
        public string Note { get; set; }
    }
}