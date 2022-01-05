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

        private void listBoxokFeltolt()
        {
            Termekek.listakDictionary['h'].ForEach(x => hamburgerekListBox.Items.Add(x));
            Termekek.listakDictionary['k'].ForEach(x => koretekListBox.Items.Add(x));
            Termekek.listakDictionary['d'].ForEach(x => desszertekListBox.Items.Add(x));
            Termekek.listakDictionary['i'].ForEach(x => italokListBox.Items.Add(x));
        }

        private void termekModositas(object sender, MouseButtonEventArgs e)
        {
            Window termekmodositasUI = new TermekModositas();
            termekmodositasUI.Show();
        }
    }
}
