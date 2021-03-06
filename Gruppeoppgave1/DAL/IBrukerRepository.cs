﻿using Gruppeoppgave1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.DAL
{
    public interface IBrukerRepository
    {
        Task<bool> LoggInn(Bruker bruker);

        Task<List<Brukere>> HentAlleBrukere();

        Task<Brukere> LagGjesteBruker();

        Task<bool> SlettBruker(int brukerId);
    }
}
