using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Models
{
    public class Avgang
    {
        public int Id { get; set; }

        public int StasjonFra { get; set; }

        public int StasjonTil { get; set; }
        
        public string Dato { get; set; }

        public string Tidspunkt { get; set; }

        public string Pris { get; set; }
    }
}
