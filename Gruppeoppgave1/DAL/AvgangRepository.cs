using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                List<Avgang> alleAvganger = await _db.Avganger.ToListAsync();

                //Sjekker om en avgang har de riktige stasjonene og dato
                if (alleAvganger.Any(a => a.StasjonFra.Id == stasjonFraId && a.StasjonTil.Id == stasjonTilId && a.Dato == dato))
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
                Stasjon stasjonFraValg = new Stasjon();
                stasjonFraValg = _db.Stasjoner.Find(stasjonFraId);

                Stasjon stasjonTilValg = new Stasjon();
                stasjonTilValg = _db.Stasjoner.Find(stasjonTilId);

                if (stasjonFraValg != null && stasjonTilValg != null)
                {
                    for (int i = 0; i < 24; i += 2)
                    {
                        Avgang nyAvgangRad = new Avgang
                        {
                            StasjonFra = stasjonFraValg,
                            StasjonTil = stasjonTilValg,
                            Dato = dato,
                            Tidspunkt = i + ":00",
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

        //Gir liste over avganger for spesifikk strekning på valgt dato
        public async Task<List<Avgang>> HentAvganger(int stasjonFraId, int stasjonTilId, string dato)
        {
            try
            {
                List<Avgang> Avganger = await _db.Avganger.Where(a => a.StasjonFra.Id == stasjonFraId && a.StasjonTil.Id == stasjonTilId && a.Dato == dato).ToListAsync();
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

