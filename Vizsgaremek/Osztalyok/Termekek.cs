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

        // Kulcsokhoz kötött listák egy dictionaryben, h-hamburger,k-köret,d-desszert,i-ital, minden kulcshoz tartozik egy termék típusú lista.
        public static Dictionary<char, List<Termek>> listakDictionary = new()
        {
            { 'h', new() },
            { 'k', new() },
            { 'd', new() },
            { 'i', new() }
        };

        /// <summary>
        /// Ez az eljárás törli és újra feltölti a megadott listát (kulcs alapján) a listakDictionary dictionary-ben.
        /// </summary>
        /// <param name="command">SQL parancs az SQL parancsok dictionaryből</param>
        /// <param name="listaKezdobetu">Egy betű ami a listakDictionaryban a kulcs lesz, h-hamburger,k-köret,d-desszert,i-ital.</param>
        public static void listaFrissit(string command, char listaKezdobetu)
        {
            List<string> eredmeny = MySQL.query(command, false); //lekérjük az adott tábla értékeit
            listakDictionary[listaKezdobetu] = new(); //töröljük/új üres listára cseréljük ki az adott listát kulcs alapján

            if (eredmeny.Any())
            {
                for (int i = 0; i < eredmeny.Count; i += 4) //Minden termék a DB-ben 4 oszlopból áll
                {
                    //1.azon - int, 2.nev - string, 3.ar - int, 4.leir - string.
                    Termek termek = new(int.Parse(eredmeny[i]), eredmeny[i + 1], int.Parse(eredmeny[i + 2]), eredmeny[i + 3]);
                    listakDictionary[listaKezdobetu].Add(termek); //hozzáadjuk az adott listához a dictionaryben.
                }
            }
        }

        public static void mindenListaFrissit()
        {
            listaFrissit("hamburgerlekerdezes", 'h');
            listaFrissit("koretlekerdezes", 'k');
            listaFrissit("desszerteklekerdezes", 'd');
            listaFrissit("itallekerdezes", 'i');
        }
    }
}
