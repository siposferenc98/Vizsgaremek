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
        /// <summary>
        /// 20x20as Elipsziseket rajzol, benne az asztal számával, piros a háttere ha az asztal foglalt.
        /// </summary>
        /// <param name="x">Balról milyen messzi legyen az asztal pixelben.</param>
        /// <param name="fentE">Ha true akkor nincs fentről távolság, ha false akkor 55 pixelre lejebb tolja.</param>
        /// <param name="asztalszam">Asztal száma.</param>
        /// <returns>Viewbox amibe van egy grid, abban az elipszis,rajta a szám.</returns>
        public static Viewbox rajzol(double x, bool fentE, int asztalszam)
        {
            //Ebbe a listába van minden még nem fizetett rendelés (etel,italstatus < 4)
            List<Rendeles> aktivRendelesek = Rendelesek.rendelesekLista.Where(x => x.etelstatus < 4 && x.italstatus < 4).ToList();

            Viewbox vb = new(); //A benne lévő elemet a maximális méretére kinyújtja.
            vb.Tag = asztalszam; //hozzárendeljük az asztalszámot mert szükségünk lesz még rá.
            vb.MouseLeftButtonDown += asztalRendelesReszletekUI; //balclickre feliratkoztatjuk
            vb.Width = 30;
            vb.Height = 30;

            Grid grid = new(); //ez lesz a viewbox eleme amit ki fog húzni, gridbe több elem is lehet
            grid.Width = 20;
            grid.Height = 20;
            vb.Child = grid;

            Ellipse el = new(); //új kör, ami akkora lesz mint a grid amibe belerakjuk
            el.Stroke = new SolidColorBrush(Colors.Black); //körvonal szín
            if (aktivRendelesek.Any(x => x.asztal == asztalszam)) //hogyha megtalálható az asztalszám a listánkba
                el.Fill = new SolidColorBrush(Colors.Red); //akkor van rendelés rá felvéve, szóval piros lesz a háttere

            TextBlock tb = new(); //ebbe rakjuk az asztal számot amit középre igazítunk
            tb.TextAlignment = TextAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Text = asztalszam.ToString();

            grid.Children.Add(el);//grid megkapja először a kört(+ a piros kitöltést ha van)
            grid.Children.Add(tb);//rá rajzolva a számot
            Canvas.SetLeft(vb, x);//pozicionáljuk a viewboxot
            if (!fentE)//ha nem fenti kör akkor letoljuk 55 pixellel
                Canvas.SetTop(vb, 55);

            return vb; //visszatérünk a viewbox elemmel
        }


        /// <summary>
        /// Asztal kattintásánál fut le, a sender a ViewBox lesz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void asztalRendelesReszletekUI(object sender, RoutedEventArgs e)
        {
            Viewbox vb = (Viewbox)sender; //lementjük a sendert átcastolva viewbox-á

            //megpróbáljuk kiszedni a hozzá tartozó aktív rendelést a rendelések lista közül, ha nem talál ilyet akkor default(null) lesz a lista.
            Rendeles rendeles = Rendelesek.rendelesekLista.FirstOrDefault(x => x.asztal == (int)vb.Tag && x.etelstatus < 4 && x.italstatus < 4);

            if (rendeles is not null)//ha nem null, szóval van rendelésünk
            {
                //akkor megnyitunk egy rendelés részletek uit, paraméterbe átadjuk neki a rendelésünket
                Window rendelesReszletekUI = new Felszolgalo.RendelesReszletekUI(rendeles);
                rendelesReszletekUI.Show();
            }
            else
                MessageBox.Show("Az asztal üres / nincs még rendelés felvéve hozzá!");

        }
    }
}
