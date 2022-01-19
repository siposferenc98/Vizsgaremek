using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Vizsgaremek.Felszolgalo
{
    /// <summary>
    /// Interaction logic for RendelesReszletekUI.xaml
    /// </summary>
    public partial class RendelesReszletekUI : Window
    {
        private readonly Rendeles rendeles;
        private Tetel tetel;
        private int hdb, kdb, ddb, idb;
        public RendelesReszletekUI(Rendeles rendeles)
        {
            InitializeComponent();
            this.rendeles = rendeles;
            this.rendeles.tetelekFrissit();

            //minden db textboxot feliratjuk a csak számok regexre, komment a classban
            hamburgerDB.PreviewTextInput += RegexClass.csakSzamok;
            koretDB.PreviewTextInput += RegexClass.csakSzamok;
            desszertDB.PreviewTextInput += RegexClass.csakSzamok;
            italDB.PreviewTextInput += RegexClass.csakSzamok;
            tetelekListBoxFeltolt();
            comboBoxokFeltolt();
        }

        //Listboxok,comboboxok feltöltése
        #region List es comboboxok feltoltese
        /// <summary>
        /// Feltölti a tételek listboxot a rendelés tételeivel,lenyitható stackpanelekkel.
        /// </summary>
        private void tetelekListBoxFeltolt()
        {
            tetelekListBox.Items.Clear();

            foreach (Tetel t in rendeles.tetelek)
            {
                StackPanelExpander stackPanelElem = new(); //létrehozunk minden tételnek egy példányt
                StackPanel sp = stackPanelElem.tetelElemKeszit(rendeles,t); //elkészíttetjük az expanderes stackpanelt ami lenyitáskor frissíti magát
                tetelekListBox.Items.Add(sp);
            }
        }

        /// <summary>
        /// Feltöltjük a comboboxokat a választható termékekkel.
        /// </summary>
        private void comboBoxokFeltolt()
        {
            Termekek.listakDictionary['h'].Where(x=>x.aktiv).ToList().ForEach(x => hamburgerComboBox.Items.Add(x));
            Termekek.listakDictionary['i'].Where(x => x.aktiv).ToList().ForEach(x => italComboBox.Items.Add(x));
            Termekek.listakDictionary['d'].Where(x => x.aktiv).ToList().ForEach(x => desszertComboBox.Items.Add(x));
            Termekek.listakDictionary['k'].Where(x => x.aktiv).ToList().ForEach(x => koretComboBox.Items.Add(x));
        }
        #endregion

        //OnSelectionChanged eventekre
        #region Eventek
        /// <summary>
        /// Beállítja a darabszámokat a textboxokból az ablakunk mezőibe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dbErtekBeallitas(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;//egyik darab textbox lesz
            int textSzamkent = !string.IsNullOrEmpty(tb.Text) ? int.Parse(tb.Text) : 0; //ha NEM null or empty a textbox értéke akkor átcastoljuk int-é , különben 0-t állítunk be.

            if (tb.Name.StartsWith('h')) //hamburgerDB
                hdb = textSzamkent;
            else if (tb.Name.StartsWith('k')) //koretDB
                kdb = textSzamkent;
            else if (tb.Name.StartsWith('d')) //desszertDB
                ddb = textSzamkent;
            else //italDB
                idb = textSzamkent;
        }

        /// <summary>
        /// Tétel kiválasztásánál kiválasztja a comboboxokból a tétel termékeit,leírását,darabszámait.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tetelekListBoxValasztas(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem is not null) //ha van kiválasztott tétel
            {
                tetelSzerkesztesTorles.IsEnabled = true; //használhatóvá tesszük a vezérlőket
                StackPanel sp = (StackPanel)lb.SelectedItem; //stackpanel tagjának be van állítva a tétel azonosítója
                tetel = rendeles.tetelek.First(x => x.tazon == (int)sp.Tag); //ez alapján be tudjuk állítani az adott tételt a classunk mezőjébe

                //és ezek után feltölteni mindent az adataival
                hamburgerComboBox.SelectedItem = tetel.Burger;
                hamburgerDB.Text = tetel.Bdb.ToString();

                koretComboBox.SelectedItem = tetel.Koret;
                koretDB.Text = tetel.Kdb.ToString();

                italComboBox.SelectedItem = tetel.Ital;
                italDB.Text = tetel.Idb.ToString();

                desszertComboBox.SelectedItem = tetel.Desszert;
                desszertDB.Text = tetel.Ddb.ToString();

                megjegyzesTextBox.Text = tetel.megjegyzes;

            }
            else //különben kikapcsoljuk a vezérlőket
            {
                tetelSzerkesztesTorles.IsEnabled = false;
            }

        }
        #endregion

        //MySQL törlés, módosítás
        #region MySQL eljarasok
        /// <summary>
        /// Törli a kiválasztott tételt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tetelTorlese(object sender, RoutedEventArgs e)
        {
            //lementjük a messagebox eredményét
            MessageBoxResult result = MessageBox.Show($"Biztosan törölni akarod a {tetel.tazon} számú tételt?", "Tétel törlése", MessageBoxButton.OKCancel);

            //ha biztosan törölni akarja
            if (result == MessageBoxResult.OK)
            {
                //elkészítjük a tétel azonosító paramétert a (kiválasztott) beállított tételből
                MySqlParameter tazonparam = new("@tazon", tetel.tazon);
                List<MySqlParameter> tetelTorleseParams = new() { tazonparam }; //meg belőle a paraméter listát
                List<string> eredmeny = MySQL.query("teteltorlese", true, tetelTorleseParams);
                MessageBox.Show(eredmeny[0]);
                rendeles.tetelekFrissit(); //rendelésünknél ráfrissítünk a tételeire mert törlés történt
                tetelekListBoxFeltolt(); //és újra feltöltjük a tételek listboxunkat
            }
        }

        /// <summary>
        /// Módosítja a kiválasztott tételt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tetelModositasa(object sender, RoutedEventArgs e)
        {
            //a beállított termékeket,megjegyzést lementjük
            Termek hamburger = (Termek)hamburgerComboBox.SelectedItem;
            Termek koret = (Termek)koretComboBox.SelectedItem;
            Termek ital = (Termek)italComboBox.SelectedItem;
            Termek desszert = (Termek)desszertComboBox.SelectedItem;
            string megjegyzes = megjegyzesTextBox.Text;

            //Létrehozzuk a paramétereket, konstruktor első paraméter a cserélendő paraméter, a második az érték
            MySqlParameter tazonparam = new("@tazon", tetel.tazon);
            MySqlParameter hazonparam = new("@hazon", hamburger.azon);
            MySqlParameter hdbparam = new("@hdb", hdb);
            MySqlParameter kazonparam = new("@kazon", koret.azon);
            MySqlParameter kdbparam = new("@kdb", kdb);
            MySqlParameter dazonparam = new("@dazon", desszert.azon);
            MySqlParameter ddbparam = new("@ddb", ddb);
            MySqlParameter iazonparam = new("@iazon", ital.azon);
            MySqlParameter idbparam = new("@idb", idb);
            MySqlParameter megjegyzesparam = new("@megjegyzes", megjegyzes);

            List<MySqlParameter> paramListTetel = new() { tazonparam, hazonparam, hdbparam, kazonparam, kdbparam, dazonparam, ddbparam, iazonparam, idbparam, megjegyzesparam }; //belőlük a paraméter listát is

            List<string> eredmeny = MySQL.query("tetelmodosit", true, paramListTetel);//lementjük a nonquery eredményét, szóval vagy sikeres vagy hiba
            MessageBox.Show(eredmeny[0]);
            Close();
        }
        #endregion
    }
}
