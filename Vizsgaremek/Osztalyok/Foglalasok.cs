using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    class Foglalasok
    {
        public static List<Foglalas> foglalasLista = new();


        /// <summary>
        /// Lekéri az összes foglalást a 'foglalas' táblánkból, az összesből csinál egy Foglalas példányt és hozzáadja a foglalasLista-ba.
        /// </summary>
        public static void foglalasokFrissit()
        {
            //lementjük az eredményt
            List<string> eredmeny = MySQL.query("foglalaslekerdezes", false);
            //töröljük/újat csinálunk a listánkból hogy ne a régiekhez adjuk hozzá
            foglalasLista = new();

            //fazon,azon,szemelydb,datum,idopont,ido,megjelent - 7-el növeljük a ciklusváltozót
            for (int i = 0; i < eredmeny.Count; i += 7)
            {
                Foglalas foglalas = new(int.Parse(eredmeny[i]), int.Parse(eredmeny[i+1]), int.Parse(eredmeny[i+2]), DateTime.Parse(eredmeny[i+3]), DateTime.Parse(eredmeny[i + 4]), DateTime.Parse(eredmeny[i + 5]), bool.Parse(eredmeny[i+6]));
                foglalasLista.Add(foglalas);
            }
        }

    }
}
