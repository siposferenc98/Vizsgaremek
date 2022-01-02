using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RendelesReszletekUI.xaml
    /// </summary>
    public partial class RendelesReszletekUI : Window
    {
        private readonly Rendeles rendeles;
        private Tetel tetel;
        private int hdb, kdb, ddb, idb;
        public RendelesReszletekUI(Rendeles rendeles)
        {
            InitializeComponent();
            this.rendeles = rendeles;
            this.rendeles.tetelekFrissit();
            tetelekListBoxFeltolt();
            comboBoxokFeltolt();
        }

        private void tetelekListBoxFeltolt()
        {
            tetelekListBox.Items.Clear();

            foreach (Tetel t in rendeles.tetelek)
            {
                StackPanel sp = StackPanelExpander.tetelElemKeszit(t);
                tetelekListBox.Items.Add(sp);
            }
        }

        private void comboBoxokFeltolt()
        {
            Termekek.listakDictionary['h'].ForEach(x => hamburgerComboBox.Items.Add(x));
            Termekek.listakDictionary['i'].ForEach(x => italComboBox.Items.Add(x));
            Termekek.listakDictionary['d'].ForEach(x => desszertComboBox.Items.Add(x));
            Termekek.listakDictionary['k'].ForEach(x => koretComboBox.Items.Add(x));
        }


        private void dbErtekBeallitas(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int textSzamkent = !string.IsNullOrEmpty(tb.Text) ? int.Parse(tb.Text) : 0;
            if (tb.Name.StartsWith('h'))
                hdb = textSzamkent;
            else if (tb.Name.StartsWith('k'))
                kdb = textSzamkent;
            else if (tb.Name.StartsWith('d'))
                ddb = textSzamkent;
            else
                idb = textSzamkent;
        }

        private void csakSzamok(object sender, TextCompositionEventArgs e)
        {
            Regex szamokPattern = new Regex("[^0-9]+");
            e.Handled = szamokPattern.IsMatch(e.Text);
        }

        private void tetelekListBoxValasztas(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem is not null)
            {
                tetelSzerkesztesTorles.IsEnabled = true;
                StackPanel sp = (StackPanel)lb.SelectedItem;
                tetel = rendeles.tetelek.First(x => x.tazon == (int)sp.Tag);

                hamburgerComboBox.SelectedItem = tetel.burger;
                hamburgerDB.Text = tetel.bdb.ToString();

                koretComboBox.SelectedItem = tetel.koret;
                koretDB.Text = tetel.kdb.ToString();

                italComboBox.SelectedItem = tetel.ital;
                italDB.Text = tetel.idb.ToString();

                desszertComboBox.SelectedItem = tetel.desszert;
                desszertDB.Text = tetel.ddb.ToString();

                megjegyzesTextBox.Text = tetel.megjegyzes;

            }
            else
                tetelSzerkesztesTorles.IsEnabled = false;

        }

        private void tetelTorlese(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Biztosan törölni akarod a {tetel.tazon} számú tételt?", "Tétel törlése", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                MySqlParameter tazonparam = new("@tazon", tetel.tazon);
                List<MySqlParameter> tetelTorleseParams = new() { tazonparam };
                List<string> eredmeny = MySQL.query("teteltorlese", true, tetelTorleseParams);
                MessageBox.Show(eredmeny[0]);
                rendeles.tetelekFrissit();
                tetelekListBoxFeltolt();
            }
        }

        private void tetelModositasa(object sender, RoutedEventArgs e)
        {
            Termek hamburger = (Termek)hamburgerComboBox.SelectedItem;
            Termek koret = (Termek)koretComboBox.SelectedItem;
            Termek ital = (Termek)italComboBox.SelectedItem;
            Termek desszert = (Termek)desszertComboBox.SelectedItem;
            string megjegyzes = megjegyzesTextBox.Text;

            MySqlParameter tazonparam = new("@tazon", tetel.tazon);
            MySqlParameter hazonparam = new("@hazon", hamburger.azon);
            MySqlParameter hdbparam = new("@hdb", hdb);
            MySqlParameter kazonparam = new("@kazon", koret.azon);
            MySqlParameter kdbparam = new("@kdb", kdb);
            MySqlParameter dazonparam = new("@dazon", desszert.azon);
            MySqlParameter ddbparam = new("@ddb", ddb);
            MySqlParameter iazonparam = new("@iazon", ital.azon);
            MySqlParameter idbparam = new("@idb", idb);
            MySqlParameter megjegyzesparam = new("@megjegyzes", megjegyzes);
            List<MySqlParameter> paramListTetel = new() { tazonparam, hazonparam, hdbparam, kazonparam, kdbparam, dazonparam, ddbparam, iazonparam, idbparam, megjegyzesparam };
            List<string> eredmeny = MySQL.query("tetelmodosit", true, paramListTetel);
            MessageBox.Show(eredmeny[0]);
            Close();
        }
    }
}
