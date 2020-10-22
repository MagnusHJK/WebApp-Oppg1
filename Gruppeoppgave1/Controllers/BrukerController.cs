using Castle.Core.Internal;
using Gruppeoppgave1.DAL;
using Gruppeoppgave1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Controllers
{
    [Route("[controller]/[action]")]
    public class BrukerController : ControllerBase
    {
        private readonly IBrukerRepository _db;
        private readonly ILogger<BrukerController> _log;
        private const string _loggetInn = "loggetInn";

        public BrukerController(IBrukerRepository db, ILogger<BrukerController> log)
        {
            _db = db;
            _log = log;
        }
        public async Task<ActionResult> LoggInn(Bruker bruker)
        {
            if (ModelState.IsValid)
            {
                bool returnOK = await _db.LoggInn(bruker);
                if (!returnOK)
                {
                    _log.LogInformation("Innlogging feilet for");
                    HttpContext.Session.SetString(_loggetInn, "");
                    return Ok(false);
                }
                HttpContext.Session.SetString(_loggetInn, "LoggetInn");
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering for innlogging");
            return BadRequest("Feil i inputvalidering på server for innlogging");
        }

        public void LoggUt()
        {
            HttpContext.Session.SetString(_loggetInn, "");
        }

        public ActionResult SjekkInnlogget()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke innlogget");
            }
            else
            {
                return Ok("Innlogget");
            }
        }

        public async Task<ActionResult> HentAlleBrukere()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized("Ikke innlogget");
            }

            List<Brukere> alleBrukere = await _db.HentAlleBrukere();

            if (alleBrukere.IsNullOrEmpty())
            {
                _log.LogInformation("Liste av brukere hentet, men den var tom eller null");
                return NotFound("Ingen brukere funnet");
            }

            return Ok(alleBrukere);
        }


        public async Task<ActionResult> LagGjesteBruker()
        {
            Brukere returBruker = await _db.LagGjesteBruker();
            if (returBruker == null)
            {
                _log.LogInformation("Oppretting av gjestebruker feilet");
                return BadRequest("Oppretting av gjestebruker feilet");
            }
            _log.LogInformation("Gjestebruker opprettet");
            return Ok(returBruker);
        }
    }
}
