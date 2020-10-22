using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Models
{
    [ExcludeFromCodeCoverage]
    public class Avgang
    {
        public int Id { get; set; }

        virtual public Stasjon StasjonFra { get; set; }

        virtual public Stasjon StasjonTil { get; set; }
        
        public DateTime Dato { get; set; }

        [RegularExpression(@"^[0-9]{1,5}$")]
        public int Pris { get; set; }

    }
}
