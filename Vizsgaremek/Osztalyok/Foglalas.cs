using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    class Foglalas
    {
        public int fazon, azon, szemelydb;
        public bool megjelent;
        public DateTime idopontDatum, idopontOraPerc, leadva;
        public Felhasznalo felhasznalo => Felhasznalok.felhasznaloLista.First(x => x.id == azon);

        public Foglalas()
        {
        }

        public Foglalas(int fazon, int azon, int szemelydb, DateTime idopontDatum,DateTime idopontOraPerc, DateTime leadva, bool megjelent)
        {
            this.fazon = fazon;
            this.azon = azon;
            this.szemelydb = szemelydb;
            this.megjelent = megjelent;
            this.idopontDatum = idopontDatum;
            this.idopontOraPerc = idopontOraPerc;
            this.leadva = leadva;
        }

        public override string ToString()
        {
            return $"{fazon} sz.f.,név: {felhasznalo.nev}, {szemelydb} főre.";
        }
    }
}
