﻿using Gruppeoppgave1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.DAL
{
    public interface IAvgangRepository
    {
        Task<bool> SjekkAvganger(int stasjonFraId, int stasjonTilId, string dato);
        Task<bool> GenererAvganger(int stasjonFraId, int stasjonTilId, string dato);
        Task<List<Avgang>> HentAvganger(int stasjonFraId, int stasjonTilId, string dato);

    }
}
