using System;
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source=C:\\Users\\Hirad\\source\\repos\\AdSR\\AdSR\\AdSR.db");
            }
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

                // ═══════════════════════════════════════════════════════
                // CRE / DDT / JOSH
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.Cre).HasColumnName("CRE");
                entity.Property(e => e.DataCre).HasColumnName("DATA_CRE");
                entity.Property(e => e.Josh).HasColumnName("JOSH");
                entity.Property(e => e.Ddt).HasColumnName("DDT");
                entity.Property(e => e.DataDdt).HasColumnName("DATA_DDT");

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

                // ═══════════════════════════════════════════════════════
                // ALTRO
                // ═══════════════════════════════════════════════════════
                entity.Property(e => e.PdC).HasColumnName("PdC");
                entity.Property(e => e.Cartella).HasColumnName("CARTELLA");
                entity.Property(e => e.Note).HasColumnName("NOTE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}