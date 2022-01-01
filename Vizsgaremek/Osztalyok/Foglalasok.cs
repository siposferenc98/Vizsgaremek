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

        public static void foglalasokFrissit()
        {
            List<string> eredmeny = MySQL.query("foglalaslekerdezes", false);
            foglalasLista = new();

            if (eredmeny.Any())
                for (int i = 0; i < eredmeny.Count; i += 7)
                {
                    Foglalas foglalas = new(int.Parse(eredmeny[i]), int.Parse(eredmeny[i+1]), int.Parse(eredmeny[i+2]), DateTime.Parse(eredmeny[i+3]), DateTime.Parse(eredmeny[i + 4]), DateTime.Parse(eredmeny[i + 5]), bool.Parse(eredmeny[i+6]));
                    foglalasLista.Add(foglalas);
                }
        }

    }
}
