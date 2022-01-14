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
    /// Interaction logic for JelszoValtoztat.xaml
    /// </summary>
    public partial class JelszoValtoztat : Window
    {
        public JelszoValtoztat()
        {
            InitializeComponent();
        }
        //MySQL eljárások
        #region Mysql eljarasok
        /// <summary>
        /// Jelszóváltoztatásért felel,megnézi hogy jó-e az adott jelszó,és az ellenőrzés.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jelszoValtoztat(object sender, RoutedEventArgs e)
        {
            string akt = MySQL.hashPW(aktualisJelszo.Password); //aktuális jelszó
            MySqlParameter pwparam = new("pw", akt);
            MySqlParameter azonparam = new("azon", AktualisFelhasznalo.felhasznalo.id);
            List<MySqlParameter> pwCsekkParams = new() {azonparam, pwparam };
            List<string> eredmeny = MySQL.query("aktualisjelszo", false, pwCsekkParams); //megnézzük van e ilyen ID-vel ilyen jelszó
            if (eredmeny.Any()) //ha van
            {
                if (jelszoEloszor.Password == jelszoMasodszor.Password) //akkor megnézzük az új jelszavak egyeznek-e
                {
                    string ujpw = MySQL.hashPW(jelszoEloszor.Password); //ha igen akkor lehasheljük az új jelszót
                    MySqlParameter ujpwparam = new("pw", ujpw);
                    List<MySqlParameter> jelszoValtoztatParams = new() { azonparam, ujpwparam };
                    List<string> valtoztatEredmeny = MySQL.query("jelszovaltoztat", true, jelszoValtoztatParams); //és megváltoztatjuk az adott ID-hez
                    MessageBox.Show(valtoztatEredmeny[0]);
                    Close();
                }
                else
                {
                    MessageBox.Show("Az új jelszó mezők nem egyeznek meg.");
                }
            }
            else
            {
                MessageBox.Show("Téves aktuális jelszó.");
            }
            #endregion


        }
    }
}
