using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppeoppgave1.Models
{
    public class Bestilling
    {
        public int Id { get; set; }
        
        virtual public Avgang Avgang { get; set; }

        //public Kunde Kunde { get; set; }
    }
}
