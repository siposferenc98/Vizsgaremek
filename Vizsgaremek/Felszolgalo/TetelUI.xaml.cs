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
using System.Text.RegularExpressions;
using Vizsgaremek.Osztalyok;
using MySql.Data.MySqlClient;

namespace Vizsgaremek.Felszolgalo
{
    /// <summary>
    /// Interaction logic for TetelUI.xaml
    /// </summary>
    public partial class TetelUI : Window
    {
        private readonly int rendelesID;
        private int hdb, kdb, ddb, idb;

        public TetelUI(int rendelesID)
        {
            InitializeComponent();
            this.rendelesID = rendelesID;
            comboBoxokFeltolt();
        }

        private void csakSzamok(object sender, TextCompositionEventArgs e)
        {
            Regex szamokPattern = new Regex("[^0-9]+");
            e.Handled = szamokPattern.IsMatch(e.Text);
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

        private void comboBoxokFeltolt()
        {
            Termekek.listakDictionary['h'].ForEach(x => hamburgerComboBox.Items.Add(x));
            Termekek.listakDictionary['i'].ForEach(x => italComboBox.Items.Add(x));
            Termekek.listakDictionary['d'].ForEach(x => desszertComboBox.Items.Add(x));
            Termekek.listakDictionary['k'].ForEach(x => koretComboBox.Items.Add(x));
            
        }

        private void leirasValtoztat(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            TextBlock leiras = (TextBlock)FindName(cb.Name + "leiras");
            Termek termek = (Termek)cb.SelectedItem;
            leiras.Text = $"{termek.leiras}, {termek.ar} Ft.";
        }

        private void tetelHozzaad(object sender, RoutedEventArgs e)
        {
            Termek hamburger = (Termek)hamburgerComboBox.SelectedItem;
            Termek koret = (Termek)koretComboBox.SelectedItem;
            Termek ital = (Termek)italComboBox.SelectedItem;
            Termek desszert = (Termek)desszertComboBox.SelectedItem;
            string megjegyzes = megjegyzesTextBox.Text;
            MessageBoxResult okcancel = MessageBox.Show(
                $"RendelésID : {rendelesID} \n " +
                $"Burger: {hamburger.nev} db: {hdb} \n " +
                $"Köret: {koret.nev}, db: {kdb} \n " +
                $"Desszert: {desszert.nev}, db: {ddb} \n " +
                $"Ital: {ital.nev}, db: {idb} \n " +
                $"Megjegyzés: {megjegyzes} \n " +
                $"Összesen: {hamburger.ar * hdb + koret.ar * kdb + desszert.ar * ddb + ital.ar * idb}",
                "Tétel megerősítése", 
                MessageBoxButton.OKCancel);

            if(okcancel == MessageBoxResult.OK)
            {
                //@razon, @hazon, @hdb, @kazon, @kdb, @dazon, @ddb, @iazon, @idb, 1, 1, @megjegyzes
                MySqlParameter razonparam = new("@razon", rendelesID);
                MySqlParameter hazonparam = new("@hazon", hamburger.azon);
                MySqlParameter hdbparam = new("@hdb", hdb);
                MySqlParameter kazonparam = new("@kazon", koret.azon);
                MySqlParameter kdbparam = new("@kdb", kdb);
                MySqlParameter dazonparam = new("@dazon", desszert.azon);
                MySqlParameter ddbparam = new("@ddb", ddb);
                MySqlParameter iazonparam = new("@iazon", ital.azon);
                MySqlParameter idbparam = new("@idb", idb);
                MySqlParameter megjegyzesparam = new("@megjegyzes", megjegyzes);
                List<MySqlParameter> paramListTetel = new() { razonparam, hazonparam, hdbparam, kazonparam, kdbparam, dazonparam, ddbparam, iazonparam, idbparam, megjegyzesparam };
                MySQL.query("tetelbeszur", true, paramListTetel);
                Close();
            }

        }
    }
}
