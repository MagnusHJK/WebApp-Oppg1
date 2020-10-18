using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;
using Gruppeoppgave1.DAL;
using Microsoft.Extensions.Logging;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;

namespace Gruppeoppgave1.Controllers
{
    [Route("[controller]/[action]")]
    public class BestillingController : ControllerBase
    {
        private readonly IBestillingRepository _db;
        private readonly ILogger<BestillingController> _log;
        private const string _loggetInn = "loggetInn";

        public BestillingController(IBestillingRepository db, ILogger<BestillingController> log)
        {
            _db = db;
            _log = log;
        }


        public async Task<ActionResult> LagBestilling(int avgangId, int antall, int brukerId)
        {
            if (ModelState.IsValid)
            {
                bool returOK = await _db.LagBestilling(avgangId, antall, brukerId);

                if (!returOK)
                {
                    _log.LogInformation("Bestilling for avgang: " + avgangId + " ble ikke opprettet");
                    return BadRequest("Bestilling ble ikke opprettet");
                }
                return Ok("Bestilling opprettet");
            }
            _log.LogInformation("Feil input validering");
            return BadRequest("Feil input validering");
        }

        public async Task<ActionResult> EndreBestilling(int bestillingId, int nyAvgangId, int nyttAntall)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            { return Unauthorized(); }

            bool returOK = await _db.EndreBestilling(bestillingId, nyAvgangId, nyttAntall);

            if (!returOK)
            {
                _log.LogInformation("Bestilling med ID: " + bestillingId + " ble ikke endret");
                return BadRequest("Bestilling med ID: " + bestillingId + " ble ikke endret");
            }
            return Ok("Bestilling endret");
        }

        public async Task<ActionResult> SlettBestilling(int bestillingId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            { return Unauthorized(); }

            bool returOK = await _db.SlettBestilling(bestillingId);

            if (!returOK)
            {
                _log.LogInformation("Bestilling med ID: " + bestillingId + " ble ikke slettet");
                return BadRequest("Bestilling med ID: " + bestillingId + " ble ikke slettet");
            }
            return Ok("Bestillig slettet");
        }

        public async Task<ActionResult> HentBestillinger(int brukerId)
        {
            List<Bestillinger> bestillinger = await _db.HentBestillinger(brukerId);

            if (bestillinger.IsNullOrEmpty())
            {
                _log.LogInformation("Liste av bestillinger for bruker " + brukerId + " , men den var tom eller null");
                return BadRequest("Ingen bestillinger funnet");
            }
            return Ok(bestillinger);
        }

        public async Task<ActionResult> HentAlleBestillinger()
        {
            List<Bestillinger> alleBestillinger = await _db.HentAlleBestillinger();

            if (alleBestillinger.IsNullOrEmpty())
            {
                _log.LogInformation("Liste av bestillinger hentet, men den var tom eller null");
                return BadRequest("Ingen bestillinger funnet");
            }

            return Ok(alleBestillinger);
        }
    }
}
