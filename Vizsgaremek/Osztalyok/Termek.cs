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
        public bool aktiv;

        public Termek(int azon, string nev, int ar, string leiras, bool aktiv)
        {
            this.azon = azon;
            this.ar = ar;
            this.nev = nev;
            this.leiras = leiras;
            this.aktiv = aktiv;
        }

        public override string ToString()
        {
            return nev;
        }
    }
}
