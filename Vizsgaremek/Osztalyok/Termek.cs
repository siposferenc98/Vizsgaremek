using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    class Termek
    {
        int azon, ar;
        string nev , leiras;
        public Termek(int azon, string nev, int ar, string leiras)
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
