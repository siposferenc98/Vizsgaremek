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
            //amiket kapunk lementjük mert másik funkcióba szükségünk lesz rá
            rendeles = r;
            tetel = t;


            StackPanel sp = new();//legkülső stackpanel
            sp.Orientation = Orientation.Horizontal; //vízszintesen, először a cím lesz benne, mellé jobbra az expanderünk
            sp.Tag = t.tazon; //stackpanel tagjének beállítjuk a tétel azonosítóját,frissítésnél szükség lesz rá
            Label cim = new();
            cim.Content = $"{t.tazon} számú tétel.";

            Expander expander = new();
            expander.Expanded += expanderLenyitva; //az expander lenyitás eventjére hozzáadjuk a funkciót

            Label rendelesreszletek = new();
            rendelesreszletek.Content = t; // a content a tételünk lesz, mivel csak magát a tételt adtuk meg automatikusan a .ToString lesz rá meghívva

            expander.Content = rendelesreszletek;
            sp.Children.Add(cim); //hozzáadjuk a legkülső stackpanelhez a címet meg az expandert
            sp.Children.Add(expander);
            return sp;
        }

        /// <summary>
        /// Lefrissíti a tételt az expander lenyitásánál
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void expanderLenyitva(object sender, RoutedEventArgs e)
        {
            //Amint lenyitunk egy panelt, ráfrissít az összes rendelésre,kikeresi megint a(akár frissített) rendelést,köztük a(akár frissített) tételt,és az expander contentjét lecseréli a már frissített tételre.
            //ha idő közben törölték a tételt akkor defaultként az expander contentje null lesz,pultos meg szakacsUI-ban a null expander contentes stackpaneleket töröljük a listákból
           Rendelesek.rendelesekFrissit();
            Expander exp = (Expander)sender;
            if(tetel is not null)
            {
                tetel = Rendelesek.rendelesekLista.FirstOrDefault(x => x.razon == rendeles.razon).tetelek.FirstOrDefault(x => x.tazon == tetel.tazon);
                exp.Content = tetel;
            }
        }
    }
}
