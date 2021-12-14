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

namespace Vizsgaremek
{
    /// <summary>
    /// Interaction logic for Regisztracio.xaml
    /// </summary>
    public partial class Regisztracio : Window
    {
        public Regisztracio()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Végrehajtja a regisztrálást a db-ben, lecsekkolja hogy a jelszó verifikáció jó-e.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void regisztralas(object sender, RoutedEventArgs e)
        {
            if (jelszoEloszor.Password != jelszoMasodszor.Password)
                MessageBox.Show("A jelszavaknak egyeznie kell!");
            else
            {
                string felh = felhNevDoboz.Text;
                string pw = MySQL.hashPW(jelszoEloszor.Password); //stringet MD5 technológiával hasheljük, csakis hash-t tárolunk.
                int jog = jogosultsag.SelectedIndex + 1;
                //MySqlParameter , konstruktor első paramétere a cserélendő paraméter, a második az érték hogy mire.
                MySqlParameter fparam = new("@felh", felh);
                MySqlParameter pwparam = new("@pw", pw);
                MySqlParameter jogparam = new("@jog", jog);
                List<MySqlParameter> paramListLetezikE = new() { fparam }; //létrehozunk egy listát paraméterekkel
                List<string> letezikE = MySQL.query("regisztracioletezik", false, paramListLetezikE); //először megnézzük egy selecttel hogy létezik-e ilyen felhasználó, ha nincs benne semmi akkor nem,szóval lehet regisztrálni.

                if (letezikE.Count == 0)
                {
                    List<MySqlParameter> paramListRegisztracio = new() { fparam, pwparam, jogparam }; //új paraméterlista
                    List<string> eredmeny = MySQL.query("regisztracio", true, paramListRegisztracio);
                    MessageBox.Show(eredmeny[0].ToString()); //kiiratjuk az eredmény listánk elemét, vagy sikeres lesz, vagy hibát fog tartalmazni.
                    Close();
                }
                else
                    MessageBox.Show("Ez a felhasználó már létezik!");
            }
        }

        private void regisztraciosDobozMindKitoltveE(object sender, RoutedEventArgs e)
        {
            if (felhNevDoboz.Text.Length > 0 && jelszoEloszor.Password.Length > 0 && jelszoMasodszor.Password.Length > 0)
                regisztralasGomb.IsEnabled = true;
            else
                regisztralasGomb.IsEnabled = false;
        }
    }
}
