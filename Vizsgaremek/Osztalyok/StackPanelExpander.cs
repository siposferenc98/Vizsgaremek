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
        public static StackPanel tetelElemKeszit(Tetel t)
        {
            StackPanel sp = new();
            sp.Orientation = Orientation.Horizontal;
            sp.Tag = t.tazon;
            Label cim = new();
            cim.Content = $"{t.tazon} számú tétel.";
            Expander expander = new();
            Label rendelesreszletek = new();
            rendelesreszletek.Content = t;
            expander.Content = rendelesreszletek;
            sp.Children.Add(cim);
            sp.Children.Add(expander);
            return sp;
        }
    }
}
