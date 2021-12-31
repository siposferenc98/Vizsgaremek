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

namespace Vizsgaremek.Felszolgalo
{
    /// <summary>
    /// Interaction logic for RendelesReszletekUI.xaml
    /// </summary>
    public partial class RendelesReszletekUI : Window
    {
        private readonly Rendeles rendeles;
        public RendelesReszletekUI(Rendeles rendeles)
        {
            InitializeComponent();
            this.rendeles = rendeles;
            this.rendeles.tetelekFrissit();
            tetelekListBoxFeltolt();
        }

        private void tetelekListBoxFeltolt()
        {
            foreach (Tetel t in rendeles.tetelek)
            {
                StackPanel sp = new();
                sp.Orientation = Orientation.Horizontal;
                Label cim = new();
                cim.Content = $"{t.tazon} számú tétel.";
                Expander expander = new();
                Label rendelesreszletek = new();
                rendelesreszletek.Content = 
                    $"{t.burger.nev}, {t.bdb} db, ár: {t.bdb*t.burger.ar} Ft. \n" +
                    $"{t.koret.nev}, {t.kdb} db, ár: {t.kdb*t.koret.ar} Ft. \n" +
                    $"{t.desszert.nev}, {t.kdb} db, ár: {t.ddb*t.desszert.ar} Ft. \n" +
                    $"{t.ital.nev}, {t.kdb} db, ár: {t.idb*t.ital.ar} Ft. \n" +
                    $"Összesen: {t.vegosszeg} Ft.";
                expander.Content = rendelesreszletek;
                sp.Children.Add(cim);
                sp.Children.Add(expander);
                tetelekListBox.Items.Add(sp);
            }
        }
    }
}
