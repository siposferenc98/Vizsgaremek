using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Vizsgaremek.Osztalyok
{
    class RegexClass
    {
        /// <summary>
        /// TextBoxoknak a PreviewTextInput eventjére iratjuk fel ahol azt szeretnénk hogy csak számokat lehessen beütni.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void csakSzamok(object sender, TextCompositionEventArgs e)
        {
            Regex szamokPattern = new Regex("[^0-9]+"); //Regex pattern , NEM(^) SZÁMOK(0-9)
            //TextComposition argumenteket kapjuk meg, ha a .Handled TRUE, akkor azt jelenti számára hogy nem kell megváltoztatni a TextCompositiont, mert már le lett kezelve, ha false akkor nem csinál semmit és hagyja beírni a szöveget.
            e.Handled = szamokPattern.IsMatch(e.Text);

            //Ebben az esetben a Regexünk akkor fog Matchelni, ha NEM számokat írunk bele, szóval a szamokPattern visszatér ilyenkor TRUE-val, és a .Handled = true lesz, szóval nem változik meg (üresről) a textboxunk tartalma, ha NEM matchel akkor azt jelenti hogy számokat írtunk be, szóval a .Handled = false lesz, beíródik a szám.
        }
    }
}
