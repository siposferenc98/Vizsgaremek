using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Vizsgaremek.Osztalyok
{
    class StackPanelExpander
    {
        private Rendeles rendeles;
        private Tetel tetel;
        public StackPanel tetelElemKeszit(Rendeles r , Tetel t)
        {
            rendeles = r;
            tetel = t;


            StackPanel sp = new();
            sp.Orientation = Orientation.Horizontal;
            sp.Tag = t.tazon;
            Label cim = new();
            cim.Content = $"{t.tazon} számú tétel.";
            Expander expander = new();
            expander.Expanded += expanderLenyitva;
            Label rendelesreszletek = new();
            rendelesreszletek.Content = t;
            expander.Content = rendelesreszletek;
            sp.Children.Add(cim);
            sp.Children.Add(expander);
            return sp;
        }

        private void expanderLenyitva(object sender, RoutedEventArgs e)
        {
            Rendelesek.rendelesekFrissit();
            Expander exp = (Expander)sender;
            tetel = Rendelesek.rendelesekLista.First(x => x.razon == rendeles.razon).tetelek.First(x => x.tazon == tetel.tazon);
            exp.Content = tetel;
            
        }
    }
}
