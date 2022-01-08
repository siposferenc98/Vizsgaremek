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
        /// <summary>
        /// Egy lenyitható stackpanel elemet készít a rendelésből, aminek a tartalma a szintén lenyitható stackpanelek a tételeiből.
        /// </summary>
        /// <param name="r">Egy rendelés</param>
        /// <returns>Stackpanel elem, benne a tételeivel</returns>
        public static StackPanel rendelesElemKeszit(Rendeles r)
        {
            StackPanel sp = new();
            sp.Orientation = Orientation.Horizontal; //első stackpanelünk vízszintes 

            Label cim = new();
            cim.Content = $"{r.razon} számú rendelés, {r.ido}"; //jön a cím először, és mellé jobbra majd az expander

            Expander ex = new(); //expander nem tartalmazhat több elemet, csak .Content-je lehet, szóval a contentje egy újabb stackpanel lesz

            StackPanel expanderStackPanel = new(); //amibe bele tudjuk rakosgatni a tételekhez készült többi stackpanelt függőlegesen
            foreach (Tetel t in r.tetelek) //végigmegyünk a tételein
            {
                StackPanelExpander tetelExpander = new(); //példányosítjuk a tételekből készítgető classt
                expanderStackPanel.Children.Add(tetelExpander.tetelElemKeszit(r, t)); //minden tételnél beküldjük a rendelést és a tételt, és amit visszakapunk azt hozzáadjuk
            }
            ex.Content = expanderStackPanel; //ha végigmentünk minden tételen az expander contentje az a stackpanelünk lesz amibe van a sok tételes stackpanel.

            sp.Children.Add(cim); //végül hozzáadhatjuk a fő stackpanelünkbe a címet és mellé jobbra az expandert
            sp.Children.Add(ex);
            return sp;

        }
    }
}
