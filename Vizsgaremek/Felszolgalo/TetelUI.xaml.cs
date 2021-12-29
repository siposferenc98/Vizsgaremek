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
            Hamburgerek.hamburgerLista.ForEach(x => hamburgerComboBox.Items.Add(x));
        }

    }
}
