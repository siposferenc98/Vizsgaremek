using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Vizsgaremek.Osztalyok
{
    class RendelesStackPanelExpander
    {
        public static StackPanel rendelesElemKeszit(Rendeles r)
        {
            StackPanel sp = new();
            sp.Orientation = Orientation.Horizontal;
            Label cim = new();
            cim.Content = $"{r.razon} számú rendelés, {r.ido}";
            Expander ex = new();
            StackPanel expanderStackPanel = new();
            foreach (Tetel t in r.tetelek)
            {
                StackPanelExpander tetelExpander = new();
                expanderStackPanel.Children.Add(tetelExpander.tetelElemKeszit(r, t));
            }
            ex.Content = expanderStackPanel;

            sp.Children.Add(cim);
            sp.Children.Add(ex);
            return sp;

        }
    }
}
