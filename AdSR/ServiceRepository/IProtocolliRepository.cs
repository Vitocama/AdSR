using AdSR.Models;
using System;
using System.Collections.Generic;

namespace AdSR.ServiceRepository
{
    public interface IProtocolliRepository
    {
        List<Protocolli> ProtocolliGetAll();
        Protocolli ProtocolliGetById(int id);
        void ProtocolliInsert(Protocolli protocollo);
        void ProtocolliUpdate(Protocolli protocollo);
        void ProtocolliDelete(int id);
    }
}