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
    /// Interaction logic for RendelesElozmenyekUI.xaml
    /// </summary>
    public partial class RendelesElozmenyekUI : Window
    {
        public RendelesElozmenyekUI()
        {
            InitializeComponent();
            listBoxFeltolt();
        }
        //Listbox feltöltése
        #region Listboxok feltoltese
        private void listBoxFeltolt()
        {
            //végigmegyünk a rendeléseklistán ahol 4-4esek a statusok,szóval már fizetett(régi) rendelések
            foreach (Rendeles r in Rendelesek.rendelesekLista.Where(x=>x.etelstatus == 4 && x.italstatus == 4))
            {
                //és minden rendelésből készíttetünk egy rendeléses stackpanelt expanderrel, aminek a contentje a többi expanderes tétel lesz
                rendelesElozmenyekListBox.Items.Add(RendelesStackPanelExpander.rendelesElemKeszit(r));
            }

        }
        #endregion
    }
}
