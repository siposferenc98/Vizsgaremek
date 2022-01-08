using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    class Rendelesek
    {
        public static List<Rendeles> rendelesekLista = new();

        /// <summary>
        /// Lekéri az összes rendelést a 'rendeles' táblánkból, az összesből csinál egy Rendeles példányt és hozzáadja a rendelesekLista-ba.
        /// </summary>
        public static void rendelesekFrissit()
        {
            //lementjük az eredményt
            List<string> eredmeny = MySQL.query("rendeleslekerdezes", false);
            //töröljük/újat csinálunk a listánkból hogy ne a régiekhez adjuk hozzá
            rendelesekLista = new();

            //razon,fazon,asztal,ido,etelstatus,italstatus - 6al növeljük a ciklusváltozónkat
            for (int i = 0; i < eredmeny.Count; i += 6)
            {
                Rendeles rendeles = new(int.Parse(eredmeny[i]), int.Parse(eredmeny[i + 1]),int.Parse(eredmeny[i + 2]),DateTime.Parse(eredmeny[i + 3]),int.Parse(eredmeny[i + 4]),int.Parse(eredmeny[i + 5]));
                rendelesekLista.Add(rendeles);
            }
        }


    }
}
