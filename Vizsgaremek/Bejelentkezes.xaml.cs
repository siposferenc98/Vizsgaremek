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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vizsgaremek.Osztalyok;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Vizsgaremek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Bejelentkezes : Window
    {
        public Bejelentkezes()
        {
            InitializeComponent();
            MySQL.dictionaryFeltolt(); //minden indításnál feltöltjük a dictionarynkat az sql.txtben található értékekkel.
        }

        //TextChanged eventek
        #region Eventek
        /// <summary>
        /// Ez az eljárás minden alkalommal meghívásra kerül amikor a szövegdobozokban az érték változik.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void szovegValtozott(object sender, RoutedEventArgs e)
        {
            // csak akkor legyen kattintható a gomb ha mindkét mezőbe van szöveg.
            if (felhasznaloBX.Text.Length > 0 && jelszoBX.Password.Length > 0) 
                loginBTN.IsEnabled = true;
            else 
                loginBTN.IsEnabled = false;
        }
        private void adatbazisValasztoComboBox(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            switch (cb.SelectedIndex)
            {
                case 0:
                    MySQL.conn = new(ConfigurationManager.ConnectionStrings["localhost"].ConnectionString);
                    break;
            }
        }
        #endregion

        //Új ablakok megnyitása
        #region Uj ablakok
        /// <summary>
        /// Ez az eljárás indítja el a bejelentkezést.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bejelentkezes(object sender, RoutedEventArgs e)
        {
            inditasiEljarasok();
            string felhasznalo = felhasznaloBX.Text;
            string pw = MySQL.hashPW(jelszoBX.Password); //stringet MD5 technológiával hasheljük, csakis hash-t tárolunk.

            //Létrehozzuk a paramétereket, konstruktor első paraméter a cserélendő paraméter, a második az érték
            MySqlParameter fparam = new("@felh", felhasznalo);
            MySqlParameter pwparam = new("@pw", pw);

            List<MySqlParameter> param = new() {fparam, pwparam}; //hozzáadjuk egy paraméter típusú listához
            List<string> eredmenyek = MySQL.query("bejelentkezes", false, param); //A listánk üres lesz ha nincs ilyen felhasználó, ha lesz benne 1 érték akkor vagy sikerült a bejelentkezés, vagy hibát fog tartalmazni.

            if (eredmenyek.Count > 1)
            {
                // id, nev , lakh, tel, email, jog
                AktualisFelhasznalo.felhasznalo = new Felhasznalo(int.Parse(eredmenyek[0]), eredmenyek[1], eredmenyek[2], eredmenyek[3], eredmenyek[4], int.Parse(eredmenyek[5]));
                MessageBox.Show($"Üdvözlünk {AktualisFelhasznalo.felhasznalo.nev} !");

                //switchelünk egyet az aktuális bejelentkeztetett felhasználó jogán és megjelenítjük az UI-ját
                //1-Felszolgáló, 2-Szakács, 3-Pultos, 4-Admin
                switch (AktualisFelhasznalo.felhasznalo.jog)
                {
                    case 1:
                        Window felszolgaloUI = new Felszolgalo.FelszolgaloUI();
                        felszolgaloUI.Show();
                        Close();
                        break;
                    case 2:
                        Window szakacsUI = new Szakacs.SzakacsUI();
                        szakacsUI.Show();
                        Close();
                        break;
                    case 3:
                        Window pultosUI = new Pultos.PultosUI();
                        pultosUI.Show();
                        Close();
                        break;
                    case 4:
                        Window adminUI = new Admin.AdminUI();
                        adminUI.Show();
                        Close();
                        break;
                    default:
                        MessageBox.Show("Még nincs kész");
                        break;
                }
            }
            else if (eredmenyek.Count is 1) //ha a count 1 akkor valószínüleg hiba lesz benne.
            {
                MessageBox.Show(eredmenyek[0]);
            }
            else //ha semmi akkor meg nem jók a bejelentkezési adatok
            {
                MessageBox.Show("Nincs ilyen felhasználónév vagy hibás jelszó!");
            }
        }

        private void regisztracio(object sender, RoutedEventArgs e)
        {
            Window reg = new Regisztracio();
            reg.Show();
        }

        #endregion

        //Indítási eljárások
        #region Egyeb eljarasok
        private void inditasiEljarasok()
        { 

            try
            {
                //A termékek osztályba van kommentelve
                Termekek.mindenListaFrissit();
            }
            catch (Exception e)
            {
                MessageBoxResult messageboxEredmeny = MessageBox.Show($"MySQL csatlakozási / formátum hiba \n ({e.Message}) \n Újra próbálja?","Hiba",MessageBoxButton.YesNo);

                if(messageboxEredmeny == MessageBoxResult.Yes) //ha igenre nyom, újra meghívjuk az indítási eljárásokat
                {
                    inditasiEljarasok();
                }

            }
        }

        #endregion
        



    }
}
