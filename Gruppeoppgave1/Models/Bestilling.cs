using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Models
{
    public class Bestilling
    {
        public int Id { get; set; }
        
        public string StasjonFra { get; set; }

        public string StasjonTil { get; set; }

        public string Dato { get; set; }

        public string Tidspunkt { get; set; }

    }
}
