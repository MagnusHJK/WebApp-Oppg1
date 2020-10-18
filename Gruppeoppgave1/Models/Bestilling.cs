using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Models
{
    public class Bestilling
    {
        public int Id { get; set; }
        
        virtual public Avgang Avgang { get; set; }

        [RegularExpression(@"[0-9]{1,2}")]
        public int Antall { get; set; }

        public Bruker Bruker { get; set; }
    }
}
