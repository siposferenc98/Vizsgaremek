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
    /// Interaction logic for FoglalasokUI.xaml
    /// </summary>
    public partial class FoglalasokUI : Window
    {
        public FoglalasokUI()
        {
            InitializeComponent();
            listBoxFeltolt();
        }

        private void listBoxFeltolt()
        {
            Felhasznalok.felhasznalokFrissit();
            Foglalasok.foglalasLista.ForEach(x => foglalasokListBox.Items.Add(x));
        }
    }
}
