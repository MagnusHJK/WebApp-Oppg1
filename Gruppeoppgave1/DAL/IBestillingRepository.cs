using Gruppeoppgave1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.DAL
{
    public interface IBestillingRepository
    {
        Task<bool> LagBestilling(int avgangId, int antall, int brukerId);

        Task<bool> EndreBestilling(int bestillingId, int nyAvgangId, int nyttAntall);

        Task<bool> SlettBestilling(int bestillingId);

        Task<List<Bestillinger>> HentBestillinger(int brukerId);

        Task<List<Bestillinger>> HentAlleBestillinger();

    }
}
