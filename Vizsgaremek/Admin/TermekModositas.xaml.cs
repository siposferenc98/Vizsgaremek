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

namespace Vizsgaremek.Admin
{
    /// <summary>
    /// Interaction logic for TermekModositas.xaml
    /// </summary>
    public partial class TermekModositas : Window
    {
        private readonly Termek termek;
        private readonly string sqlLekerdezes;
        private TermekFajtak termekfajta;
        private enum TermekFajtak
        {
            Hamburger,
            Koret,
            Desszert,
            Ital
        }

        public TermekModositas(Termek termek = null, string sqlLekerdezes = null)
        {
            InitializeComponent();
            this.termek = termek;
            this.sqlLekerdezes = sqlLekerdezes;

            termekAr.PreviewTextInput += RegexClass.csakSzamok; //ár textboxot feliratunk a csak számok regexre, komment a classban
            //ha kapunk paraméterbe valamit pl 'hamburgerfrissit'-et,akkor feltöltjük adatokkal az UI-t,különben hozzáadni szeretnénk szóval teljesen üres mezőket kell kapjunk
            if (sqlLekerdezes is not null) 
                adatokFeltoltese();
        }

        //Listbox feltölt
        #region Listboxok feltoltese
        /// <summary>
        /// Feltölti adatokkal a comboboxokat,textboxokat,checkboxokat.
        /// </summary>
        private void adatokFeltoltese()
        {
            if (sqlLekerdezes.StartsWith('h'))
                termekTipus.SelectedIndex = 0;
            if (sqlLekerdezes.StartsWith('k'))
                termekTipus.SelectedIndex = 1;
            if (sqlLekerdezes.StartsWith('d'))
                termekTipus.SelectedIndex = 2;
            if (sqlLekerdezes.StartsWith('i'))
                termekTipus.SelectedIndex = 3;

            termekTipus.IsEnabled = false;
            termekNev.Text = termek.nev;
            termekAr.Text = termek.ar.ToString();
            termekLeiras.Text = termek.leiras;
            aktivCheckBox.IsChecked = termek.aktiv;
        }
        #endregion

        //SelectionChanged event
        #region Eventek
        /// <summary>
        /// Beállítja a termékfajtát selectedindex alapján, egy enumból.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void termekFajtaBeallit(object sender, SelectionChangedEventArgs e)
        {
            termekfajta = (TermekFajtak)termekTipus.SelectedIndex;
        }
        #endregion

        //Mysql termék hozzáad,változtat
        #region MySQL eljarasok
        /// <summary>
        /// Frissít,vagy hozzáad egy új terméket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void termekFrissitese(object sender, RoutedEventArgs e)
        {
            //Létrehozzuk a paramétereket, konstruktor első paraméter a cserélendő paraméter, a második az érték
            MySqlParameter termeknevparam = new("@termeknev", termekNev.Text);
            MySqlParameter termekarparam = new("@termekar", termekAr.Text);
            MySqlParameter termekleirasparam = new("@termekleiras", termekLeiras.Text);
            MySqlParameter aktivparam = new("@aktiv", aktivCheckBox.IsChecked);

            //ha van termékünk(sqlLekerdezesunk is) akkor frissíteni szeretnénk az adott terméket
            if (termek is not null)
            {
                //így ki tudjuk szedni a termék azonosítóját a kapott termékből
                MySqlParameter termekazonparam = new("@termekazon", termek.azon);
                //létrehozzuk a paraméterlistát a többi adattal és az azonosítóval
                List<MySqlParameter> termekFrissitParams = new() { termekazonparam, termeknevparam, termekarparam, termekleirasparam, aktivparam };
                //itt adjuk tovább a kapott pl 'hamburgerfrissit'-et, hogy melyik táblába van az adott termék
                List<string> eredmeny = MySQL.query(sqlLekerdezes, true, termekFrissitParams);
                
                MessageBox.Show(eredmeny[0]);
                
            }
            else //különben újat szeretnénk hozzáadni
            {
                //switch expression
                //a string értéke = valami amin switchelünk
                string sql = termekfajta switch
                {
                    TermekFajtak.Hamburger => "hamburgerhozzaad", //ha a beállított termekfajta-nk enumból hamburger akkor az sqlünk 'hamburgerhozzaad'
                    TermekFajtak.Koret => "korethozzaad",
                    TermekFajtak.Desszert => "desszerthozzaad",
                    TermekFajtak.Ital => "italhozzaad",
                    _ => throw new NotImplementedException()
                };

                List<MySqlParameter> termekHozzaadParams = new() {termeknevparam, termekarparam, termekleirasparam, aktivparam };
                //itt megkapja a query végül az sql stringet ami a switch után lett belőle
                List<string> eredmeny = MySQL.query(sql , true, termekHozzaadParams);

                MessageBox.Show(eredmeny[0]);
            }

            TermekekUI owner = (TermekekUI)Owner;
            Termekek.mindenListaFrissit();
            owner.listBoxokFeltolt(); //miután frissítettünk vagy hozzáadtunk egy terméket,ráfrissítünk az owner listboxára
            Close();
        }
        #endregion

    }
}
