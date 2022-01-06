using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using MySql.Data.MySqlClient;

namespace Vizsgaremek.Felszolgalo
{
    /// <summary>
    /// Interaction logic for FelszolgaloUI.xaml
    /// </summary>
    public partial class FelszolgaloUI : Window
    {
        
        public FelszolgaloUI()
        {
            InitializeComponent();
            comboListBoxokFrissites();
            Task.Run(() => listboxokFrissitAsync());
            
        }

        //Async frissítések
        private async void listboxokFrissitAsync()
        {

            while (true)
            {
                rendelesekFrissitListBoxokFeltolt();
                await Task.Delay(10000);
            }
            
        }

        private void rendelesekFrissitListBoxokFeltolt()
        {
            Dispatcher.Invoke(() =>
            {
                Rendelesek.rendelesekFrissit();
                listboxokFeltolt();
            });
            
        }

        //Listboxok,comboboxok feltöltése
        private void listboxokFeltolt()
        {
            felvettRendelesek.Items.Clear();
            keszRendelesek.Items.Clear();
            

            if (Rendelesek.rendelesekLista.Any())
                foreach (Rendeles r in Rendelesek.rendelesekLista)
                {
                    if (r.etelstatus == 1 && r.italstatus == 1)
                        felvettRendelesek.Items.Add(r);
                    else if(r.etelstatus < 4 && r.italstatus < 4)
                        keszRendelesek.Items.Add(r);
                }
        }

        private void rendelesekComboBoxFeltolt()
        {
            rendelesekComboBox.Items.Clear();
            foreach(Rendeles r in Rendelesek.rendelesekLista)
            {
                if(r.etelstatus < 4 && r.italstatus < 4)
                    rendelesekComboBox.Items.Add(r.razon);
            }
        }

        private void asztalokComboBoxFeltolt()
        {
            asztalokComboBox.Items.Clear();
            Rendelesek.rendelesekFrissit();
            for (int i = 0; i < 10; i++)
            {
                if (Rendelesek.rendelesekLista.Any(x => x.asztal == i && x.etelstatus < 4))
                    continue;
                asztalokComboBox.Items.Add(i);
            }
        }

        private void foglalasComboBoxFeltolt()
        {
            foglalasComboBox.Items.Clear();
            Felhasznalok.felhasznalokFrissit();
            Foglalasok.foglalasokFrissit();
            foreach (Foglalas f in Foglalasok.foglalasLista.Where(x=>x.megjelent == false && x.idopontDatum.Month == DateTime.Now.Month && x.idopontDatum.Day == DateTime.Now.Day))
            {
                foglalasComboBox.Items.Add(f);
            }
        }

        private void comboListBoxokFrissites()
        {
            asztalokComboBoxFeltolt();
            foglalasComboBoxFeltolt();
            rendelesekComboBoxFeltolt();
            listboxokFeltolt();
            asztalokRajzol();
        }


        //Onselectionchanged eventekre 
        private void tetelUIIsEnabled(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.SelectedItem is not null)
                tetelUIGomb.IsEnabled = true;
            else
                tetelUIGomb.IsEnabled = false;
        }
        private void listBoxVezerloIsEnabled(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            StackPanel vezerlo = (StackPanel)FindName(lb.Name+"Vezerlo");
            if (lb.SelectedItem is not null)
                vezerlo.IsEnabled = true;
            else
                vezerlo.IsEnabled = false;
        }
        private void rendelesFelvetelIsEnabled(object sender, SelectionChangedEventArgs e)
        {
            if (asztalokComboBox.SelectedItem is not null)
                rendelesFelvetelGomb.IsEnabled = true;
            else
                rendelesFelvetelGomb.IsEnabled = false;
        }


        //Új ablakok megnyitása
        private void tetelUiMegnyit(object sender, RoutedEventArgs e)
        {
            Window tetelUI = new TetelUI((int)rendelesekComboBox.SelectedItem);
            tetelUI.Owner = this;
            tetelUI.Show();
        }
        private void rendelesReszletek(object sender, RoutedEventArgs e)
        {
            Button reszletekgomb = (Button)sender;
            string nev = reszletekgomb.Name;
            Rendeles rendeles = nev == "felvettRendelesekReszletek" ? (Rendeles)felvettRendelesek.SelectedItem : (Rendeles)keszRendelesek.SelectedItem;

            Window reszletek = new RendelesReszletekUI(rendeles);
            reszletek.Owner = this;
            reszletek.Show();
        }


