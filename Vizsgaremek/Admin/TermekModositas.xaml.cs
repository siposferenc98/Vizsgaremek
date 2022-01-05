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
        public TermekModositas(Termek termek, string sqlLekerdezes)
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
            termekNev.Text = termek.nev;
            termekAr.Text = termek.ar.ToString();
            termekLeiras.Text = termek.leiras;
        }

        private void termekFrissitese(object sender, RoutedEventArgs e)
        {
            MySqlParameter termekazonparam = new("@termekazon", termek.azon);
            MySqlParameter termeknevparam = new("@termeknev", termekNev.Text);
            MySqlParameter termekarparam = new("@termekar", termekAr.Text);
            MySqlParameter termekleirasparam = new("@termekleiras", termekLeiras.Text);
            List<MySqlParameter> termekFrissitParams = new() { termekazonparam, termeknevparam, termekarparam, termekleirasparam };
            List<string> eredmeny = MySQL.query(sqlLekerdezes, true, termekFrissitParams);
            if(eredmeny.Any())
            {
                TermekekUI owner = (TermekekUI)Owner;
                Termekek.mindenListaFrissit();
                owner.listBoxokFeltolt();
                MessageBox.Show(eredmeny[0]);
                Close();
            }
        }
    }
}
