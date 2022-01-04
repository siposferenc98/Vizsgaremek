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
            asztalokRajzol();
            aktualisVendegekSzamol();
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

        private void asztalokRajzol()
        {
            asztalok.Children.Clear();
            int x = 10;
            bool fentE = true;
            for (int i = 1; i <= 10; i++)
            {
                asztalok.Children.Add(Asztalok.rajzol(x, fentE, i));
                fentE = !fentE;
                if(i > 0 && fentE)
                    x += 35;
            }
        }

        private void aktualisVendegekSzamol()
        {
            Foglalasok.foglalasokFrissit();
            List<Rendeles> aktualisRendelesek = Rendelesek.rendelesekLista.Where(x => x.etelstatus < 4 && x.italstatus < 4).ToList();
            int vendegek = 0;
            foreach (Rendeles r in aktualisRendelesek)
            {
                vendegek += Foglalasok.foglalasLista.First(x => x.fazon == r.fazon).szemelydb;
            }
            aktualisVendegek.Content = vendegek;
        }

        private void kijelentkezes(object sender, RoutedEventArgs e)
        {
            AktualisFelhasznalo.felhasznalo = null;
            Window bejelentkezes = new Bejelentkezes();
            bejelentkezes.Show();
            Close();
        }


        private void felszolgaloraValtas(object sender, RoutedEventArgs e)
        {
            Window felszolgaloUI = new Felszolgalo.FelszolgaloUI();
            felszolgaloUI.Owner = this;
            felszolgaloUI.Show();
        }

        private void szakacsraValtas(object sender, RoutedEventArgs e)
        {
            Window szakacsUI = new Szakacs.SzakacsUI();
            szakacsUI.Owner = this;
            szakacsUI.Show();
        }

        private void pultosraValtas(object sender, RoutedEventArgs e)
        {
            Window pultosUI = new Pultos.PultosUI();
            pultosUI.Owner = this;
            pultosUI.Show();
        }

        private void frissites(object sender, RoutedEventArgs e)
        {
            bevetelSzamol();
            asztalokRajzol();
            aktualisVendegekSzamol();
        }
    }
}
