using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizsgaremek.Osztalyok
{
    class Rendeles
    {
        public int razon, fazon, asztal, etelstatus, italstatus;

        public Rendeles(int razon, int fazon, int asztal, int etelstatus, int italstatus)
        {
            this.razon = razon;
            this.fazon = fazon;
            this.asztal = asztal;
            this.etelstatus = etelstatus;
            this.italstatus = italstatus;
        }

        //TODO SWITCH KIIRÁSRA, ETELSTATUS,ITALSTATUS 
        public override string ToString()
        {
            return $"{razon}. számú rendelés";
        }
    }
}