        //Mysql törlés,beszúrás
        private void rendelesFelvetel(object sender, RoutedEventArgs e)
        {
            Foglalas foglalas = new();
            if (foglalasComboBox.SelectedItem is not null)
            {
                foglalas = (Foglalas)foglalasComboBox.SelectedItem;
            }
            else
            {
                List<string> foglalase = MySQL.query("vendegfoglalasbeszur", true);
                Foglalasok.foglalasokFrissit();
                foglalas = Foglalasok.foglalasLista.Last();
            }

            
            MySqlParameter foglalasparam = new("@fazon", foglalas.fazon);
            MySqlParameter asztalparam = new("@asztal", asztalokComboBox.SelectedValue);
            List<MySqlParameter> rendelesFelvetelParams = new() { foglalasparam, asztalparam };
            List<string> eredmeny = MySQL.query("rendelesfelvetel", true, rendelesFelvetelParams);
            MessageBox.Show(eredmeny[0]);
            comboListBoxokFrissites();
        }


        private void rendelesTorles(object sender, RoutedEventArgs e)
        {
            Button torlesGomb = (Button)sender;
            string nev = torlesGomb.Name;
            Rendeles rendeles = nev == "felvettRendelesekTorles" ? (Rendeles)felvettRendelesek.SelectedItem : (Rendeles)keszRendelesek.SelectedItem;

            MessageBoxResult megerosites = MessageBox.Show($"Biztosan törölni akarod a {rendeles.razon} számú rendelést?", "Rendelés Törlése", MessageBoxButton.OKCancel);
            if (megerosites == MessageBoxResult.OK)
            {
                MySqlParameter razonparam = new("@razon", rendeles.razon);
                List<MySqlParameter> rendelesTorlesParams = new() { razonparam };
                List<string> eredmeny = MySQL.query("rendelestorles", true, rendelesTorlesParams);
                MessageBox.Show(eredmeny[0]);
                comboListBoxokFrissites();
            }

        }

        private void fizetesreVar(object sender, RoutedEventArgs e)
        {
            Rendeles rendeles = (Rendeles)keszRendelesek.SelectedItem;
            Button allapotvaltoztatgomb = (Button)sender;
            string nev = allapotvaltoztatgomb.Name;
            int allapot = nev == "fVar" ? 3 : 4;

            MessageBoxResult megerosites = MessageBox.Show($"Biztosan meg akarod változtatni a {rendeles.razon} számú rendelés állapotát?", "Rendelés állapot", MessageBoxButton.OKCancel);
            if(megerosites == MessageBoxResult.OK)
            {
                MySqlParameter razonparam = new("@razon", rendeles.razon);
                MySqlParameter allapotparam = new("@allapot", allapot);
                List<MySqlParameter> rendelesAllapotValtoztat = new() { razonparam, allapotparam};
                List<string> eredmeny = MySQL.query("rendelesallapotvaltoztat", true, rendelesAllapotValtoztat);
                MessageBox.Show(eredmeny[0]);
                comboListBoxokFrissites();
            }
        }

        private void kijelentkezes(object sender, RoutedEventArgs e)
        {
            AktualisFelhasznalo.felhasznalo = null;
            Window bejelentkezes = new Bejelentkezes();
            bejelentkezes.Show();
            Close();
        }

        private void vendegChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.IsChecked == true)
            {
                foglalasComboBox.SelectedItem = null;
                foglalasComboBox.IsEnabled = false;
            }
            else
                foglalasComboBox.IsEnabled = true;
        }

        private void vezerlokFrissitese(object sender, RoutedEventArgs e)
        {
            comboListBoxokFrissites();
        }

        private void asztalokRajzol()
        {
            asztalokCanvas.Children.Clear();
            double x = 50;
            bool fentE = true;
            for (int i = 1; i <= 10; i++)
            {
                asztalokCanvas.Children.Add(Asztalok.rajzol(x, fentE, i));
                fentE = !fentE;
                if (i > 0 && fentE)
                    x += 35;
            }
        }
    }
}
