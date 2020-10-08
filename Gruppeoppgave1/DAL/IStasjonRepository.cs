using Gruppeoppgave1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.DAL
{
    public interface IStasjonRepository
    {
        Task<List<Stasjoner>> HentAlleStasjoner();
        Task<Stasjoner> HentEnStasjon(int id);
        Task<bool> LagStasjon(Stasjoner stasjon);
        Task<bool> EndreStasjon(Stasjoner stasjon);
        Task<bool> SlettStasjon(int id);
    }
}
