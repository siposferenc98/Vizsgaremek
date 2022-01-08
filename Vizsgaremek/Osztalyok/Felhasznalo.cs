using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    public class Felhasznalo
    {
        public string nev, lakh, email, tel;

        //biztonság kedvéért ez a kettő private setre lett állítva
        public int id { get; private set; }
        public int jog { get; private set; }

        public Felhasznalo(int id, string nev, string lakh, string tel, string email, int jog)
        {
            this.nev = nev;
            this.lakh = lakh;
            this.email = email;
            this.tel = tel;
            this.id = id;
            this.jog = jog;
        }

        //Ez lesz meghívva minden alkalommal amikor belerakjuk egy listboxba,comboboxba, vagy csak simán kiiratjuk.
        public override string ToString()
        {
            return nev;
        }
    }
}
