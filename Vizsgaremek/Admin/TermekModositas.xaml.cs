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
            adatokFeltoltese();
        }

        private void csakSzamok(object sender, TextCompositionEventArgs e)
        {
            Regex szamokPattern = new Regex("[^0-9]+");
            e.Handled = szamokPattern.IsMatch(e.Text);
        }

        private void adatokFeltoltese()
        {
            if(sqlLekerdezes is not null)
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
        }

        private void termekFrissitese(object sender, RoutedEventArgs e)
        {
            MySqlParameter termeknevparam = new("@termeknev", termekNev.Text);
            MySqlParameter termekarparam = new("@termekar", termekAr.Text);
            MySqlParameter termekleirasparam = new("@termekleiras", termekLeiras.Text);
            MySqlParameter aktivparam = new("@aktiv", aktivCheckBox.IsChecked);
            if (termek is not null)
            {
                MySqlParameter termekazonparam = new("@termekazon", termek.azon);
                List<MySqlParameter> termekFrissitParams = new() { termekazonparam, termeknevparam, termekarparam, termekleirasparam, aktivparam };
                List<string> eredmeny = MySQL.query(sqlLekerdezes, true, termekFrissitParams);
                
                MessageBox.Show(eredmeny[0]);
                
            }
            else
            {
                string sql = termekfajta switch
                {
                    TermekFajtak.Hamburger => "hamburgerhozzaad",
                    TermekFajtak.Koret => "korethozzaad",
                    TermekFajtak.Desszert => "desszerthozzaad",
                    TermekFajtak.Ital => "italhozzaad",
                    _ => throw new NotImplementedException()
                };

                List<MySqlParameter> termekHozzaadParams = new() {termeknevparam, termekarparam, termekleirasparam, aktivparam };
                List<string> eredmeny = MySQL.query(sql , true, termekHozzaadParams);

                MessageBox.Show(eredmeny[0]);
            }

            TermekekUI owner = (TermekekUI)Owner;
            Termekek.mindenListaFrissit();
            owner.listBoxokFeltolt();
            Close();
        }

        private void termekFajtaBeallit(object sender, SelectionChangedEventArgs e)
        {
            termekfajta = (TermekFajtak)termekTipus.SelectedIndex;
        }
    }
}
