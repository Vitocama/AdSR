using AdSR.Common;
using AdSR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AdSR.ServiceRepository
{
    public class ProtocolliService : IProtocolliRepository
    {
        // ═══ LETTURA ═══

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
                MostraErrore("Errore nel recupero dei protocolli", ex);
                throw;
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
                MostraErrore("Errore nel recupero del protocollo", ex);
                throw;
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
                MostraErrore($"Errore nel recupero del protocollo '{marsau}'", ex);
                throw;
            }
        }

        // ═══ SCRITTURA ═══

        public void ProtocolliInsert(Protocolli protocollo)
        {
            if (protocollo == null)
            {
                MarinaMessageBox.Show("Il protocollo da inserire non è valido.",
                    "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                throw new ArgumentNullException(nameof(protocollo), "Il protocollo non può essere nullo");
            }

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
                MostraErrore("Errore nell'inserimento del protocollo", ex);
                throw;
            }
        }

        public void ProtocolliUpdate(Protocolli protocollo)
        {
            if (protocollo == null)
            {
               MarinaMessageBox.Show("Il protocollo da aggiornare non è valido.",
                    "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                throw new ArgumentNullException(nameof(protocollo), "Il protocollo non può essere nullo");
            }

            try
            {
                using (var db = new AdSRContext())
                {
                    var existing = db.Protocolli.FirstOrDefault(p => p.Marsau == protocollo.Marsau);

                    if (existing == null)
                        throw new InvalidOperationException($"Protocollo con Marsau '{protocollo.Marsau}' non trovato");

                    // Copia TUTTE le colonne del modello
                    db.Entry(existing).CurrentValues.SetValues(protocollo);

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MostraErrore("Errore nell'aggiornamento del protocollo", ex);
                throw;
            }
        }

        public void ProtocolliDelete(int id)
        {
            // Protocolli non ha un Id numerico: bloccato, usare ProtocolliDeleteByMarsau
            const string msg = "Protocolli non ha un Id numerico: usare ProtocolliDeleteByMarsau(marsau).";
            MarinaMessageBox.Show(msg, "Operazione non supportata",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            throw new NotSupportedException(msg);
        }

        public void ProtocolliDeleteByMarsau(string marsau)
        {
            var risposta = MarinaMessageBox.Show(
                $"Eliminare definitivamente il protocollo '{marsau}'?\n\nL'operazione non è reversibile.",
                "Conferma eliminazione", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (risposta != MessageBoxResult.Yes)
                return;

            try
            {
                using (var db = new AdSRContext())
                {
                    var protocollo = db.Protocolli.FirstOrDefault(p => p.Marsau == marsau);
                    if (protocollo == null)
                        throw new InvalidOperationException($"Protocollo con Marsau '{marsau}' non trovato");

                    db.Protocolli.Remove(protocollo);
                    db.SaveChanges();
                }

                MarinaMessageBox.Show($"Protocollo '{marsau}' eliminato.",
                    "AdSR", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MostraErrore("Errore nell'eliminazione del protocollo", ex);
                throw;
            }
        }

        // ═══ SUPPORTO ═══

        private static void MostraErrore(string operazione, Exception ex)
        {
           MarinaMessageBox.Show($"{operazione}:\n\n{ex.GetBaseException().Message}",
                "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}