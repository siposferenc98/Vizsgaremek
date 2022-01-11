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
    /// Interaction logic for FoglalasokUI.xaml
    /// </summary>
    public partial class FoglalasokUI : Window
    {
        private List<Foglalas> foglalasok = Foglalasok.foglalasLista;
        public FoglalasokUI()
        {
            InitializeComponent();
        }

        //OnTextChanged, OnSelectionChanged eventekre
        #region Eventek
        /// <summary>
        /// Név szerint keresés textbox text változtatásánál fut le.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nevSzerintKereses(object sender, TextChangedEventArgs e)
        {
            foglalasokListBox.Items.Clear();
            TextBox tb = (TextBox)sender;
            //megkeressük a foglalás listánkból azt ami tartalmazza a beírt szöveget, stringcomparisonnal ignoráljuk a kis és nagy betűket
            foglalasok.Where(x => x.felhasznalo.nev.Contains(tb.Text, StringComparison.InvariantCultureIgnoreCase)).ToList().ForEach(x => foglalasokListBox.Items.Add(x));
        }

        /// <summary>
        /// Időtartam combobox változásánál fut le. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        // az ablak megnyitásánál automatikusan le fog futni mert XAML-ben van 
        private void idoTartamValtozott(object sender, SelectionChangedEventArgs e)
        {
            foglalasokListBox.Items.Clear();
            Felhasznalok.felhasznalokFrissit(); //ráfrissítünk a felhasználókra
            ComboBox cb = (ComboBox)sender;
            foglalasok = cb.SelectedIndex switch //a foglalás listánk legyen egyenlő -> switchelünk egyet a selectedindexen
            {
                0 => Foglalasok.foglalasLista, //ha 0 - "Összes" akkor a teljes foglalás lista
                1 => Foglalasok.foglalasLista.Where(x => x.idopontDatum.Date == DateTime.Today.Date).ToList(), //1 - "Ma"
                2 => Foglalasok.foglalasLista.Where(x => x.idopontDatum.Month == DateTime.Today.Month).ToList(), //2 - "Ez a hónap"
                _ => throw new NotImplementedException()
            };
            nevSzerintKereses(nevSzerintKeresesTextBox, null); //a már változtatott listára hívjuk meg a névszerint keresést,ha üres a név akkor a teljes filterelt listánkkal töltjük fel, ha van bele írva valami akkor már ebből szűri ki.
        }
        #endregion

        //Új ablakok megnyitás
        #region Uj ablakok
        /// <summary>
        /// Megnyit egy ablakot amiben a kiválaszott foglalás rendelései(és benne tételei) lesznek.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void foglalasRendeleseiMegnyit(object sender, MouseButtonEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            Window foglalasRendelesekUI = new RendelesElozmenyekUI((Foglalas)lb.SelectedItem); //ha paraméterbe adunk neki egy foglalást akkor a konstruktorjában egy olyan funkció fog lefutni ami a foglalás rendeléseivel tölti fel.
            foglalasRendelesekUI.Owner = this;
            foglalasRendelesekUI.Show();
        }
        #endregion
    }
}
