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
        public TetelUI()
        {
            InitializeComponent();
            comboBoxokFeltolt();
        }

        private void csakSzamok(object sender, TextCompositionEventArgs e)
        {
            Regex szamokPattern = new Regex("[^0-9]+");
            e.Handled = szamokPattern.IsMatch(e.Text);
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
        }

        private void tetelHozzaad(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
