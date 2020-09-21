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
    public class BestillingController : ControllerBase
    {
        private readonly IBestillingRepository _db;
        public BestillingController(IBestillingRepository db)
        {
            _db = db;
        }


        public async Task<bool> LagBestilling(int avgangId, int antall)
        {
            return await _db.LagBestilling(avgangId, antall);
        }

        public async Task<List<Bestilling>> HentAlleBestillinger()
        {
            return await _db.HentAlleBestillinger();
        }

    }
}
