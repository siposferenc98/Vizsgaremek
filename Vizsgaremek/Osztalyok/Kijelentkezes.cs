using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Vizsgaremek.Osztalyok
{
    internal class Kijelentkezes
    {
        public static void kijelentkezes(object sender, RoutedEventArgs e)
        {
            AktualisFelhasznalo.felhasznalo = null;
            Window bejelentkezes = new Bejelentkezes();
            bejelentkezes.Show();
            foreach (Window window in Application.Current.Windows)
            {
                if (window != bejelentkezes)
                {
                    window.Close();
                }
            }
        }
    }
}
