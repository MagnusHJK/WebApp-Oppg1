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
using Microsoft.AspNetCore.Http;
using Castle.Core.Internal;

namespace Gruppeoppgave1.Controllers
{
    [Route("[controller]/[action]")]
    public class AvgangController : ControllerBase
    {
        private readonly IAvgangRepository _db;
        private readonly ILogger<AvgangController> _log;
        private const string _loggetInn = "loggetInn";

        public AvgangController(IAvgangRepository db, ILogger<AvgangController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> LagAvgang(int stasjonFraId, int stasjonTilId, string datoTid, int pris)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            { return Unauthorized(); }

            bool returOK = await _db.LagAvgang(stasjonFraId, stasjonTilId, datoTid, pris);

            if (!returOK)
            {
                _log.LogInformation("Avgangen fra: " + stasjonFraId + " til: " + stasjonTilId + " på dato: " + datoTid + " ble ikke laget");
                return NotFound("Avgangen fra: " + stasjonFraId + " til: " + stasjonTilId + " på dato: " + datoTid + " ble ikke laget");
            }
            return Ok("Avgang opprettet");
        }

        public async Task<ActionResult> EndreAvgang(int avgangId, string datoTid, int pris)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            { return Unauthorized(); }

            bool returOK = await _db.EndreAvgang(avgangId, datoTid, pris);

            if (!returOK)
            {
                _log.LogInformation("Avgangen med ID: " + avgangId + " ble ikke endret");
                return BadRequest("Avgangen med ID: " + avgangId + " ble ikke endret");
            }
            return Ok("Avgangen ble endret");
        }

        public async Task<ActionResult> SlettAvgang(int avgangId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            { return Unauthorized(); }

            bool returOK = await _db.SlettAvgang(avgangId);

            if (!returOK)
            {
                _log.LogInformation("Avgangen med ID: " + avgangId + " ble ikke slettet");
                return BadRequest("Avgangen med ID: " + avgangId + " ble ikke slettet");
            }
            return Ok("Avgangen ble slettet");
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
            _log.LogInformation("Avgangen fra: " + stasjonFraId + " til: " + stasjonTilId + " på dato: " + dato + " ble generert");
            return Ok("Avgang generert");
        }

        public async Task<ActionResult> HentAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            List<Avganger> avganger = await _db.HentAvganger(stasjonFraId, stasjonTilId, dato);

            if (avganger.IsNullOrEmpty())
            {
                _log.LogInformation("Avganger fra: " + stasjonFraId + " til: " + stasjonTilId + " på dato: " + dato + " eksisterer ikke");
                return NotFound("Avganger fra: " + stasjonFraId + " til: " + stasjonTilId + " på dato: " + dato + " eksisterer ikke");
            }
            _log.LogInformation("Avgangen fra: " + stasjonFraId + " til: " + stasjonTilId + " på dato: " + dato + " ble hentet");
            return Ok(avganger);
        }

        public async Task<ActionResult> HentAlleAvganger()
        {
            List<Avganger> alleAvganger = await _db.HentAlleAvganger();

            if (alleAvganger.IsNullOrEmpty())
            {
                _log.LogInformation("Fant ingen avganger");
                return NotFound("Fant ingen avganger");
            }
            _log.LogInformation("Alle avganger hentet");
            return Ok(alleAvganger);
        }
    }
}
