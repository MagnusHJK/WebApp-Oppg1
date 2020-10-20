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
using Castle.Core.Internal;

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
            if (alleStasjoner.IsNullOrEmpty())
            {
                return NotFound("Fant ingen stasjoner");
            }

            return Ok(alleStasjoner);
        }

        public async Task<ActionResult> HentEnStasjon(int id)
        {
            Stasjoner stasjon = await _db.HentEnStasjon(id);
            if (stasjon == null)
            {
                _log.LogInformation("Fant ikke stasjonen");
                return NotFound("Fant ikke stasjonen");
            }
            return Ok(stasjon);
        }

        public async Task<ActionResult> LagStasjon(Stasjon stasjon)
        {
            //Sjekker innlogget
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            { return Unauthorized("Ikke innlogget"); }

            if (ModelState.IsValid)
            {
                bool returOK = await _db.LagStasjon(stasjon);
                if (!returOK)
                {
                    _log.LogInformation("Oppretting av stasjon feilet");
                    return BadRequest("Oppretting av stasjon feilet");
                }
                _log.LogInformation("Stasjon opprettet");
                return Ok("Stasjon opprettet");
            }
            _log.LogInformation("Feil i inputvalidering for oppretting av Stasjon");
            return BadRequest("Feil i inputvalidering for oppretting av Stasjon");
        }

        public async Task<ActionResult> EndreStasjon(Stasjon stasjon)
        {
            //Sjekker innlogget
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            { return Unauthorized("Ikke innlogget"); }

            if (ModelState.IsValid)
            {
                bool returOK = await _db.EndreStasjon(stasjon);
                if (!returOK)
                {
                    _log.LogInformation("Endring av stasjonen feilet");
                    return BadRequest("Endring av stasjonen feilet");
                }
                return Ok("Stasjon endret");
            }
            _log.LogInformation("Feil i inputvalidering for endring av Stasjon");
            return BadRequest("Feil i inputvalidering for endring av Stasjon");
        }

        public async Task<ActionResult> SlettStasjon(int id)
        {
            //Sjekker innlogget
            if(string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke innlogget");
            }

            if(ModelState.IsValid)
            {
                bool returOK = await _db.SlettStasjon(id);
                if(!returOK)
                {
                    _log.LogInformation("Sletting av stasjon feilet");
                    return BadRequest("Sletting av stasjon feilet");
                }
                return Ok("Stasjon slettet");
            }
            _log.LogInformation("Feil i sletting av Stasjon");
            return BadRequest("Feil i sletting av Stasjon");
        }
    }
}
