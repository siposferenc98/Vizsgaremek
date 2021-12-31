using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    public class Tetel
    {
        public int tazon { get; private set; }
        public int razon { get; private set; }
        public int bazon, bdb, kazon, kdb, dazon, ddb, iazon, idb, etelstatus, italstatus;
        public string megjegyzes;
        public Termek burger => Termekek.listakDictionary['h'].First(x => x.azon == bazon);
        public Termek koret => Termekek.listakDictionary['k'].First(x => x.azon == kazon);
        public Termek desszert => Termekek.listakDictionary['d'].First(x => x.azon == dazon);
        public Termek ital => Termekek.listakDictionary['i'].First(x => x.azon == iazon);
        public int vegosszeg => burger.ar*bdb + koret.ar*kdb + desszert.ar*ddb + ital.ar * idb;

        public Tetel(int tazon, int razon, int bazon, int bdb, int kazon, int kdb, int dazon, int ddb, int iazon, int idb, int etelstatus, int italstatus, string megjegyzes)
        {
            this.tazon = tazon;
            this.razon = razon;
            this.bazon = bazon;
            this.bdb = bdb;
            this.kazon = kazon;
            this.kdb = kdb;
            this.dazon = dazon;
            this.ddb = ddb;
            this.iazon = iazon;
            this.idb = idb;
            this.etelstatus = etelstatus;
            this.italstatus = italstatus;
            this.megjegyzes = megjegyzes;
        }

        public override string ToString()
        {
            return $"megjegyzés: {megjegyzes}, stb {bazon}";
        }
    }
}
