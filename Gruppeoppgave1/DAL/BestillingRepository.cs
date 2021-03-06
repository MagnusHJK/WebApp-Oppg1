﻿using Castle.Core.Internal;
using Gruppeoppgave1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Gruppeoppgave1.DAL
{
    [ExcludeFromCodeCoverage]
    public class BestillingRepository : IBestillingRepository
    {
        private readonly NORWAYContext _db;

        public BestillingRepository(NORWAYContext db)
        {
            _db = db;
        }

        public async Task<bool> LagBestilling(int avgangId, int antall, int brukerId)
        {
            try
            {
                var nyBestillingRad = new Bestillinger();

                Avganger ValgtAvgang = await _db.Avganger.FindAsync(avgangId);
                Brukere ValgtBruker = await _db.Brukere.FindAsync(brukerId);

                if (ValgtAvgang != null)
                {
                    nyBestillingRad.Avgang = ValgtAvgang;
                    nyBestillingRad.Antall = antall;
                    nyBestillingRad.Bruker = ValgtBruker;

                    _db.Bestillinger.Add(nyBestillingRad);
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

        //Sender en email med alle bestillingene til en bruker til gitt mail
        public async Task<bool> SendBestillingMail(string tilMail, int brukerId)
        {
            List<Bestillinger> bestillinger = await HentBestillinger(brukerId);
            string mailBody = "";

            foreach(Bestillinger bestilling in bestillinger)
            {
                mailBody += "<h2>" + bestilling.Antall + " billett(er) for strekningen: </h2><br/>" + 
                            bestilling.Avgang.StasjonFra.Navn + " -> " + bestilling.Avgang.StasjonTil.Navn + " " + 
                            bestilling.Avgang.Dato.ToString("dddd, dd MMMM yyyy HH:mm:ss") + "<br/>" +
                            "Total pris " + (bestilling.Avgang.Pris * bestilling.Antall) + ",- <br/>";
            }

            using(MailMessage emailMessage = new MailMessage())
            {
                var fraAddresse = new MailAddress("NORWAY.ITPE3200@gmail.com", "NOR-WAY");
                var fraPassord = "*c*S%vX6PSXr6mw9tjy!tstfF";
                var tilAddresse = new MailAddress(tilMail);
               
                emailMessage.To.Add(tilAddresse);
                emailMessage.From = fraAddresse;
                emailMessage.Subject = "NOR-WAY - Dine bestillinger";
                emailMessage.Body = mailBody;
                emailMessage.Priority = MailPriority.Normal;
                emailMessage.IsBodyHtml = true;

                using (SmtpClient MailClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    try
                    {
                        MailClient.EnableSsl = true;
                        MailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        MailClient.UseDefaultCredentials = false;
                        MailClient.Credentials = new System.Net.NetworkCredential(fraAddresse.Address, fraPassord);
                        MailClient.Timeout = 20000;
                        MailClient.Send(emailMessage);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }

        public async Task<bool> EndreBestilling(int bestillingId, int nyAvgangId, int nyttAntall, int nyBrukerId)
        {
            try
            {
                Bestillinger endretBestilling = await _db.Bestillinger.FirstOrDefaultAsync(b => b.Id == bestillingId);

                if (endretBestilling != null)
                {
                    Avganger nyAvgang = await _db.Avganger.FirstOrDefaultAsync(a => a.Id == nyAvgangId);
                    Brukere nyBruker = await _db.Brukere.FirstOrDefaultAsync(b => b.Id == nyBrukerId);

                    if(nyAvgang != null && nyBruker != null)
                    {
                        endretBestilling.Avgang = nyAvgang;
                        endretBestilling.Antall = nyttAntall;
                        endretBestilling.Bruker = nyBruker;
                        await _db.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                return false;

            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> SlettBestilling(int bestillingId)
        {
            try
            {
                Bestillinger slettetBestilling = await _db.Bestillinger.FirstOrDefaultAsync(b => b.Id == bestillingId);
                _db.Bestillinger.Remove(slettetBestilling);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Henter bestillinger for gitt bruker
        public async Task<List<Bestillinger>> HentBestillinger(int brukerId)
        {
            try
            {
                List<Bestillinger> bestillinger = await _db.Bestillinger.Where(b => b.Bruker.Id == brukerId).ToListAsync();
                return bestillinger;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Bestillinger>> HentAlleBestillinger()
        {
            try
            {
                List<Bestillinger> alleBestillinger = await _db.Bestillinger.Select(b => new Bestillinger
                {
                    Id = b.Id,
                    Avgang = b.Avgang,
                    Antall = b.Antall

                }).ToListAsync();

                return alleBestillinger;
            }
            catch
            {
                return null;
            }
        }
    }
}

