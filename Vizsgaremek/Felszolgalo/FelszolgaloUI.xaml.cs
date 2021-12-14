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
            Task.Run(() => keszListboxFrissitAsync());
            
        }

        private void keszListboxFrissit()
        {
            Dispatcher.Invoke(() =>
            {
                zartListBox.Items.Clear();
                List<string> keszRendelesek = MySQL.query("pincerkeszfrissit", false);
                if (keszRendelesek.Any())
                    for (int i = 0; i < keszRendelesek.Count; i += 3)
                    {
                        string rendeles = $"{keszRendelesek[i]} {keszRendelesek[i + 1]} {keszRendelesek[i + 2]}";
                        zartListBox.Items.Add(rendeles);
                    }
                
            });
            
        }

        private async void keszListboxFrissitAsync()
        {

            while (true)
            {
                keszListboxFrissit();
                await Task.Delay(5000);
            }
            
        }

    }
}
