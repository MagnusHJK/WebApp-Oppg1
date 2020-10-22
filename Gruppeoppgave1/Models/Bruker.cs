using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Models
{
    [ExcludeFromCodeCoverage]
    public class Bruker
    {
        public int Id { get; set; }

        public string Brukernavn { get; set; }

        public string Passord { get; set; }
    }
}
