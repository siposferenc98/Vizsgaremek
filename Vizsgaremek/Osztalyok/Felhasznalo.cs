using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    public class Felhasznalo
    {
        public string nev;
        public int id { get; private set; }
        public int jog { get; private set; }
        public Felhasznalo(string nev, int id, int jog)
        {
            this.nev = nev;
            this.id = id;
            this.jog = jog;
        }
    }
}
