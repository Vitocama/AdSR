using AdSR.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdSR.ServiceRepository
{
    public class ProtocolliService : IProtocolliRepository
    {
        public List<Protocolli> ProtocolliGetAll()
        {
            try
            {
                var db = new AdSRContext();
                var protocolli = db.Protocolli.ToList();
                return protocolli;
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nel recupero dei protocolli: {ex.Message}", ex);
            }
        }

        public Protocolli ProtocolliGetById(int id)
        {
            try
            {
                var db = new AdSRContext();
                // Protocolli non ha proprietà Id, quindi restituisce il primo record
                return db.Protocolli.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nel recupero del protocollo: {ex.Message}", ex);
            }
        }
        
        public void ProtocolliInsert(Protocolli protocollo)
        {
            try
            {
                if (protocollo == null)
                    throw new ArgumentNullException(nameof(protocollo), "Il protocollo non può essere nullo");

                var db = new AdSRContext();
                db.Protocolli.Add(protocollo);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nell'inserimento del protocollo: {ex.Message}", ex);
            }
        }

        public void ProtocolliUpdate(Protocolli protocollo)
        {
            if (protocollo == null)
                throw new ArgumentNullException(nameof(protocollo), "Il protocollo non può essere nullo");

            try
            {
                using (var db = new AdSRContext())
                {
                    var existing = db.Protocolli.FirstOrDefault(p => p.Marsau == protocollo.Marsau);

                    if (existing == null)
                        throw new Exception($"Protocollo con Marsau '{protocollo.Marsau}' non trovato");

                    existing.Ditta = protocollo.Ditta;
                    existing.Rdo = protocollo.Rdo;
                    existing.Atto = protocollo.Atto;
                    existing.Contratto = protocollo.Contratto;
                    existing.Richiesta = protocollo.Richiesta;
                    existing.Importo = protocollo.Importo;
                    existing.Giorni = protocollo.Giorni;
          
                    existing.Siga45 = protocollo.Siga45;
                    existing.Forniture = protocollo.Forniture;
                    existing.Note = protocollo.Note;
                    existing.Cre = protocollo.Cre;
                    existing.Ddt= protocollo.Ddt;
                    existing.DataIntroduzione = protocollo.DataIntroduzione;
                    existing.Fattura = protocollo.Fattura;
                    existing.ImportoFattura = protocollo.ImportoFattura;
                    existing.Bdc = protocollo.Bdc;
                    existing.Collaudo = protocollo.Collaudo;
                    existing.Cartella=protocollo.Cartella;
                    existing.Argomento = protocollo.Argomento;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nell'aggiornamento del protocollo: {ex.Message}", ex);
            }
        }

        public void ProtocolliDelete(int id)
        {
            try
            {
                var db = new AdSRContext();
                var protocollo = db.Protocolli.FirstOrDefault();
                if (protocollo == null)
                    throw new Exception("Protocollo non trovato");

                db.Protocolli.Remove(protocollo);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nell'eliminazione del protocollo: {ex.Message}", ex);
            }
        }
    }
}