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
        public Regisztracio(Felhasznalo f = null)
        {
            InitializeComponent();
            felhasznalo = f;
            if (felhasznalo is not null)
                adatokFeltolt();
        }
        
        private void adatokFeltolt()
        {
            teljesNevDoboz.Text = felhasznalo.nev;
            lakhelyDoboz.Text = felhasznalo.lakh;
            telszamDoboz.Text = felhasznalo.tel;
            emailDoboz.Text = felhasznalo.email;
            jogosultsag.SelectedIndex = felhasznalo.jog - 1;

            jelszoEloszor.IsEnabled = false;
            jelszoMasodszor.IsEnabled = false;
            if (felhasznalo.jog == 4)
                jogosultsag.IsEnabled = false;

            regisztralasGomb.Content = "Felhasználó frissítése";
        }

        /// <summary>
        /// Végrehajtja a regisztrálást a db-ben, lecsekkolja hogy a jelszó verifikáció jó-e.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regisztralas(object sender, RoutedEventArgs e)
        {
            MySqlParameter teljesnevparam = new("@teljesnev", teljesNevDoboz.Text);
            MySqlParameter lakhelyparam = new("@lakh", lakhelyDoboz.Text);
            MySqlParameter telparam = new("@tel", telszamDoboz.Text);
            MySqlParameter emailparam = new("@email", emailDoboz.Text);
            MySqlParameter jogparam = new("@jog", jogosultsag.SelectedIndex + 1);

            if (felhasznalo is null)
            {
                if (jelszoEloszor.Password != jelszoMasodszor.Password)
                    MessageBox.Show("A jelszavaknak egyeznie kell!");
                else
                {
                    string pw = MySQL.hashPW(jelszoEloszor.Password); //stringet MD5 technológiával hasheljük, csakis hash-t tárolunk.
                    //MySqlParameter , konstruktor első paramétere a cserélendő paraméter, a második az érték hogy mire.
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
            else
            {
                Admin.FelhasznalokModositasaUI owner = (Admin.FelhasznalokModositasaUI)Owner;
                MySqlParameter azonparam = new("@azon", felhasznalo.id);
                List<MySqlParameter> felhasznaloFrissitesParams = new() { teljesnevparam, lakhelyparam, telparam, emailparam, jogparam, azonparam};
                List<string> eredmeny = MySQL.query("felhasznalofrissit", true, felhasznaloFrissitesParams);
                MessageBox.Show(eredmeny[0].ToString());
                owner.listBoxokFeltolt();
                Close();
            }
        }

        private void regisztraciosDobozMindKitoltveE(object sender, RoutedEventArgs e)
        {
            if (felhasznalo is null)
            {
                if (teljesNevDoboz.Text.Length > 0 && jelszoEloszor.Password.Length > 0 && jelszoMasodszor.Password.Length > 0 && lakhelyDoboz.Text.Length > 0 && telszamDoboz.Text.Length > 0 && emailDoboz.Text.Length > 0)
                    regisztralasGomb.IsEnabled = true;
                else
                    regisztralasGomb.IsEnabled = false;
            }
            else
            {
                if (teljesNevDoboz.Text.Length > 0 && lakhelyDoboz.Text.Length > 0 && telszamDoboz.Text.Length > 0 && emailDoboz.Text.Length > 0)
                    regisztralasGomb.IsEnabled = true;
                else
                    regisztralasGomb.IsEnabled = false;
            }
        }

        private void csakSzamok(object sender, TextCompositionEventArgs e)
        {
            Regex szamokPattern = new Regex("[^0-9]+");
            e.Handled = szamokPattern.IsMatch(e.Text);
        }
    }
}
