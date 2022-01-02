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
            inditasiEljarasok();
        }

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

        /// <summary>
        /// Ez az eljárás indítja el a bejelentkezést.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bejelentkezes(object sender, RoutedEventArgs e)
        {
            string felhasznalo = felhasznaloBX.Text;
            string pw = MySQL.hashPW(jelszoBX.Password); //stringet MD5 technológiával hasheljük, csakis hash-t tárolunk.
            //Létrehozzuk a paramétereket
            MySqlParameter fparam = new("@felh", felhasznalo);
            MySqlParameter pwparam = new("@pw", pw);
            List<MySqlParameter> param = new() {fparam, pwparam}; //hozzáadjuk egy paraméter típusú listához
            List<string> eredmenyek = MySQL.query("bejelentkezes", false, param); //A listánk üres lesz ha nincs ilyen felhasználó, ha lesz benne 1 érték akkor vagy sikerült a bejelentkezés, vagy hibát fog tartalmazni.

            if (eredmenyek.Count > 1)
            {
                AktualisFelhasznalo.felhasznalo = new Felhasznalo(int.Parse(eredmenyek[0]), eredmenyek[1], eredmenyek[2], eredmenyek[3], eredmenyek[4], int.Parse(eredmenyek[5]));
                MessageBox.Show($"Üdvözlünk {AktualisFelhasznalo.felhasznalo.nev} !");
                //1-Felszolgáló, 2-Szakács, 3-Pultos, 4-Admin
                switch (AktualisFelhasznalo.felhasznalo.jog)
                {
                    case 1:
                        Window felszolgaloUI = new Felszolgalo.FelszolgaloUI();
                        felszolgaloUI.Show();
                        Close();
                        break;
                    default:
                        MessageBox.Show("Még nincs kész");
                        break;
                }
            }
            else if (eredmenyek.Count is 1) //ha a count 1 akkor valószínüleg hiba lesz benne.
                MessageBox.Show(eredmenyek[0]);
            else //ha semmi akkor meg nem jók az adatok
                MessageBox.Show("Nincs ilyen felhasználónév vagy hibás jelszó!");
        }


        private void inditasiEljarasok()
        {
            MySQL.dictionaryFeltolt(); //minden indításnál feltöltjük a dictionarynkat az sql.txtben található értékekkel.

            //A termékek osztályba van kommentelve
            Termekek.listaFrissit("hamburgerlekerdezes", 'h');
            Termekek.listaFrissit("koretlekerdezes", 'k');
            Termekek.listaFrissit("desszerteklekerdezes", 'd');
            Termekek.listaFrissit("itallekerdezes", 'i');
            
        }

        private void regisztracio(object sender, RoutedEventArgs e)
        {
            Window reg = new Regisztracio();
            reg.Show();
        }
    }
}
