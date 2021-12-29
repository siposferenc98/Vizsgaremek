using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    class Desszertek
    {
        public static List<Termek> desszertLista = new();

        public static void desszertekFrissit()
        {
            List<string> eredmeny = MySQL.query("desszerteklekerdezes", false);
            desszertLista = new();

            if (eredmeny.Any())
                for (int i = 0; i < eredmeny.Count; i += 4)
                {
                    Termek termek = new(int.Parse(eredmeny[i]), eredmeny[i + 1], int.Parse(eredmeny[i + 2]), eredmeny[i + 3]);
                    desszertLista.Add(termek);
                }
        }
    }
}
