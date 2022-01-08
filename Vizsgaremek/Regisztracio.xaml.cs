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
using System.Text.RegularExpressions;

namespace Vizsgaremek
{
    /// <summary>
    /// Interaction logic for Regisztracio.xaml
    /// </summary>
    public partial class Regisztracio : Window
    {
        private Felhasznalo felhasznalo;
        public Regisztracio(Felhasznalo f = null) //opcionális paraméter, ha nem kap felhasználót akkor regisztrálni akarunk
        {
            InitializeComponent();
            felhasznalo = f;
            telszamDoboz.PreviewTextInput += RegexClass.csakSzamok; //event feliratkozás csak szám bevitelre, komment a classban
            if (felhasznalo is not null) // ha viszont kaptunk egy felhasználót akkor módosítani szeretnénk egy meglévőt
                adatokFeltolt(); //feltöltjük adatokkal a textboxokat
        }

        //TextBoxok feltöltése ha módosítunk felhasználót
        #region TextBoxok feltoltese
        private void adatokFeltolt()
        {
            teljesNevDoboz.Text = felhasznalo.nev;
            lakhelyDoboz.Text = felhasznalo.lakh;
            telszamDoboz.Text = felhasznalo.tel;
            emailDoboz.Text = felhasznalo.email;
            jogosultsag.SelectedIndex = felhasznalo.jog - 1;

            //kikapcsoljuk a jelszó mezőt hogy ne lehessen másokét modosítani
            jelszoEloszor.IsEnabled = false;
            jelszoMasodszor.IsEnabled = false;
            if (felhasznalo.jog == 4)
                jogosultsag.IsEnabled = false; //és a jogosultságot se tudja az egyik admin levenni másikéról

            regisztralasGomb.Content = "Felhasználó frissítése";
        }
        #endregion

        //TextChanged eventekre
        #region Eventek
        /// <summary>
        /// Regisztrálás gombot engedélyezi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regisztraciosDobozMindKitoltveE(object sender, RoutedEventArgs e)
        {
            //ha regisztrálni akarunk akkor minden mezőt meg kell nézni hogy van-e benne szöveg
            if (felhasznalo is null)
            {
                if (teljesNevDoboz.Text.Length > 0 && jelszoEloszor.Password.Length > 0 && jelszoMasodszor.Password.Length > 0 && lakhelyDoboz.Text.Length > 0 && telszamDoboz.Text.Length > 0 && emailDoboz.Text.Length > 0)
                    regisztralasGomb.IsEnabled = true;
                else
                    regisztralasGomb.IsEnabled = false;
            }
            else //különben csak a jelszó dobozokat kihagyjuk
            {
                if (teljesNevDoboz.Text.Length > 0 && lakhelyDoboz.Text.Length > 0 && telszamDoboz.Text.Length > 0 && emailDoboz.Text.Length > 0)
                    regisztralasGomb.IsEnabled = true;
                else
                    regisztralasGomb.IsEnabled = false;
            }
        }
        #endregion

        //MySQL regisztráció, felhasználó módosítás
        #region MySQL eljarasok
        /// <summary>
        /// Végrehajtja a regisztrálást a db-ben, lecsekkolja hogy a jelszó verifikáció jó-e.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regisztralas(object sender, RoutedEventArgs e)
        {
            //Létrehozzuk a paramétereket, konstruktor első paraméter a cserélendő paraméter, a második az érték
            MySqlParameter teljesnevparam = new("@teljesnev", teljesNevDoboz.Text);
            MySqlParameter lakhelyparam = new("@lakh", lakhelyDoboz.Text);
            MySqlParameter telparam = new("@tel", telszamDoboz.Text);
            MySqlParameter emailparam = new("@email", emailDoboz.Text);
            MySqlParameter jogparam = new("@jog", jogosultsag.SelectedIndex + 1);

            //hogyha regisztrálni szeretnénk
            if (felhasznalo is null)
            {
                if (jelszoEloszor.Password != jelszoMasodszor.Password) //megnézzük egyezik-e a jelszó ellenőrzés
                    MessageBox.Show("A jelszavaknak egyeznie kell!");
                else
                {
                    string pw = MySQL.hashPW(jelszoEloszor.Password); //stringet MD5 technológiával hasheljük, csakis hash-t tárolunk.
                    MySqlParameter pwparam = new("@pw", pw);
                    List<MySqlParameter> paramListLetezikE = new() { emailparam }; //létrehozunk egy listát csak a felhasználónév paraméterrel
                    List<string> letezikE = MySQL.query("regisztracioletezik", false, paramListLetezikE); //először megnézzük egy selecttel hogy létezik-e ilyen felhasználó, ha nincs benne semmi akkor nem,szóval lehet regisztrálni.

                    if (letezikE.Count == 0)
                    {
                        List<MySqlParameter> regisztracioParams = new() { teljesnevparam, lakhelyparam, telparam, emailparam, jogparam, pwparam }; //új paraméterlista
                        List<string> eredmeny = MySQL.query("regisztracio", true, regisztracioParams);
                        MessageBox.Show(eredmeny[0].ToString()); //kiiratjuk az eredmény listánk elemét, vagy sikeres lesz, vagy hibát fog tartalmazni.
                        Close();
                    }
                    else
                        MessageBox.Show("Ez a felhasználó már létezik!");
                }
            }
            else //különben adminUI-ról módosítunk felhasználót
            {
                Admin.FelhasznalokModositasaUI owner = (Admin.FelhasznalokModositasaUI)Owner; //ennek az ablaknak az ownerje
                MySqlParameter azonparam = new("@azon", felhasznalo.id);
                List<MySqlParameter> felhasznaloFrissitesParams = new() { teljesnevparam, lakhelyparam, telparam, emailparam, jogparam, azonparam};
                List<string> eredmeny = MySQL.query("felhasznalofrissit", true, felhasznaloFrissitesParams);
                MessageBox.Show(eredmeny[0].ToString());
                owner.listBoxokFeltolt(); //ráfrissítünk az ownernél a listboxokra amint elvégeztük a módosítást
                Close();
            }
        }
        #endregion
    }
}
