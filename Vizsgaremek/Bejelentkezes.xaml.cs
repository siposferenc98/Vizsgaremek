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

namespace Vizsgaremek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Bejelentkezes : Window
    {
        private List<Felhasznalo> felh = new List<Felhasznalo>
        {
            new Felhasznalo("Feri",1,4),
            new Felhasznalo("Jani",2,3),
            new Felhasznalo("Márk",3,2),
            new Felhasznalo("Dani",4,1),
        };

        public Bejelentkezes()
        {
            InitializeComponent();
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
            string pw = MySQL.hashPW(jelszoBX.Password);
            string queryNotInDictionary = $"SELECT * FROM login WHERE acc = '{felhasznalo}' AND pw = '{pw}'";
            List<string> eredmenyek = MySQL.query(queryNotInDictionary, false, false);
            if (eredmenyek.Count > 0)
            {
                MessageBox.Show(eredmenyek[0]);
            }
            else
                MessageBox.Show("Nincs ilyen felhasználónév vagy hibás jelszó!");
        }

        private void regisztracio(object sender, RoutedEventArgs e)
        {
            Window reg = new Regisztracio();
            reg.Show();
        }
    }
}
