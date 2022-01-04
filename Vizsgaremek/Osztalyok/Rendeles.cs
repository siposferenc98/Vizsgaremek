using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Vizsgaremek.Osztalyok
{
    public class Rendeles
    {
        public int razon, fazon, asztal, etelstatus, italstatus;
        public DateTime ido;
        public List<Tetel> tetelek;
        //Amint van tétel lista, összeadjuk a végösszegeket belőlük és az lesz a rendelés végösszege, különben 0.
        public int vegosszeg => tetelek is not null ? tetelek.Sum(x=>x.vegosszeg) : 0 ;

        public Rendeles(int razon, int fazon, int asztal, DateTime ido, int etelstatus, int italstatus)
        {
            this.razon = razon;
            this.fazon = fazon;
            this.asztal = asztal;
            this.etelstatus = etelstatus;
            this.italstatus = italstatus;
            this.ido = ido;
            tetelekFrissit();
        }

        public void tetelekFrissit()
        {
            tetelek = new();
            MySqlParameter razonparam = new("@razon",razon);
            List<MySqlParameter> param = new() { razonparam };
            List<string> eredmeny = MySQL.query("rendelestetelek", false, param);

            for (int i = 0; i < eredmeny.Count; i += 13)
            {
                Tetel tetel = new(int.Parse(eredmeny[i]), int.Parse(eredmeny[i + 1]), int.Parse(eredmeny[i + 2]), int.Parse(eredmeny[i + 3]), int.Parse(eredmeny[i + 4]), int.Parse(eredmeny[i + 5]), int.Parse(eredmeny[i + 6]), int.Parse(eredmeny[i + 7]), int.Parse(eredmeny[i + 8]), int.Parse(eredmeny[i + 9]), int.Parse(eredmeny[i + 10]), int.Parse(eredmeny[i + 11]), eredmeny[i + 12]);
                tetelek.Add(tetel);
            }
        }


        //Ez lesz meghívva minden alkalommal amikor belerakjuk egy listboxba,comboboxba, vagy csak simán kiiratjuk.
        public override string ToString()
        {
            string eStatus = "Folyamatban";
            string iStatus = "Folyamatban";
            if (etelstatus > 1)
                eStatus = "Kész!";
            if (italstatus > 1)
                iStatus = "Kész!";
            if(etelstatus == 2 && italstatus == 2)
                return $"({razon}.r.sz) Asztal: {asztal}, felszolgálásra vár!";
            if (etelstatus == 3 && italstatus == 3)
                return $"({razon}.r.sz) Asztal: {asztal}, fizetésre vár, összeg: {vegosszeg}!";
            if (etelstatus == 4 && italstatus == 4)
                return $"({razon}.r.sz) Asztal: {asztal}, fizetve!";

            return $"({razon}.r.sz) Asztal: {asztal}, étel:{eStatus} ital: {iStatus}";
        }
    }
}
