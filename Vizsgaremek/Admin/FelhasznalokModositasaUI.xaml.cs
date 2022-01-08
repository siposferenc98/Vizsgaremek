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

namespace Vizsgaremek.Admin
{
    /// <summary>
    /// Interaction logic for FelhasznalokModositasaUI.xaml
    /// </summary>
    public partial class FelhasznalokModositasaUI : Window
    {
        public FelhasznalokModositasaUI()
        {
            InitializeComponent();
            listBoxokFeltolt();
        }
        //Listbox feltöltése
        #region Listboxok feltoltese
        public void listBoxokFeltolt()
        {
            //töröljük a listboxokat
            alkalmazottakListBox.Items.Clear();
            vendegekListBox.Items.Clear();

            Felhasznalok.felhasznalokFrissit(); //ráfrissítünk a felhasználókra
            foreach (Felhasznalo f in Felhasznalok.felhasznaloLista) //végigmegyünk a listán
            {
                if (f.jog > 0) //ha nagyobb a joga mint 0 akkor ott dolgozó
                    alkalmazottakListBox.Items.Add(f);
                else //különben vendég
                    vendegekListBox.Items.Add(f);
            }
        }
        #endregion

        //Dupla klikk event
        #region Eventek
        private void alkalmazottakModositasa(object sender, MouseButtonEventArgs e)
        {
            ListBox lb = (ListBox)sender; //a senderünk a listbox
            Window alkalmazottModositasaUI = new Regisztracio((Felhasznalo)lb.SelectedItem); //létrehozunk egy új regisztráció ablakot és paraméterbe átadjuk a kiválasztott felhasználót, így a regisztráció ablakunk módosításként fog szolgálni mert kap paramétert(kommentek ott)
            alkalmazottModositasaUI.Owner = this;
            alkalmazottModositasaUI.Show();
        }
        #endregion
    }
}
