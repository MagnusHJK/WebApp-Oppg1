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
    public class StasjonController : ControllerBase
    {
        private readonly IStasjonRepository _db;
        public StasjonController(IStasjonRepository db)
        {
            _db = db;
        }

        public async Task<List<Stasjon>> HentAlleStasjoner()
        {
            return await _db.HentAlleStasjoner();
        }

        public async Task<Stasjon> HentEnStasjon(int id)
        {
            return await _db.HentEnStasjon(id);
        }
    }
}
