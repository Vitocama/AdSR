using AdSR.Models;
using System.Collections.Generic;

namespace AdSR.ServiceRepository
{
    public interface IProtocolliRepository
    {
        List<Protocolli> ProtocolliGetAll();
        Protocolli ProtocolliGetById(int id);
        Protocolli ProtocolliGetByMarsau(string marsau);
        void ProtocolliInsert(Protocolli protocollo);
        void ProtocolliUpdate(Protocolli protocollo);
        void ProtocolliDelete(int id);
        void ProtocolliDeleteByMarsau(string marsau);
    }
}