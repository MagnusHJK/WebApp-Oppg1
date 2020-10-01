using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;
using Gruppeoppgave1.DAL;
using Microsoft.Extensions.Logging;

namespace Gruppeoppgave1.Controllers
{
    [Route("[controller]/[action]")]
    public class BestillingController : ControllerBase
    {
        private readonly IBestillingRepository _db;
        private ILogger<BestillingController> _log;

        public BestillingController(IBestillingRepository db, ILogger<BestillingController> log)
        {
            _db = db;
            _log = log;
        }


        public async Task<ActionResult> LagBestilling(int avgangId, int antall)
        {
            bool returOK = await _db.LagBestilling(avgangId, antall);

            if (!returOK)
            {
                _log.LogInformation("Bestilling for avgang: " + avgangId + " ble ikke opprettet");
                return BadRequest("Bestilling for avgang: " + avgangId + " ble ikke opprettet");
            }
            return Ok("Bestilling opprettet");
        }

        public async Task<ActionResult> HentAlleBestillinger()
        {
            List<Bestilling> alleBestillinger = await _db.HentAlleBestillinger();
            return Ok(alleBestillinger);
        }
    }
}
