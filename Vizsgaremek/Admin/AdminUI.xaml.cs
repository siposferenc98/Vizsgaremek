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

        //Bevétel labelek, aktuális vendégek frissítése
        #region Labelek frissitese
        /// <summary>
        /// Kiszámolja a már kifizetett rendelésekből a napi,havi,összes bevételt.
        /// </summary>
        private void bevetelSzamol()
        {
            Rendelesek.rendelesekFrissit(); //ráfrissítünk a rendelésekre
            //Csak a 4-4es statussal rendelkező rendeléseket (már fizetett) szeretnénk
            List<Rendeles> fizetettRendelesek = Rendelesek.rendelesekLista.Where(x => x.etelstatus == 4 && x.italstatus == 4).ToList();
            //dátumokat egyeztetünk és Sum()oljuk a végösszegeket
            int maiOsszeg = fizetettRendelesek.Where(x => x.ido.Date == DateTime.Today).Sum(x => x.vegosszeg);
            int haviOsszeg = fizetettRendelesek.Where(x => x.ido.Month == DateTime.Today.Month).Sum(x => x.vegosszeg);
            int osszesOsszeg = fizetettRendelesek.Sum(x => x.vegosszeg);

            maiBevetel.Content = $"{maiOsszeg} Ft.";
            haviBevetel.Content = $"{haviOsszeg} Ft.";
            osszesBevetel.Content = $"{osszesOsszeg} Ft.";
        }

        /// <summary>
        /// Kiszámolja az aktuálisan bent tartozkodó vendégek számát.
        /// </summary>
        private void aktualisVendegekSzamol()
        {
            Foglalasok.foglalasokFrissit();//ráfrissítünk a foglalásokra
            Rendelesek.rendelesekFrissit();//meg a rendelésekre
            //csak a 4es statusnál kisebb rendelések (még nincsenek fizetve) kellenek
            List<Rendeles> aktualisRendelesek = Rendelesek.rendelesekLista.Where(x => x.etelstatus < 4 && x.italstatus < 4).ToList();
            int vendegek = 0; //default 0 vendég ül bent, ehhez adogatjuk hozzá
            foreach (Rendeles r in aktualisRendelesek) //végigmegyünk a rendeléseken
            {
                vendegek += Foglalasok.foglalasLista.First(x => x.fazon == r.fazon).szemelydb; //és a hozzá tartozó foglalás személy db számát hozzáadjuk a vendégekhez
            }
            aktualisVendegek.Content = vendegek;
        }

        /// <summary>
        /// Lefrissíti az összes adatot(bevételek,asztalok,vendégek).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frissites(object sender, RoutedEventArgs e)
        {
            bevetelSzamol();
            asztalokRajzol();
            aktualisVendegekSzamol();
        }
        #endregion

        //Új ablakok megnyitása
        #region Uj ablakok
        private void termekekModositasa(object sender, RoutedEventArgs e)
        {
            Window termekekUI = new TermekekUI();
            termekekUI.Owner = this;
            termekekUI.Show();
        }

        private void rendelesElozmenyek(object sender, RoutedEventArgs e)
        {
            Window rendelesElozmenyekUI = new RendelesElozmenyekUI();
            rendelesElozmenyekUI.Owner = this;
            rendelesElozmenyekUI.Show();
        }

        private void felhasznalokModositasa(object sender, RoutedEventArgs e)
        {
            Window felhasznalokUI = new FelhasznalokModositasaUI();
            felhasznalokUI.Owner = this;
            felhasznalokUI.Show();
        }

        private void foglalasokListaMegnyit(object sender, RoutedEventArgs e)
        {
            Window foglalasokUI = new FoglalasokUI();
            foglalasokUI.Owner = this;
            foglalasokUI.Show();
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
        #endregion

        //Asztalok rajzolása, kijelentkezés
        #region Egyeb eljarasok
        private void asztalokRajzol()
        {
            asztalok.Children.Clear();
            double x = 40;
            bool fentE = true;
            for (int i = 1; i <= 10; i++)
            {
                asztalok.Children.Add(Asztalok.rajzol(x, fentE, i));
                fentE = !fentE;
                if (i > 0 && fentE)
                    x += 35;
            }
        }
        private void kijelentkezes(object sender, RoutedEventArgs e)
        {
            AktualisFelhasznalo.felhasznalo = null;
            Window bejelentkezes = new Bejelentkezes();
            bejelentkezes.Show();
            Close();
        }
        #endregion
    }
}
