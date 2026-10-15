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
                using (var db = new AdSRContext())
                {
                    return db.Protocolli.ToList();
                }
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
                using (var db = new AdSRContext())
                {
                    // ATTENZIONE: Protocolli non ha un Id numerico (la chiave è MARSAU),
                    // quindi qui viene restituito solo il primo record. Usare ProtocolliGetByMarsau.
                    return db.Protocolli.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nel recupero del protocollo: {ex.Message}", ex);
            }
        }

        public Protocolli ProtocolliGetByMarsau(string marsau)
        {
            try
            {
                using (var db = new AdSRContext())
                {
                    return db.Protocolli.FirstOrDefault(p => p.Marsau == marsau);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nel recupero del protocollo: {ex.Message}", ex);
            }
        }

        public void ProtocolliInsert(Protocolli protocollo)
        {
            if (protocollo == null)
                throw new ArgumentNullException(nameof(protocollo), "Il protocollo non può essere nullo");

            try
            {
                using (var db = new AdSRContext())
                {
                    db.Protocolli.Add(protocollo);
                    db.SaveChanges();
                }
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

                    // Copia TUTTE le colonne del modello (prima ne venivano aggiornate solo alcune
                    // e le modifiche a Lotto, Capitolo, RUP, DEC, date, PdG, ecc. andavano perse).
                    db.Entry(existing).CurrentValues.SetValues(protocollo);

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
            // ATTENZIONE: la versione precedente cancellava il PRIMO record della tabella,
            // qualunque fosse l'id passato. Bloccato finché non si usa ProtocolliDeleteByMarsau.
            throw new NotSupportedException(
                "Protocolli non ha un Id numerico: usare ProtocolliDeleteByMarsau(marsau).");
        }

        public void ProtocolliDeleteByMarsau(string marsau)
        {
            try
            {
                using (var db = new AdSRContext())
                {
                    var protocollo = db.Protocolli.FirstOrDefault(p => p.Marsau == marsau);
                    if (protocollo == null)
                        throw new Exception($"Protocollo con Marsau '{marsau}' non trovato");

                    db.Protocolli.Remove(protocollo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nell'eliminazione del protocollo: {ex.Message}", ex);
            }
        }
    }
}