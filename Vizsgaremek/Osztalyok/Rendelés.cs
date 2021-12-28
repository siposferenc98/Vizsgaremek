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
                return $"({razon}.r.sz) Asztal: {asztal}, fizetésre vár!";
            if (etelstatus == 4 && italstatus == 4)
                return $"({razon}.r.sz) Asztal: {asztal}, fizetve!";

            return $"({razon}.r.sz) Asztal: {asztal}, étel:{eStatus} ital: {iStatus}";
        }
    }
}
