using Gruppeoppgave1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Controllers
{
    [Route("[controller]/[action]")]
    public class BestillingController : ControllerBase
    {
        private readonly NORWAYContext _db;

        public BestillingController(NORWAYContext db)
        {
            _db = db;
        }

        public async Task<bool> Bestill(int avgangId)
        {
            try
            {
                var AvgangValg = _db.Avganger.Find(avgangId);

                var nyBestillingRad = new Bestilling
                {
                    Avgang = AvgangValg
                };

                _db.Bestillinger.Add(nyBestillingRad);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Bestilling>> HentAlleBestillinger()
        {
            try
            {
                List<Bestilling> alleBestillinger = await _db.Bestillinger.ToListAsync();

                return alleBestillinger;
            }
            catch
            {
                return null;
            }
        }
    }
}
