using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Vizsgaremek.Osztalyok;

namespace Vizsgaremek.Admin
{
    /// <summary>
    /// Interaction logic for AdminUI.xaml
    /// </summary>
    public partial class AdminUI : Window
    {
        public AdminUI()
        {
            InitializeComponent();
            bevetelSzamol();
            asztalokRajzolCiklus();
        }

        private void bevetelSzamol()
        {
            Rendelesek.rendelesekFrissit();
            List<Rendeles> fizetettRendelesek = Rendelesek.rendelesekLista.Where(x => x.etelstatus == 4 && x.italstatus == 4).ToList();
            int maiOsszeg = fizetettRendelesek.Where(x => x.ido.Date == DateTime.Today).Sum(x => x.vegosszeg);
            int haviOsszeg = fizetettRendelesek.Where(x => x.ido.Month == DateTime.Today.Month).Sum(x => x.vegosszeg);
            int osszesOsszeg = fizetettRendelesek.Sum(x => x.vegosszeg);

            maiBevetel.Content = $"{maiOsszeg} Ft.";
            haviBevetel.Content = $"{haviOsszeg} Ft.";
            osszesBevetel.Content = $"{osszesOsszeg} Ft.";
        }

        private void asztalokRajzolCiklus()
        {
            int x = 10;
            bool fentE = true;
            for (int i = 1; i <= 10; i++)
            {
                asztalokRajzol(x, fentE, i);
                fentE = !fentE;
                if(i > 0 && fentE)
                    x += 35;
            }
        }

        private void asztalokRajzol(int x, bool fentE, int asztalszam)
        {
            List<Rendeles> aktivRendelesek = Rendelesek.rendelesekLista.Where(x => x.etelstatus < 4 && x.italstatus < 4).ToList();
            Canvas canvas = asztalok;
            Viewbox vb = new();
            vb.Tag = asztalszam;
            vb.MouseLeftButtonDown += test;
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
            if(!fentE)
                Canvas.SetTop(vb, 55);

            canvas.Children.Add(vb);
        }

        private void kijelentkezes(object sender, RoutedEventArgs e)
        {
            AktualisFelhasznalo.felhasznalo = null;
            Window bejelentkezes = new Bejelentkezes();
            bejelentkezes.Show();
            Close();
        }

        private void test(object sender, RoutedEventArgs e)
        {
            Viewbox el = (Viewbox)sender;
            Rendeles rendeles = Rendelesek.rendelesekLista.FirstOrDefault(x=> x.asztal == (int)el.Tag && x.etelstatus < 4 && x.italstatus < 4);
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
