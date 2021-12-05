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

        private void regisztralas(object sender, RoutedEventArgs e)
        {
            if (jelszoEloszor.Password != jelszoMasodszor.Password)
                MessageBox.Show("A jelszavaknak egyeznie kell!");
            else
            {
                string felh = felhNevDoboz.Text;
                string pw = MySQL.hashPW(jelszoEloszor.Password);
                int jog = jogosultsag.SelectedIndex + 1;
                string nonQuery = $"INSERT INTO login VALUES('{felh}', '{pw}', {jog})";
                List<string> eredmeny = MySQL.query(nonQuery, true, false);
                MessageBox.Show(eredmeny[0].ToString());
                Close();
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
