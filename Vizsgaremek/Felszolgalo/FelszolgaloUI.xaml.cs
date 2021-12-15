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
        //TODO ÖSSZES RENDELÉST 1BE GYŰJTENI,VÉGIGMENNI ÉS ELDÖNTENI MELYIK LISTBOXBA KERÜLJÖN, LEKÉRDEZÉSBE NEM KELL WHERE FELTÉTEL.
        private List<Rendeles> keszRendelesek = new();
        public FelszolgaloUI()
        {
            InitializeComponent();
            Task.Run(() => keszListboxFrissitAsync());
            
        }

        private void keszListboxFrissit()
        {
            Dispatcher.Invoke(() =>
            {
                keszRendelesek = new();
                List<int> eredmeny = MySQL.query("pincerkeszfrissit", false).Select(int.Parse).ToList();
                if (eredmeny.Any())
                    for (int i = 0; i < eredmeny.Count; i += 5)
                    {
                        Rendeles rendeles = new(eredmeny[i], eredmeny[i + 1], eredmeny[i + 2], eredmeny[i + 3], eredmeny[i + 4]);
                        keszRendelesek.Add(rendeles);
                    }
                keszListboxFeltolt();
                
            });
            
        }

        private void keszListboxFeltolt()
        {
            zartListBox.Items.Clear();
            if (keszRendelesek.Any())
                foreach (Rendeles r in keszRendelesek)
                    zartListBox.Items.Add(r);
        }

        private async void keszListboxFrissitAsync()
        {

            while (true)
            {
                keszListboxFrissit();
                await Task.Delay(5000);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Rendeles item in zartListBox.Items)
            {
                MessageBox.Show($"{item.razon}");
            }
        }
    }
}
