using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.IO;

namespace Vizsgaremek.Osztalyok
{
    public class MySQL
    {
        //MySqlConnection példányosítva, paraméterbe a connection stringet kapja meg.
        public static MySqlConnection conn = new("server=localhost;database=hamb;username=root;pwd=;sslmode=none;");
        //ebben tároljuk az összes sql lekérdezést, formátum : 'kulcs'-'lekérdezés'
        private static Dictionary<string, string> lekerdezesekDict = new();

        /// <summary>
        /// Ez a funkció lefuttat egy queryt, vagy non queryt, és visszatér egy string típusú listával, amiben query esetében a sorok találhatóak, non query esetében az hogy sikeresen lefutott , vagy egy SQL hiba.
        /// </summary>
        /// <param name="command">Egy kulcs a dictionaryhoz.</param>
        /// <param name="nonQuery">True: ha sikerül akkor csak azt kapjuk vissza hogy sikerült-e vagy hibát, False: visszakapjuk a listát az értékekkel.</param>
        /// <param name="parameterek">Opcionális paraméter, egy lista feltöltve MySqlParameter-ekkel, amik szükségesek a query végrehajtásához.</param>
        /// <returns></returns>
        public static List<string> query(string command, bool nonQuery, List<MySqlParameter> parameterek = null)
        {

            List<string> result = new(); // eredmények, ezt töltjük fel és térünk vissza vele
            MySqlDataReader reader; //ezzel tudjuk sorrol sorra kiolvasni a sorokat a db-ből, .GetString() funkcióval kinyerni egy cella adatát pl.
            MySqlCommand comm = new(lekerdezesekDict[command] , conn); //MySqlCommand egy példánya, első paraméterbe az SQL query stringet kapja a paraméterünkből, második paraméterként a connectiont.
            
            //végigmegyünk a paraméterek listán ha nem null(meg lett adva), és a MySqlCommandhoz hozzáadjuk a paramétereket.
            if(parameterek is not null)
                foreach (MySqlParameter par in parameterek)
                    comm.Parameters.Add(par);
            
            try
            {
                //Létrehozzuk a kapcsolatot, megnézzük hogy a műveletnél kérjük e vissza az értékeket
                conn.Open();
                switch(nonQuery)
                {
                    //Ha nem nonQuery, akkor kérjük szóval
                    case false:
                        reader = comm.ExecuteReader(); //a datareader objektumunk = lesz a mysqlcommand.executereader funkcióval
                        if (reader.HasRows) //ha a lekérdezés visszatért sorokkal
                            while (reader.Read()) //akkor ameddig tudjuk olvasni a sorokat
                                for (int i = 0; i < reader.FieldCount; i++) //végigmegyünk egy sor celláin (FieldCount = hány cellából áll egy sor)
                                    result.Add(reader.GetString(i)); //és hozzáadjuk a visszatérési Listánkhoz
                        break;

                    // Amennyiben nonQuery
                    case true:
                        int sorok = comm.ExecuteNonQuery(); //lementjük egy int változóba hogy hány sor lett érintett
                        if (sorok > 0) //ha több mint 0 akkor sikeresen elvégezte
                            result.Add("A művelet sikeresen elvégezve!");
                        break;
                }
                
            }
            catch(Exception ex)
            {
                //Minden hibát elkapunk és belerakjuk a visszatérési tömbbe,majd visszatérünk vele.
                result.Add(ex.ToString());
            }
            conn.Close();//bezárjuk a csatlakozást.
            return result;
        }

        /// <summary>
        /// Ez a funkció készít egy MD5 hasht egy beküldött jelszóból.
        /// </summary>
        /// <param name="pw">Jelszó szöveg formában.</param>
        /// <returns>Visszatér egy 32 hosszú (hash)string-el.</returns>
        public static string hashPW(string pw)
        {
            byte[] passtobytearr = ASCIIEncoding.ASCII.GetBytes(pw); //A beküldött jelszót átalakítjuk egy byte tömbbé.
            byte[] md5hashbytearr = new MD5CryptoServiceProvider().ComputeHash(passtobytearr); //A byte tömbből kiszámítjuk a hasht, egy adott szövegből a hash mindig ugyanaz lesz, bejelentkezésnél már hashelve küldjük be a db-be, és a 2 hasht hasonlítjuk össze.
            string md5hash = "";
            foreach (byte a in md5hashbytearr)
            {
                md5hash += a.ToString("x2"); //visszaalakítjuk hexadecimális értékeké
            }
            return md5hash;
        }

        /// <summary>
        /// Feltölti az osztály szintű dictionarynkat egy txt fileból, ami kötőjelnél tördeli fel az utasítás nevét a parancstól.
        /// </summary>
        public static void dictionaryFeltolt()
        {
            StreamReader sr = new("./sql.txt");
            while (!sr.EndOfStream)
            {
                List<string> sor = sr.ReadLine().Split("-").ToList(); //beolvasunk egy sort, feltördeljük '-'-nél, és listát csinálunk belőle.
                lekerdezesekDict.Add(sor[0], sor[1]); //Lista 0. eleme az utasítás neve, 1. maga a parancs.
            }
        }
    }
}
