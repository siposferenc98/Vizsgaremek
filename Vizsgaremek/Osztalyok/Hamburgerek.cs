using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    class Hamburgerek
    {
        public static List<Termek> hamburgerLista = new();

        public static void hamburgerekFrissit()
        {
            List<string> eredmeny = MySQL.query("hamburgerlekerdezes", false);
            hamburgerLista = new();

            if (eredmeny.Any())
                for (int i = 0; i < eredmeny.Count; i += 4)
                {
                    Termek termek = new(int.Parse(eredmeny[i]),int.Parse(eredmeny[i+1]),eredmeny[i+2],eredmeny[i+3]);
                    hamburgerLista.Add(termek);
                }
        }
    }
}
