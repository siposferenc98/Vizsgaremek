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
        private int italstatus;
        public string megjegyzes;
        private int bazon;
        private int bdb;
        private int kazon;
        private int kdb;
        private int dazon;
        private int ddb;
        private int iazon;
        private int idb;
        private int etelstatus;

        //propertyk, minden alkalommal amikor hivatkozunk rá újra lesznek határozva.
        // => propertyk, ugyan az csak rövidebb mint pl:
        // public Termek burger{
        //      get{
        //           return Termekek.listakDictionary['h'].First(x=>x.azon == bazon);
        //         }
        //  }
        public Termek Burger => Termekek.listakDictionary['h'].First(x => x.azon == Bazon);
        public Termek Koret => Termekek.listakDictionary['k'].First(x => x.azon == Kazon);
        public Termek Desszert => Termekek.listakDictionary['d'].First(x => x.azon == Dazon);
        public Termek Ital => Termekek.listakDictionary['i'].First(x => x.azon == Iazon);
        public int vegosszeg => Burger.ar*Bdb + Koret.ar*Kdb + Desszert.ar*Ddb + Ital.ar * idb;

        public int Bazon { get => bazon; set => bazon = value; }
        public int Bdb { get => bdb; set => bdb = value; }
        public int Kazon { get => kazon; set => kazon = value; }
        public int Kdb { get => kdb; set => kdb = value; }
        public int Dazon { get => dazon; set => dazon = value; }
        public int Ddb { get => ddb; set => ddb = value; }
        public int Iazon { get => iazon; set => iazon = value; }
        public int Idb { get => idb; set => idb = value; }
        public int Etelstatus { get => etelstatus; set => etelstatus = value; }
        public int Italstatus { get => italstatus; set => italstatus = value; }

        public Tetel(int tazon, int razon, int bazon, int bdb, int kazon, int kdb, int dazon, int ddb, int iazon, int idb, int etelstatus, int italstatus, string megjegyzes)
        {
            this.tazon = tazon;
            this.razon = razon;
            this.Bazon = bazon;
            this.Bdb = bdb;
            this.Kazon = kazon;
            this.Kdb = kdb;
            this.Dazon = dazon;
            this.Ddb = ddb;
            this.Idb = idb;
            this.Iazon = iazon;
            this.Etelstatus = etelstatus;
            this.Italstatus = italstatus;
            this.megjegyzes = megjegyzes;
        }

        //Ez lesz meghívva minden alkalommal amikor belerakjuk egy listboxba,comboboxba, vagy csak simán kiiratjuk.
        public override string ToString()
        {
            return $"{Burger.nev}, {Bdb} db, ár: {Bdb * Burger.ar} Ft. \n" +
                    $"{Koret.nev}, {Kdb} db, ár: {Kdb * Koret.ar} Ft. \n" +
                    $"{Ital.nev}, {idb} db, ár: {idb * Ital.ar} Ft. \n" +
                    $"{Desszert.nev}, {Ddb} db, ár: {Ddb * Desszert.ar} Ft. \n" +
                    $"Megjegyzés: {megjegyzes} \n" +
                    $"Összesen: {vegosszeg} Ft. \n \n" +
                    $"Állapot: Étel:{(Etelstatus == 4 ? "Kész" : Etelstatus == 3 ? "Felszolgálva" : Etelstatus == 2 ? "Kész" : "Folyamatban")} ," +
                    $"Ital: {(Italstatus == 4 ? "Kész" : Italstatus == 3 ? "Felszolgálva" : Italstatus == 2 ? "Kész" : "Folyamatban")}";
        }
    }
}
