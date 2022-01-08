using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Vizsgaremek.Osztalyok
{
    public class Rendeles
    {
        public int razon, fazon, asztal, etelstatus, italstatus;
        public DateTime ido;
        public List<Tetel> tetelek;

        //propertyk, minden alkalommal amikor hivatkozunk rá újra lesznek határozva.
        // => propertyk, ugyan az csak rövidebb mint pl:
        // public int vegosszeg{
        //      get{
        //           return tetelek is not null ? tetelek.Sum(x=>x.vegosszeg) : 0 ;
        //         }
        //  }
        // amíg null a tetelek lista addig ez az érték 0, különben összeadja az összes tétel végösszegét
        public int vegosszeg => tetelek is not null ? tetelek.Sum(x=>x.vegosszeg) : 0 ;

        public Rendeles(int razon, int fazon, int asztal, DateTime ido, int etelstatus, int italstatus)
        {
            this.razon = razon;
            this.fazon = fazon;
            this.asztal = asztal;
            this.etelstatus = etelstatus;
            this.italstatus = italstatus;
            this.ido = ido;
            tetelekFrissit();
        }

        /// <summary>
        /// Lefrissíti az adott rendeléshez tartozó tételeket,mindegyikhez létrehoz egy Tetel példányt, és belerakja a 'tetelek' listába.
        /// </summary>
        public void tetelekFrissit()
        {
            tetelek = new();
            //Létrehozzuk a paramétereket, konstruktor első paraméter a cserélendő paraméter, a második az érték
            MySqlParameter razonparam = new("@razon",razon);
            List<MySqlParameter> param = new() { razonparam }; //hozzáadjuk mysqlparameter típusú listához
            List<string> eredmeny = MySQL.query("rendelestetelek", false, param); //lementjük a visszakapott eredményeket

            //tazon,razon,bazon,bdb,kazon,kdb,dazon,ddb,iazon,idb,etelstatus,italstatus,megjegyzes - 13al növeljük a ciklusváltozót
            for (int i = 0; i < eredmeny.Count; i += 13)
            {
                Tetel tetel = new(int.Parse(eredmeny[i]), int.Parse(eredmeny[i + 1]), int.Parse(eredmeny[i + 2]), int.Parse(eredmeny[i + 3]), int.Parse(eredmeny[i + 4]), int.Parse(eredmeny[i + 5]), int.Parse(eredmeny[i + 6]), int.Parse(eredmeny[i + 7]), int.Parse(eredmeny[i + 8]), int.Parse(eredmeny[i + 9]), int.Parse(eredmeny[i + 10]), int.Parse(eredmeny[i + 11]), eredmeny[i + 12]);
                tetelek.Add(tetel);
            }
        }


        //Ez lesz meghívva minden alkalommal amikor belerakjuk egy listboxba,comboboxba, vagy csak simán kiiratjuk.
        public override string ToString()
        {
            //Alapból a statusok folyamatban vannak
            string eStatus = "Folyamatban";
            string iStatus = "Folyamatban";
            if (etelstatus == 2) //ha 2es akkor készen van,máshova nem fog belépni, végén visszatérünk ezzel
                eStatus = "Kész!";
            if (italstatus == 2)
                iStatus = "Kész!"; //ha 2es akkor készen van,máshova nem fog belépni, végén visszatérünk ezzel
            if (etelstatus == 2 && italstatus == 2) // ha mindkettő 2es akkor belépünk és visszatérünk azzal hogy felszolgálásra vár
                return $"({razon}.r.sz) Asztal: {asztal}, felszolgálásra vár!";
            if (etelstatus == 3 && italstatus == 3)// ha mindkettő 3as akkor belépünk és visszatérünk azzal hogy fizetésre vár + rendelés végösszegével
                return $"({razon}.r.sz) Asztal: {asztal}, fizetésre vár, összeg: {vegosszeg}!";
            if (etelstatus == 4 && italstatus == 4) // ha mindkettő 4es akkor belépünk és visszatérünk azzal hogy fizetve van
                return $"({razon}.r.sz) Asztal: {asztal}, fizetve!";

            //ha sehova nem léptünk be akkor visszatérünk a statusokkal
            return $"({razon}.r.sz) Asztal: {asztal}, étel:{eStatus} ital: {iStatus}";
        }
    }
}
