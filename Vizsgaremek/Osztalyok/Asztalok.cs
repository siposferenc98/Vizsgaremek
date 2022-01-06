using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Vizsgaremek.Osztalyok
{
    class Asztalok
    {
        public static Viewbox rajzol(double x, bool fentE, int asztalszam)
        {
            List<Rendeles> aktivRendelesek = Rendelesek.rendelesekLista.Where(x => x.etelstatus < 4 && x.italstatus < 4).ToList();
            Viewbox vb = new();
            vb.Tag = asztalszam;
            vb.MouseLeftButtonDown += asztalRendelesReszletekUI;
            vb.Width = 30;
            vb.Height = 30;

            Grid grid = new();
            grid.Width = 20;
            grid.Height = 20;
            vb.Child = grid;

            Ellipse el = new();
            el.Stroke = new SolidColorBrush(Colors.Black);
            if (aktivRendelesek.Any(x => x.asztal == asztalszam))
                el.Fill = new SolidColorBrush(Colors.Red);

            TextBlock tb = new();
            tb.TextAlignment = TextAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Text = asztalszam.ToString();

            grid.Children.Add(el);
            grid.Children.Add(tb);
            Canvas.SetLeft(vb, x);
            if (!fentE)
                Canvas.SetTop(vb, 55);

            return vb;
        }

        private static void asztalRendelesReszletekUI(object sender, RoutedEventArgs e)
        {
            Viewbox el = (Viewbox)sender;
            Rendeles rendeles = Rendelesek.rendelesekLista.FirstOrDefault(x => x.asztal == (int)el.Tag && x.etelstatus < 4 && x.italstatus < 4);
            if (rendeles is not null)
            {
                Window rendelesReszletekUI = new Felszolgalo.RendelesReszletekUI(rendeles);
                rendelesReszletekUI.Show();
            }
            else
                MessageBox.Show("Az asztal üres / nincs még rendelés felvéve hozzá!");

        }
    }
}
