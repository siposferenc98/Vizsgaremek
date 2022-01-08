using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    class Felhasznalok
    {
        public static List<Felhasznalo> felhasznaloLista = new();

        /// <summary>
        /// Lekéri az összes felhasználót a 'felhasznalo' táblánkból, az összesből csinál egy Felhasznalo példányt és hozzáadja a felhasznaloLista-ba.
        /// </summary>
        public static void felhasznalokFrissit()
        {
            //lementjük a visszakapott string listát
            List<string> eredmeny = MySQL.query("felhasznaloklistalekerdezes", false);
            //töröljük/újat csinálunk a listánkból hogy ne a régiekhez adjuk hozzá
            felhasznaloLista = new();

            //azon,nev,lak,tel,email,jog - 6al növeljük a ciklusváltozót
            for (int i = 0; i < eredmeny.Count; i += 6)
            {
                Felhasznalo felhasznalo = new(int.Parse(eredmeny[i]), eredmeny[i + 1], eredmeny[i + 2], eredmeny[i + 3], eredmeny[i + 4], int.Parse(eredmeny[i + 5]));
                felhasznaloLista.Add(felhasznalo);
            }
        }
    }
}
