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

        public static void rendelesekFrissit()
        {
            List<int> eredmeny = MySQL.query("rendeleslekerdezes", false).Select(int.Parse).ToList();
            rendelesekLista = new();

            if (eredmeny.Any())
                for (int i = 0; i < eredmeny.Count; i += 5)
                {
                    Rendeles rendeles = new(eredmeny[i], eredmeny[i + 1], eredmeny[i + 2], eredmeny[i + 3], eredmeny[i + 4]);
                    rendelesekLista.Add(rendeles);
                }
        }


    }
}
