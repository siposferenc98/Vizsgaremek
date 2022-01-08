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
        //Ezzel a propertyvel lekérhető a felhasználó listából a foglaláshoz tartozó felhasználó.
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

        //Ez lesz meghívva minden alkalommal amikor belerakjuk egy listboxba,comboboxba, vagy csak simán kiiratjuk.
        public override string ToString()
        {
            return $"{fazon} sz.f.,név: {felhasznalo.nev}, {szemelydb} főre, {idopontDatum.Year}/{idopontDatum.Month}/{idopontDatum.Day} {idopontOraPerc.Hour}:00 Perc.";
        }
    }
}
