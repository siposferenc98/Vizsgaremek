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

namespace Vizsgaremek.Szakacs
{
    /// <summary>
    /// Interaction logic for SzakacsUI.xaml
    /// </summary>
    public partial class SzakacsUI : Window
    {
        public SzakacsUI()
        {
            InitializeComponent();
            Task.Run(() => listboxFrissitAsync());
        }

        private async void listboxFrissitAsync()
        {

            while (true)
            {
                rendelesekFrissitListBoxFeltolt();
                await Task.Delay(5000);
            }

        }

        private void rendelesekFrissitListBoxFeltolt()
        {
            Dispatcher.Invoke(() =>
            {
                Rendelesek.rendelesekFrissit();
                tetelekListBoxFeltolt();
            });

        }

        private void tetelekListBoxFeltolt()
        {
            foreach (Rendeles r in Rendelesek.rendelesekLista
                .Where(x => x.etelstatus == 1)
                .OrderByDescending(x => x.ido))
            {
                foreach (Tetel t in r.tetelek)
                {
                    StackPanel sp = StackPanelExpander.tetelElemKeszit(t);
                    if(!tetelekListBox.Items.OfType<StackPanel>().Any(x => (int)x.Tag == (int)sp.Tag))
                        tetelekListBox.Items.Add(sp);
                }
            }
        }

        private void kijelentkezes(object sender, RoutedEventArgs e)
        {
            AktualisFelhasznalo.felhasznalo = null;
            Window bejelentkezes = new Bejelentkezes();
            bejelentkezes.Show();
            Close();
        }
    }
}
