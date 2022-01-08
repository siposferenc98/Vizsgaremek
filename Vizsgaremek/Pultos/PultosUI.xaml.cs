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
            Task.Run(() => listboxFrissitAsync()); //Task.Run egy async eljárást fog elindítani

        }

        //Async frissítések
        #region Async funkciok
        /// <summary>
        /// Async eljárás, nem befolyásolja a main szál futását, minden ami itt történik másik CPU thread-en történik.
        /// </summary>
        private async void listboxFrissitAsync()
        {
            //egy végtelen ciklusba 
            while (true)
            {
                rendelesekFrissitListBoxFeltolt(); //frissítünk a rendeléseken és újra feltöltjük a listboxokat
                await Task.Delay(5000); //await - szóval várjon meg egy 5000 milisecondos késleltetést, ez után a végtelen ciklus kezdődik elölről
            }

        }

        private void rendelesekFrissitListBoxFeltolt()
        {
            //Mivel a listboxainkat a main thread birtokolja, és ezt az eljárást egy async eljárás hívja meg, nem tudja csak simán módosítani
            Dispatcher.Invoke(() => // ezért az ablak Dispatcherjének kell odaadni a feladatot
            {
                Rendelesek.rendelesekFrissit(); //ráfrissítünk a rendelések listánkra
                tetelekListBoxFeltolt(); //és mostmár ő fel tudja tölteni a listboxainkat
            });

        }
        #endregion

        //Listboxok,comboboxok feltöltése
        #region List es comboboxok feltoltese
        /// <summary>
        /// Feltölti az italt tartalmazó tételekkel a listboxot.
        /// </summary>
        private void tetelekListBoxFeltolt()
        {
            //mivel nem töröljük a listboxunkat, mindig csak az újakat akarjuk hozzá adogatni, és az idő közben törölt tételeket meg törölni a listboxból, lekérjük a listbox itemjeit, .OfType<StackPanel>()-el átalakítjuk egy stackpanel listává
            List<StackPanel> listboxitemek = tetelekListBox.Items.OfType<StackPanel>().ToList();
            //minden alkalommal végigmegyünk ezen a listán
            foreach (StackPanel sp in listboxitemek)
            {
                Expander expander = (Expander)sp.Children[1]; //a stackpanel [0] eleme a labelünk, az [1] az expander
                if (expander.Content is null) //ha az expander contentje null, akkor idő közben a tételt törölték
                    tetelekListBox.Items.Remove(sp); //szóval mi is töröljük a listboxunkból
            }

            //végig megyünk a rendelések listán ahol az italstatus folyamatban van, időrendi sorrendbe rakjuk
            foreach (Rendeles r in Rendelesek.rendelesekLista
                .Where(x => x.italstatus == 1)
                .OrderBy(x => x.ido))
            {
                //minden rendelésnek a tételein végigmegyünk ahol szintén folyamatban van az italstatus
                foreach (Tetel t in r.tetelek.Where(x => x.italstatus == 1)) 
                {
                    //megnézzük hogy ha nincs még benne az elem aminek a tagje(tazon van bele mentve készítéskor) megegyezik a mostanival
                    if (!listboxitemek.Any(x => (int)x.Tag == t.tazon))
                    {
                        StackPanelExpander stackPanelElem = new(); //létrehozunk minden tételnek egy példányt
                        StackPanel sp = stackPanelElem.tetelElemKeszit(r,t); //elkészíttetjük az expanderes stackpanelt ami lenyitáskor frissíti magát
                        tetelekListBox.Items.Add(sp);
                    }    
                }
            }
        }
        #endregion

        //OnSelectionChanged eventekre
        #region Eventek
        private void tetelekListBoxValasztas(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender; //a listboxunk lesz a sender
            if (lb.SelectedItem is not null) //ha van valami kiválasztva a tétel kész gombot bekapcsoljuk
                tetelKeszGomb.IsEnabled = true;
            else
                tetelKeszGomb.IsEnabled = false;
        }
        #endregion

        //MySQL állapotváltoztatás
        #region MySQL eljarasok
        /// <summary>
        /// A kiválasztott tétel státuszát késszé változtatja.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tetelKesz(object sender, RoutedEventArgs e)
        {
            StackPanel tetel = (StackPanel)tetelekListBox.SelectedItem; //a kiválasztott item egy stackpanel lesz a listboxunkból

            //lementjük a messagebox eredményét
            MessageBoxResult result = MessageBox.Show($"Biztosan készen van a {tetel.Tag}. számú tétel?", "Készen van?", MessageBoxButton.OKCancel);
            //ha biztosan készen van a tétel
            if (result == MessageBoxResult.OK)
            {
                //létrehozzuk a tétel azonosító paramétert a stackpanelünk tagjéből
                MySqlParameter tazon = new("@tazon", tetel.Tag);
                List<MySqlParameter> keszTetelParams = new() { tazon }; //létrehozzuk belőle a paraméter listát
                List<string> eredmeny = MySQL.query("italkesz", true, keszTetelParams); //a nonquery eredményt lementjük
                if (eredmeny.Any())
                {
                    tetelekListBox.Items.Remove(tetel); //kiszedjük a listboxból a kész tételt
                    MessageBox.Show(eredmeny[0]);
                }
            }

        }
        #endregion

        //Kijelentkezés
        #region Egyeb eljarasok
        private void kijelentkezes(object sender, RoutedEventArgs e)
        {
            //Kijelentkezés felső menü
            AktualisFelhasznalo.felhasznalo = null; //az aktuális felhasználónkat null-ra rakjuk
            Window bejelentkezes = new Bejelentkezes(); //létrehozunk egy új bejelentkezés ablakot
            bejelentkezes.Show();
            Close();
        }
        #endregion
    }
}
