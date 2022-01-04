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
using MySql.Data.MySqlClient;

namespace Vizsgaremek.Pultos
{
    /// <summary>
    /// Interaction logic for PultosUI.xaml
    /// </summary>
    public partial class PultosUI : Window
    {
        public PultosUI()
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
                .Where(x => x.italstatus == 1)
                .OrderByDescending(x => x.ido))
            {
                foreach (Tetel t in r.tetelek.Where(x => x.italstatus == 1))
                {
                    StackPanel sp = StackPanelExpander.tetelElemKeszit(t);
                    if (!tetelekListBox.Items.OfType<StackPanel>().Any(x => (int)x.Tag == (int)sp.Tag))
                        tetelekListBox.Items.Add(sp);
                }
            }
        }

        private void tetelKesz(object sender, RoutedEventArgs e)
        {
            StackPanel tetel = (StackPanel)tetelekListBox.SelectedItem;
            MessageBoxResult result = MessageBox.Show($"Biztosan készen van a {tetel.Tag}. számú tétel?", "Készen van?", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                MySqlParameter tazon = new("@tazon", tetel.Tag);
                List<MySqlParameter> keszTetelParams = new() { tazon };
                List<string> eredmeny = MySQL.query("italkesz", true, keszTetelParams);
                if (eredmeny.Any())
                {
                    tetelekListBox.Items.Remove(tetel);
                    MessageBox.Show(eredmeny[0]);
                }
            }

        }

        private void tetelekListBoxValasztas(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem is not null)
                tetelKeszGomb.IsEnabled = true;
            else
                tetelKeszGomb.IsEnabled = false;
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
