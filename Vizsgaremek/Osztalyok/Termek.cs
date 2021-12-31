using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    public class Termek
    {
        public int azon, ar;
        public string nev, leiras;

        public Termek(int azon, string nev, int ar, string leiras = null)
        {
            this.azon = azon;
            this.ar = ar;
            this.nev = nev;
            this.leiras = leiras;
        }

        public override string ToString()
        {
            return nev;
        }
    }
}
