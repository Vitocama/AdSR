using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AdSR.Models
{
    public partial class AdSRContext : DbContext
    {
        public AdSRContext()
        {
        }

        public AdSRContext(DbContextOptions<AdSRContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Protocolli> Protocolli { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(LeggiConnectionString());
            }
        }

        /// <summary>
        /// Legge il percorso del database dal file db.txt (nella cartella dell'eseguibile).
        /// Il file puo' contenere solo il percorso:
        ///     \\server\condivisa\AdSR.db
        /// oppure la stringa completa:
        ///     Data Source=\\server\condivisa\AdSR.db
        /// Righe vuote e righe che iniziano con # vengono ignorate.
        /// </summary>
        private static string LeggiConnectionString()
        {
            string fileConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db.txt");

            if (!File.Exists(fileConfig))
                throw new FileNotFoundException(
                    "File di configurazione del database non trovato:\n" + fileConfig +
                    "\n\nCreare il file db.txt con il percorso del database (es. \\\\server\\cartella\\AdSR.db).",
                    fileConfig);

            string riga = File.ReadAllLines(fileConfig)
                .Select(r => r.Trim())
                .FirstOrDefault(r => r.Length > 0 && !r.StartsWith("#"));

            if (string.IsNullOrWhiteSpace(riga))
                throw new InvalidOperationException("Il file db.txt e' vuoto:\n" + fileConfig);

            // Rimuove eventuali virgolette
            riga = riga.Trim('"');

            // Se c'e' gia' "Data Source=" la usa cosi' com'e', altrimenti la costruisce
            if (riga.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
                return riga;

            return "Data Source=" + riga;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Protocolli>(entity =>
            {
                entity.HasKey(e => e.Marsau);
                entity.ToTable("PROTOCOLLI");

                entity.HasIndex(e => e.Ric)
                    .IsUnique();

                entity.HasIndex(e => e.Siga45)
                    .IsUnique();

                // ═══════════════════════════════════════════════════════
                // DATI IDENTIFICATIVI
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.Ditta).HasColumnName("DITTA");
                entity.Property(e => e.Marsau).HasColumnName("MARSAU");
                entity.Property(e => e.MarsauData).HasColumnName("MARSAU_DATA");
                entity.Property(e => e.Rdo).HasColumnName("RDO");
                entity.Property(e => e.Argomento).HasColumnName("ARGOMENTO");
                entity.Property(e => e.Atto).HasColumnName("ATTO");
                entity.Property(e => e.Contratto).HasColumnName("CONTRATTO");
                entity.Property(e => e.DataIntroduzione).HasColumnName("DATA_INTRODUZIONE");

                // ═══════════════════════════════════════════════════════
                // ATTI DI RIFERIMENTO
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.AA).HasColumnName("AA");
                entity.Property(e => e.Dec).HasColumnName("DEC");
                entity.Property(e => e.Rup).HasColumnName("RUP");
                entity.Property(e => e.Ad).HasColumnName("AD");
                entity.Property(e => e.Capitolo).HasColumnName("CAPITOLO");

                // ═══════════════════════════════════════════════════════
                // RICHIESTA E IMPORTI
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.Richiesta).HasColumnName("RICHIESTA");
                entity.Property(e => e.Lotto).HasColumnName("LOTTO");
                entity.Property(e => e.Importo).HasColumnName("IMPORTO");
                entity.Property(e => e.Giorni).HasColumnName("GIORNI");
                entity.Property(e => e.Ric).HasColumnName("RIC");

                // ═══════════════════════════════════════════════════════
                // FORNITURE E SISTEMA
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.Forniture).HasColumnName("FORNITURE");
                entity.Property(e => e.Siga45).HasColumnName("SIGA45");
                entity.Property(e => e.FornituraParziale).HasColumnName("FORNITURA_PARZIALE");
                entity.Property(e => e.FornituraCompleta).HasColumnName("FORNITURA_COMPLETA");

                // ═══════════════════════════════════════════════════════
                // CRE / DDT / JOSH
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.Cre).HasColumnName("CRE");
                entity.Property(e => e.DataCre).HasColumnName("DATA_CRE");
                entity.Property(e => e.Josh).HasColumnName("JOSH");
                entity.Property(e => e.Ddt).HasColumnName("DDT");
                entity.Property(e => e.DataDdt).HasColumnName("DATA_DDT");
                entity.Property(e => e.Pervenuto).HasColumnName("DATA_PERVENUTO");
                entity.Property(e => e.Ddt2).HasColumnName("DDT_2");
                entity.Property(e => e.DataDdt2).HasColumnName("DATA_DDT_2");
                entity.Property(e => e.Pervenuto2).HasColumnName("DATA_PERVENUTO_2");
                entity.Property(e => e.Ddt3).HasColumnName("DDT_3");
                entity.Property(e => e.DataDdt3).HasColumnName("DATA_DDT_3");
                entity.Property(e => e.Pervenuto3).HasColumnName("DATA_PERVENUTO_3");
                entity.Property(e => e.Trasmesso).HasColumnName("TRASMESSO");

                // ═══════════════════════════════════════════════════════
                // FATTURAZIONE
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.Fattura).HasColumnName("FATTURA");
                entity.Property(e => e.DataFattura).HasColumnName("DATA_FATTURA");
                entity.Property(e => e.ImportoFattura).HasColumnName("IMPORTO_FATTURA");

                // ═══════════════════════════════════════════════════════
                // BDC / COLLAUDO
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.Bdc).HasColumnName("BDC");
                entity.Property(e => e.DataBdc).HasColumnName("DATA_BDC");
                entity.Property(e => e.Collaudo).HasColumnName("COLLAUDO");
                entity.Property(e => e.DataCollaudo).HasColumnName("DATA_COLLAUDO");

                // ═══════════════════════════════════════════════════════
                // PROTOCOLLO
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.Protocollo).HasColumnName("PROTOCOLLO");
                entity.Property(e => e.DataProtocollo).HasColumnName("DATA_PROTOCOLLO");

                // ═══════════════════════════════════════════════════════
                // ALTRO
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.PdC).HasColumnName("PdC");
                entity.Property(e => e.PdG).HasColumnName("PdG");
                entity.Property(e => e.Cartella).HasColumnName("CARTELLA");
                entity.Property(e => e.Note).HasColumnName("NOTE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}