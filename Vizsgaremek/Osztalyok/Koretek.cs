using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    class Koretek
    {
        public static List<Termek> koretLista = new();

        public static void koretekFrissit()
        {
            List<string> eredmeny = MySQL.query("koretlekerdezes", false);
            koretLista = new();

            if (eredmeny.Any())
                for (int i = 0; i < eredmeny.Count; i += 4)
                {
                    Termek termek = new(int.Parse(eredmeny[i]), eredmeny[i + 1], int.Parse(eredmeny[i + 2]), eredmeny[i + 3]);
                    koretLista.Add(termek);
                }
        }
    }
}
