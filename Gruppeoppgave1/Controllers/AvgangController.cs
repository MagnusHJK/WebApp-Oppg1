using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;
using Gruppeoppgave1.DAL;

namespace Gruppeoppgave1.Controllers
{
    [Route("[controller]/[action]")]
    public class AvgangController : ControllerBase
    {
        private readonly IAvgangRepository _db;
        public AvgangController(IAvgangRepository db)
        {
            _db = db;
        }

        public async Task<bool> SjekkAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            return await _db.SjekkAvganger(stasjonFraId, stasjonTilId, dato);
        }

        public async Task<bool> GenererAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            return await _db.GenererAvganger(stasjonFraId, stasjonTilId, dato);
        }

        public async Task<List<Avgang>> HentAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            return await _db.HentAvganger(stasjonFraId, stasjonTilId, dato);
        }
    }
}
