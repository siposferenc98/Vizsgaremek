using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    public class Termekek
    {
        /*public static List<Termek> hamburgerLista = new();
        public static List<Termek> koretLista = new();
        public static List<Termek> desszertLista = new();
        public static List<Termek> italLista = new();*/

        public static Dictionary<char, List<Termek>> listakDictionary = new()
        {
            { 'h', new() },
            { 'k', new() },
            { 'd', new() },
            { 'i', new() }
        };

        public static void listaFrissit(string command, char listaKezdobetu)
        {
            List<string> eredmeny = MySQL.query(command, false);
            listakDictionary[listaKezdobetu] = new();

            if (eredmeny.Any())
                for (int i = 0; i < eredmeny.Count; i += 4)
                {
                    Termek termek = new(int.Parse(eredmeny[i]), eredmeny[i + 1], int.Parse(eredmeny[i + 2]), eredmeny[i + 3]);
                    listakDictionary[listaKezdobetu].Add(termek);
                }
        }

    }
}
