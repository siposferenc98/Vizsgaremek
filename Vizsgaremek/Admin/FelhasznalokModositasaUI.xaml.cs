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
    /// Interaction logic for FelhasznalokModositasaUI.xaml
    /// </summary>
    public partial class FelhasznalokModositasaUI : Window
    {
        public FelhasznalokModositasaUI()
        {
            InitializeComponent();
            listBoxokFeltolt();
        }

        public void listBoxokFeltolt()
        {
            alkalmazottakListBox.Items.Clear();
            vendegekListBox.Items.Clear();

            Felhasznalok.felhasznalokFrissit();
            foreach (Felhasznalo f in Felhasznalok.felhasznaloLista)
            {
                if (f.jog > 0)
                    alkalmazottakListBox.Items.Add(f);
                else
                    vendegekListBox.Items.Add(f);
            }
        }

        private void alkalmazottakModositasa(object sender, MouseButtonEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            Window alkalmazottModositasaUI = new Regisztracio((Felhasznalo)lb.SelectedItem);
            alkalmazottModositasaUI.Owner = this;
            alkalmazottModositasaUI.Show();
        }
    }
}
