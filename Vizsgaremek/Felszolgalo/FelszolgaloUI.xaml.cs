using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for FelszolgaloUI.xaml
    /// </summary>
    public partial class FelszolgaloUI : Window
    {
        
        public FelszolgaloUI()
        {
            InitializeComponent();
            asztalokComboBoxFeltolt();
            Task.Run(() => listboxokFrissitAsync());
            
        }

        private async void listboxokFrissitAsync()
        {

            while (true)
            {
                keszListboxFrissit();
                await Task.Delay(10000);
            }
            
        }

        private void keszListboxFrissit()
        {
            Dispatcher.Invoke(() =>
            {
                Rendelesek.rendelesekFrissit();
                listboxokFeltolt();
            });
            
        }

        private void listboxokFeltolt()
        {
            nyitottListBox.Items.Clear();
            zartListBox.Items.Clear();
            

            if (Rendelesek.rendelesekLista.Any())
                foreach (Rendeles r in Rendelesek.rendelesekLista)
                {
                    if (r.etelstatus > 1 || r.italstatus> 1)
                        zartListBox.Items.Add(r);
                    else
                        nyitottListBox.Items.Add(r);

                    if(!rendelesekComboBox.Items.Contains(r.razon) && r.etelstatus < 3 && r.italstatus < 3)
                        rendelesekComboBox.Items.Add(r.razon);
                }
        }

        private void asztalokComboBoxFeltolt()
        {
            asztalokComboBox.Items.Clear();
            Rendelesek.rendelesekFrissit();
            for (int i = 0; i < 10; i++)
            {
                if (Rendelesek.rendelesekLista.Any(x => x.asztal == i && x.etelstatus < 3))
                    continue;
                asztalokComboBox.Items.Add(i);
            }
        }

        private void tetelUiMegnyit(object sender, RoutedEventArgs e)
        {
            Window tetelUI = new TetelUI((int)rendelesekComboBox.SelectedItem);
            tetelUI.Owner = this;
            tetelUI.Show();
        }
        private void rendelesekComboBoxValasztas(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.SelectedItem is not null)
                tetelUIGomb.IsEnabled = true;
            else
                tetelUIGomb.IsEnabled = false;
        }

        private void rendelesReszletek(object sender, RoutedEventArgs e)
        {
            Rendeles rendeles = (Rendeles)nyitottListBox.SelectedItem;
            Window reszletek = new RendelesReszletekUI(rendeles);
            reszletek.Owner = this;
            reszletek.Show();
        }

        private void nyitottListBoxValasztas(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem is not null)
                felvettRendelesekVezerlo.IsEnabled = true;
            else
                felvettRendelesekVezerlo.IsEnabled = false;
        }
    }
}
