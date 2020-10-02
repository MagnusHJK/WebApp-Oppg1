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
    public class StasjonController : ControllerBase
    {
        private readonly IStasjonRepository _db;
        private readonly ILogger<StasjonController> _log;
        public StasjonController(IStasjonRepository db, ILogger<StasjonController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> HentAlleStasjoner()
        {
            List<Stasjon> alleStasjoner = await _db.HentAlleStasjoner();
            return Ok(alleStasjoner);
        }

        public async Task<ActionResult> HentEnStasjon(int id)
        {
            Stasjon stasjon = await _db.HentEnStasjon(id);
            if (stasjon == null)
            {
                _log.LogInformation("Fant ikke stasjonen med ID: " + id);
                return NotFound("Fant ikke stasjonen med ID: " + id);
            }
            return Ok(stasjon);
        }
    }
}
