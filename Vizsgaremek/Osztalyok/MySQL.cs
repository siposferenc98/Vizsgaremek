using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Vizsgaremek.Osztalyok
{
    public class MySQL
    {
        //MySqlConnection példányosítva, paraméterbe a connection stringet kapja meg.
        public static MySqlConnection conn = new("server=localhost;database=hamb;username=root;pwd=;sslmode=none;");

        /// <summary>
        /// Ez a funkció lefuttat egy queryt, vagy non queryt, és visszatér egy string típusú listával, amiben query esetében a sorok találhatóak, non query esetében az hogy sikeresen lefutott , vagy egy SQL hiba.
        /// </summary>
        /// <param name="command">Egy teljes SQL lekérdezés, szöveg formában.</param>
        /// <param name="nonQuery">True: ha sikerül akkor csak azt kapjuk vissza hogy sikerült-e vagy hibát, False: visszakapjuk a listát az értékekkel.</param>
        /// <returns></returns>
        public static List<string> query(string command, bool nonQuery)
        {

            List<string> result = new(); // eredmények, ezt töltjük fel és térünk vissza vele
            MySqlDataReader reader; //ezzel tudjuk sorrol sorra kiolvasni a sorokat a db-ből, .GetString() funkcióval kinyerni egy cella adatát pl.
            MySqlCommand comm = new(command , conn); //MySqlCommand egy példánya, első paraméterbe az SQL query stringet kapja a paraméterünkből, második paraméterként a connectiont.
            try
            {

                conn.Open();
                switch(nonQuery)
                {
                    case false:
                        reader = comm.ExecuteReader();
                        if (reader.HasRows)
                            while (reader.Read())
                                for (int i = 0; i < reader.FieldCount; i++)
                                    result.Add(reader.GetString(i));
                        break;

                    case true:
                        int sorok = comm.ExecuteNonQuery();
                        if (sorok > 0)
                            result.Add("A művelet sikeresen elvégezve!");
                        break;
                }
                
            }
            catch(Exception ex)
            {
                result.Add(ex.ToString());
            }
            conn.Close();//bezárjuk a csatlakozást.
            return result;
        }


        public static string hashPW(string pw)
        {
            byte[] passtobytearr = ASCIIEncoding.ASCII.GetBytes(pw);
            byte[] md5hashbytearr = new MD5CryptoServiceProvider().ComputeHash(passtobytearr);
            string md5hash = "";
            foreach (byte a in md5hashbytearr)
            {
                md5hash += a.ToString("x2");
            }
            return md5hash;
        }
    }
}
