using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;
using Gruppeoppgave1.DAL;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Gruppeoppgave1.Controllers
{
    [Route("[controller]/[action]")]
    public class StasjonController : ControllerBase
    {
        private readonly IStasjonRepository _db;
        private readonly ILogger<StasjonController> _log;
        private const string _loggetInn = "loggetInn";
        public StasjonController(IStasjonRepository db, ILogger<StasjonController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> HentAlleStasjoner()
        {
            List<Stasjoner> alleStasjoner = await _db.HentAlleStasjoner();
            return Ok(alleStasjoner);
        }

        public async Task<ActionResult> HentEnStasjon(int id)
        {
            Stasjoner stasjon = await _db.HentEnStasjon(id);
            if (stasjon == null)
            {
                _log.LogInformation("Fant ikke stasjonen med ID: " + id);
                return NotFound("Fant ikke stasjonen med ID: " + id);
            }
            return Ok(stasjon);
        }

        public async Task<ActionResult> LagStasjon(Stasjoner stasjon)
        {
            //Sjekker innlogget
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            { return Unauthorized(); }

            if (ModelState.IsValid)
            {
                bool returOK = await _db.LagStasjon(stasjon);
                if (!returOK)
                {
                    _log.LogInformation("Oppretting av stasjon feilet: " + stasjon.Navn);
                    return Ok(false);
                }
                _log.LogInformation("Stasjon ved navn " + stasjon.Navn + " ble opprettet");
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering for oppretting av Stasjon");
            return BadRequest("Feil i inputvalidering for oppretting av Stasjon");
        }

        public async Task<ActionResult> EndreStasjon(Stasjoner stasjon)
        {
            System.Diagnostics.Debug.WriteLine("Controller: " + stasjon.Navn);
            //Sjekker innlogget
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            { return Unauthorized(); }

            if (ModelState.IsValid)
            {
                bool returOK = await _db.EndreStasjon(stasjon);
                if (!returOK)
                {
                    _log.LogInformation("Endring av stasjon feilet: " + stasjon.Navn);
                    return Ok(false);
                }
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering for endring av Stasjon");
            return BadRequest("Feil i inputvalidering for endring av Stasjon");
        }
    }
}
