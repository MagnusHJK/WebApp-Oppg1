using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace Gruppeoppgave1.DAL
{
    public class AvgangRepository : IAvgangRepository
    {
        private readonly NORWAYContext _db;

        public AvgangRepository(NORWAYContext db)
        {
            _db = db;
        }

        //Sjekker om avganger for reise mellom stasjonene eksisterer
        public async Task<bool> SjekkAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            try
            {
                DateTime gittDato = DateTime.Parse(dato);

                List<Avgang> alleAvganger = await _db.Avganger.ToListAsync();

                //Sjekker om en avgang har de riktige stasjonene og dato
                if (alleAvganger.Any(a => a.StasjonFra.Id == stasjonFraId && a.StasjonTil.Id == stasjonTilId && a.Dato.Date == gittDato.Date))
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

        //Generer avganger hver andre time for gitt strekning og dato.
        //Denne blir kalt hvis det ikke eksisterer noen avganger for gitt strekning og dato
        public async Task<bool> GenererAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            try
            {
                Stasjon stasjonFraValg = new Stasjon();
                stasjonFraValg = _db.Stasjoner.Find(stasjonFraId);

                Stasjon stasjonTilValg = new Stasjon();
                stasjonTilValg = _db.Stasjoner.Find(stasjonTilId);

                //Lager DateTime objekt
                DateTime datetime = DateTime.Parse(dato);
                
                if (stasjonFraValg != null && stasjonTilValg != null)
                {
                    for (int i = 0; i < 24; i += 2)
                    {
                        //Ny avgang
                        Avgang nyAvgangRad = new Avgang
                        {
                            StasjonFra = stasjonFraValg,
                            StasjonTil = stasjonTilValg,
                            Dato = datetime.AddHours(i),
                            Pris = 200
                        };
                        _db.Avganger.Add(nyAvgangRad);
                        
                    }
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        //Henter liste over avganger for spesifikk strekning på valgt dato
        //Hvis det er avganger for nåværende dag vil det kun hentes avganger som ikke er i fortiden.
        public async Task<List<Avgang>> HentAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            try
            {

                List<Avgang> Avganger;

                //Henter dagens dato
                DateTime lokalDato = DateTime.Now;
                DateTime gittDato = DateTime.Parse(dato);

                //Hvis det er snakk om dagens dato blir kun avganger som ikke har dratt enda hentet
                if (gittDato.Date == lokalDato.Date)
                {
                    //Finner tiden, kan ikke bruke gittDato ettersom denne kun inneholder dato og ikke tid
                    int lokalTid = DateTime.Now.Hour;

                    Avganger = await _db.Avganger
                        .Where(a => a.StasjonFra.Id == stasjonFraId &&
                               a.StasjonTil.Id == stasjonTilId &&
                               a.Dato.Date == gittDato.Date &&
                               a.Dato.Hour > lokalTid)
                        .ToListAsync();
                }
                //Alle avganger for dagen blir hentet
                else
                {
                    Avganger = await _db.Avganger
                        .Where(a => a.StasjonFra.Id == stasjonFraId && 
                               a.StasjonTil.Id == stasjonTilId && 
                               a.Dato.Date == gittDato.Date)
                        .ToListAsync();
                }

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

