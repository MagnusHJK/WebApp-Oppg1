using Gruppeoppgave1.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Gruppeoppgave1.DAL
{
    [ExcludeFromCodeCoverage]
    public class BrukerRepository : IBrukerRepository
    {
        private readonly NORWAYContext _db;

        public BrukerRepository(NORWAYContext db)
        {
            _db = db;
        }

        public static byte[] LagHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                password: passord,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 1000,
                numBytesRequested: 32);
        }

        public static byte[] LagSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;

        }

        public async Task<bool> LoggInn(Bruker bruker)
        {
            try
            {
                Brukere funnetBruker = await _db.Brukere.FirstOrDefaultAsync(b => b.Brukernavn == bruker.Brukernavn);
                //Sjekk passordet
                byte[] hash = LagHash(bruker.Passord, funnetBruker.Salt);
                bool ok = hash.SequenceEqual(funnetBruker.Passord);

                if (ok)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                //_log.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<List<Brukere>> HentAlleBrukere()
        {
            try
            {
                List<Brukere> alleBrukere = await _db.Brukere.ToListAsync();
                return alleBrukere;
            }
            catch
            {
                return null;
            }
        }

        //Gjeste brukerer sitt passord er tilfeldig, etter som de bare er midlertidige entiteter
        //Uten mulighet til å "logge inn".
        public async Task<Brukere> LagGjesteBruker()
        {
            try
            {
                Random rnd = new Random();
                int tilfeldig = rnd.Next();

                string passord = "Gjest" + tilfeldig;
                byte[] salt = BrukerRepository.LagSalt();
                byte[] hash = BrukerRepository.LagHash(passord, salt);

                Brukere gjesteBruker = new Brukere
                {
                    Brukernavn = "Gjest",
                    Passord = hash,
                    Salt = salt

                };

                await _db.Brukere.AddAsync(gjesteBruker);
                await _db.SaveChangesAsync();

                return gjesteBruker;
            }
            catch
            {
                return null;
            }
        }

        //Endrer brukernavn, denne er ikke i bruk
        //Kan tenkes å brukes hvis man vil knytte navn til bruker
        public async Task<bool> EndreBrukernavn(Bruker bruker)
        {
            try
            {
                Brukere funnetBruker = await _db.Brukere.FirstOrDefaultAsync(b => b.Brukernavn == bruker.Brukernavn);
                if(funnetBruker != null)
                {
                    funnetBruker.Brukernavn = bruker.Brukernavn;
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        //Sletter bruker med brukerId
        public async Task<bool> SlettBruker(int brukerId)
        {
            try
            {
                Brukere funnetBruker = await _db.Brukere.FindAsync(brukerId);
                System.Diagnostics.Debug.WriteLine(funnetBruker.Id + " " + funnetBruker.Brukernavn);
                if (funnetBruker != null)
                {
                    if(funnetBruker.Brukernavn != "Admin")
                    {
                        _db.Brukere.Remove(funnetBruker);
                        await _db.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
