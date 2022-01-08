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
using System.Text.RegularExpressions;
using Vizsgaremek.Osztalyok;
using MySql.Data.MySqlClient;

namespace Vizsgaremek.Felszolgalo
{
    /// <summary>
    /// Interaction logic for TetelUI.xaml
    /// </summary>
    public partial class TetelUI : Window
    {
        private readonly int rendelesID;
        private int hdb, kdb, ddb, idb;

        public TetelUI(int rendelesID)
        {
            InitializeComponent();
            this.rendelesID = rendelesID;

            //minden db textboxot feliratjuk a csak számok regexre, komment a classban
            hamburgerDB.PreviewTextInput += RegexClass.csakSzamok;
            koretDB.PreviewTextInput += RegexClass.csakSzamok;
            desszertDB.PreviewTextInput += RegexClass.csakSzamok;
            italDB.PreviewTextInput += RegexClass.csakSzamok;

            comboBoxokFeltolt();
        }


        //Listboxok,comboboxok feltöltése
        #region List es comboboxok feltoltese
        /// <summary>
        /// Feltöltjük a comboboxokat a választható termékekkel.
        /// </summary>
        private void comboBoxokFeltolt()
        {
            Termekek.listakDictionary['h'].Where(x => x.aktiv).ToList().ForEach(x => hamburgerComboBox.Items.Add(x));
            Termekek.listakDictionary['i'].Where(x => x.aktiv).ToList().ForEach(x => italComboBox.Items.Add(x));
            Termekek.listakDictionary['d'].Where(x => x.aktiv).ToList().ForEach(x => desszertComboBox.Items.Add(x));
            Termekek.listakDictionary['k'].Where(x => x.aktiv).ToList().ForEach(x => koretComboBox.Items.Add(x));
            
        }
        #endregion

        //TextChanged, OnSelectionChanged eventekre
        #region Eventek
        /// <summary>
        /// Beállítja a darabszámokat a textboxokból az ablakunk mezőibe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dbErtekBeallitas(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender; //egyik darab textbox lesz
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
        /// Változtatja a kiválasztott termék leírását a mellette egy sorban lévő textblockban.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leirasValtoztat(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender; //az egyik combobox (hamburger,desszert,ital,köret)
            TextBlock leiras = (TextBlock)FindName(cb.Name + "leiras"); //findname-el megkeressük a combobox neve + 'leiras' nevű textblockot pl.hamburgerComboBox a cb neve, 'hamburgerComboBoxleiras' nevű textblockot keres.
            Termek termek = (Termek)cb.SelectedItem; //a kiválasztott termék
            leiras.Text = $"{termek.leiras}, {termek.ar} Ft."; //a megtalált textblock textje a leirás és az ár lesz
        }
        #endregion

        //MySQL tétel hozzáadása
        #region MySQL eljarasok
        /// <summary>
        /// Hozzá ad egy tételt az ablak megnyitásakor kapott rendelésID-s rendeléshez.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tetelHozzaad(object sender, RoutedEventArgs e)
        {
            //lementjük a kiválasztott termékeket,megjegyzést
            Termek hamburger = (Termek)hamburgerComboBox.SelectedItem;
            Termek koret = (Termek)koretComboBox.SelectedItem;
            Termek ital = (Termek)italComboBox.SelectedItem;
            Termek desszert = (Termek)desszertComboBox.SelectedItem;
            string megjegyzes = megjegyzesTextBox.Text;

            //lementjük a messagebox eredményét
            MessageBoxResult okcancel = MessageBox.Show(
                $"RendelésID : {rendelesID} \n " +
                $"Burger: {hamburger.nev} db: {hdb} \n " +
                $"Köret: {koret.nev}, db: {kdb} \n " +
                $"Desszert: {desszert.nev}, db: {ddb} \n " +
                $"Ital: {ital.nev}, db: {idb} \n " +
                $"Megjegyzés: {megjegyzes} \n " +
                $"Összesen: {hamburger.ar * hdb + koret.ar * kdb + desszert.ar * ddb + ital.ar * idb}",
                "Tétel megerősítése", 
                MessageBoxButton.OKCancel);

            //ha biztosan jó a tétel
            if(okcancel == MessageBoxResult.OK)
            {
                //@razon, @hazon, @hdb, @kazon, @kdb, @dazon, @ddb, @iazon, @idb, 1, 1, @megjegyzes
                //Létrehozzuk a paramétereket, konstruktor első paraméter a cserélendő paraméter, a második az érték
                MySqlParameter razonparam = new("@razon", rendelesID);
                MySqlParameter hazonparam = new("@hazon", hamburger.azon);
                MySqlParameter hdbparam = new("@hdb", hdb);
                MySqlParameter kazonparam = new("@kazon", koret.azon);
                MySqlParameter kdbparam = new("@kdb", kdb);
                MySqlParameter dazonparam = new("@dazon", desszert.azon);
                MySqlParameter ddbparam = new("@ddb", ddb);
                MySqlParameter iazonparam = new("@iazon", ital.azon);
                MySqlParameter idbparam = new("@idb", idb);
                MySqlParameter megjegyzesparam = new("@megjegyzes", megjegyzes);

                List<MySqlParameter> paramListTetel = new() { razonparam, hazonparam, hdbparam, kazonparam, kdbparam, dazonparam, ddbparam, iazonparam, idbparam, megjegyzesparam }; //létrehozzuk belőlük a paraméter listát is

                List<string> eredmeny = MySQL.query("tetelbeszur", true, paramListTetel); //lementjük a nonquery eredményét ami sikeres vagy hiba
                MessageBox.Show(eredmeny[0]);
                Close();
            }
        }
            #endregion
    }
}
