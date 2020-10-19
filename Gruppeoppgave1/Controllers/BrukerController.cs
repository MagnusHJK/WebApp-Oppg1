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
                    _log.LogInformation("Innlogging feilet for " + bruker.Brukernavn);
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

        public async Task<ActionResult> HentAlleBrukere()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            List<Brukere> alleBrukere = await _db.HentAlleBrukere();

            if (alleBrukere.IsNullOrEmpty())
            {
                _log.LogInformation("Liste av brukere hentet, men den var tom eller null");
                return BadRequest("Ingen brukere funnet");
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

        public void TestSession()
        {
            //Slik testes det om noen er logget inn
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn))){
                //Hva som skal bli returnert hvis man ikke er logget inn
                //Gir en error kode 401, som skal bli sjekket for i alle .fail() på ajax kall i javascript
                //return Unauthorized();
            }
            //Så under kommer all annen kode
        }
    }
}
