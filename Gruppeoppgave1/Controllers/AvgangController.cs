using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;
using Gruppeoppgave1.DAL;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Gruppeoppgave1.Controllers
{
    [Route("[controller]/[action]")]
    public class AvgangController : ControllerBase
    {
        private readonly IAvgangRepository _db;
        private ILogger<AvgangController> _log;

        public AvgangController(IAvgangRepository db, ILogger<AvgangController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> SjekkAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            bool returOK = await _db.SjekkAvganger(stasjonFraId, stasjonTilId, dato);

            if (!returOK)
            {
                _log.LogInformation("Avgangen fra: " + stasjonFraId + " til: " + stasjonTilId + " på dato: " + dato + " eksisterer ikke");
                return NotFound("Avgangen fra: " + stasjonFraId + " til: " + stasjonTilId + " på dato: " + dato + " eksisterer ikke");
            }
            return Ok("Avgang funnet");
        }

        public async Task<ActionResult> GenererAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            bool returOK = await _db.GenererAvganger(stasjonFraId, stasjonTilId, dato);
            if (!returOK)
            {
                _log.LogInformation("Avgangen fra: " + stasjonFraId + " til: " + stasjonTilId + " på dato: " + dato + " ble ikke generert");
                return BadRequest("Avgangen fra: " + stasjonFraId + " til: " + stasjonTilId + " på dato: " + dato + " ble ikke generert");
            }
            _log.LogInformation("Avgang generert");
            return Ok("Avgang generert");
        }

        public async Task<ActionResult> HentAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            List<Avgang> alleAvganger = await _db.HentAvganger(stasjonFraId, stasjonTilId, dato);
            return Ok(alleAvganger);
        }
    }
}
