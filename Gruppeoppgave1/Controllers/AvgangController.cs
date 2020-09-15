using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;

namespace Gruppeoppgave1.Controllers
{
    [Route("[controller]/[action]")]
    public class AvgangController : ControllerBase
    {
        private readonly NORWAYContext _db;

        public AvgangController(NORWAYContext db)
        {
            _db = db;
        }

        //Sjekker om avganger for reise mellom stasjonene eksisterer
        public async Task<bool> SjekkAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            try
            {
                List<Avgang> alleAvganger = await _db.Avganger.ToListAsync();

                //Sjekker om en avgang har de riktige stasjonene og dato
                if(alleAvganger.Any(a => a.StasjonFra == stasjonFraId && a.StasjonTil == stasjonTilId && a.Dato == dato))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        
        //Generer avganger hver andre time for gitt strekning og dato. Disse blir filtrert senere
        public async Task<bool> GenererAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            try
            {
                for(int i = 0; i<24; i+=2)
                {
                    Avgang avgang = new Avgang
                    {
                        StasjonFra = stasjonFraId,
                        StasjonTil = stasjonTilId,
                        Dato = dato,
                        Tidspunkt = i + ":00",
                        Pris = "200"
                    };
                    _db.Avganger.Add(avgang);
                }

                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        //Gir liste over avganger for spesifikk strekning på valgt dato
        public async Task<List<Avgang>> HentAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            try
            {
                List<Avgang> Avganger = await _db.Avganger.Where(a => a.StasjonFra == stasjonFraId && a.StasjonTil == stasjonTilId && a.Dato == dato).ToListAsync();
                return Avganger;
            }
            catch
            {
                return null;
            }
        }

        public string Test(int stasjonFra, int stasjonTil)
        {
            return "return fra test: " + stasjonFra + " " + stasjonTil;
        }
    }
}
