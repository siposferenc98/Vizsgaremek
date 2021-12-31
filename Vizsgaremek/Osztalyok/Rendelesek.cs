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
            List<string> eredmeny = MySQL.query("rendeleslekerdezes", false);
            rendelesekLista = new();

            if (eredmeny.Any())
                for (int i = 0; i < eredmeny.Count; i += 6)
                {
                    Rendeles rendeles = new(int.Parse(eredmeny[i]), int.Parse(eredmeny[i + 1]),int.Parse(eredmeny[i + 2]),DateTime.Parse(eredmeny[i + 3]),int.Parse(eredmeny[i + 4]),int.Parse(eredmeny[i + 5]));
                    rendelesekLista.Add(rendeles);
                }
        }


    }
}
