using Gruppeoppgave1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.DAL
{
    public interface IBestillingRepository
    {
        Task<bool> LagBestilling(int avgangId, int antall);
        Task<List<Bestilling>> HentAlleBestillinger();

    }
}
