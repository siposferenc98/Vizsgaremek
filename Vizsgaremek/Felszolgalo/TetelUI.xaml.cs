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

namespace Vizsgaremek.Felszolgalo
{
    /// <summary>
    /// Interaction logic for TetelUI.xaml
    /// </summary>
    public partial class TetelUI : Window
    {
        private readonly int rendelesID;
        private int hazon, kazon, dazon, iazon;
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
            leiras.Text = termek.leiras;

            if (cb.Name.StartsWith('h'))
                hazon = termek.azon;
            else if (cb.Name.StartsWith('k'))
                kazon = termek.azon;
            else if (cb.Name.StartsWith('d'))
                dazon = termek.azon;
            else
                iazon = termek.azon;

        }

        private void tetelHozzaad(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"RendelésID : {rendelesID} \n Burger: {hazon} db: {hdb} \n Köret: {kazon}, db: {kdb} \n Desszert: {dazon}, db: {ddb} \n Ital: {iazon}, db: {idb}");
        }
    }
}
