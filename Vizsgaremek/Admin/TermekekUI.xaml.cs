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
    /// Interaction logic for TermekekUI.xaml
    /// </summary>
    public partial class TermekekUI : Window
    {
        public TermekekUI()
        {
            InitializeComponent();
            listBoxokFeltolt();
        }
        //Listbox feltölt
        #region Listboxok feltoltese
        public void listBoxokFeltolt()
        {
            //töröljük az összes listbox itemjeit
            hamburgerekListBox.Items.Clear();
            koretekListBox.Items.Clear();
            desszertekListBox.Items.Clear();
            italokListBox.Items.Clear();

            //végigmegyünk a 4 listán és hozzáadjuk ahova kell
            Termekek.listakDictionary['h'].ForEach(x => hamburgerekListBox.Items.Add(x));
            Termekek.listakDictionary['k'].ForEach(x => koretekListBox.Items.Add(x));
            Termekek.listakDictionary['d'].ForEach(x => desszertekListBox.Items.Add(x));
            Termekek.listakDictionary['i'].ForEach(x => italokListBox.Items.Add(x));
        }
        #endregion

        //Termék módosítás , hozzáadás ablak
        #region Uj ablakok
        private void termekModositas(object sender, MouseButtonEventArgs e)
        {
            ListBox lb = (ListBox)sender; //a listboxunk a 4 listbox közül az egyik lesz
            Termek termek = (Termek)lb.SelectedItem; //ez meg a belőle kiválasztott item
            Window termekmodositasUI = new TermekModositas(termek, lb.Tag.ToString()); //létrehozunk egy új termékmódosításUI-t,ahol megadjuk paraméterbe a terméket és a listboxunk tagjét, ami majd az sql query dictionary kulcsunk lesz, pl hamburger listbox hívta ezt meg, a tagje 'hamburgerfrissit', ez az amit tovább adunk paraméterbe
            termekmodositasUI.Owner = this;
            termekmodositasUI.Show();
        }

        private void termekHozzaad(object sender, RoutedEventArgs e)
        {
            Window termekhozzaadUI = new TermekModositas(); //ugyanúgy a termék módosítás UI-t nyitjuk meg ha újat akarunk hozzáadni,de nem kap semmit paraméterbe, többi komment abba az ablakba
            termekhozzaadUI.Owner = this;
            termekhozzaadUI.Show();
        }
        #endregion
    }
}
