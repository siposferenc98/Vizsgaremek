using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Vizsgaremek.Osztalyok
{
    internal class JelszoValtoztatEljaras
    {
        public static void jelszoValtoztat(object o, RoutedEventArgs e)
        {
            Window jelszoValtoztatUI = new JelszoValtoztat();
            jelszoValtoztatUI.Show();
        }
    }
}
