using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    class Italok
    {
        public static List<Termek> italLista = new();

        public static void italokFrissit()
        {
            List<string> eredmeny = MySQL.query("itallekerdezes", false);
            italLista = new();

            if (eredmeny.Any())
                for (int i = 0; i < eredmeny.Count; i += 3)
                {
                    Termek termek = new(int.Parse(eredmeny[i]), eredmeny[i + 1], int.Parse(eredmeny[i + 2]));
                    italLista.Add(termek);
                }
        }
    }
}
