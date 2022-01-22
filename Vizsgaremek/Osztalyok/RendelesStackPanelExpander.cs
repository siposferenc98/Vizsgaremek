using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace Vizsgaremek.Osztalyok
{
    class RendelesStackPanelExpander
    {
        private static Dictionary<string, string> bindingNevErtek = new()
        {
            //{ "tazon", "Tétel azonosító"},
            { "Burger", "Hamburger"},
            { "Bdb", "DB"},
            { "Desszert", "Desszert"},
            { "Ddb", "DB"},
            { "Koret", "Köret"},
            { "Kdb", "DB" },
            { "Ital", "Ital" },
            { "Idb", "DB"},
            { "vegosszeg", "Végösszeg"}
        };
        
        /// <summary>
        /// Egy lenyitható stackpanel elemet készít a rendelésből, aminek a tartalma egy DataGrid a tételekkel.
        /// </summary>
        /// <param name="r">Egy rendelés</param>
        /// <returns>Stackpanel elem, benne a tételeivel</returns>
        public static StackPanel rendelesElemKeszit(Rendeles r)
        {
            ObservableCollection<Tetel> ts = new(r.tetelek); //létrehozunk egy ObservableCollectiont a tételek listából,ezt fogjuk használni a gridünk tartalmához.

            StackPanel sp = new();
            sp.Orientation = Orientation.Horizontal; //első stackpanelünk vízszintes 

            Expander ex = new(); //expander nem tartalmazhat több elemet, csak .Content-je lehet, szóval a contentje a datagridünk lesz
            ex.Header = $"{r.razon} számú rendelés, {r.ido}"; 
            DataGrid dataGrid = new(); //létrehozunk egy datagridet
            dataGrid.Margin = new Thickness(10);
            dataGrid.AutoGenerateColumns = false; //ne generálja ki az összes oszlopot nekünk előlre névvel mert majd mi hozzáadjuk amik kellenek
            dataGrid.IsReadOnly = true; //ne tudják a gridet szerkeszteni
            dataGrid.ItemsSource = ts; //ami az új observablecollection
            
            foreach (KeyValuePair<string, string> keyValuePair in bindingNevErtek) //végigmegyünk párossával a dictionaryn
            {
                //minden elemhez készítünk egy oszlopot
                DataGridTextColumn dataGridColumn = new();
                dataGridColumn.Header = keyValuePair.Value; //aminek legfelül a headerje a dict. értéke lesz, amit látni szeretnék a UI-on
                dataGridColumn.Binding = new Binding(keyValuePair.Key); //a binding a dict. key lesz,ez létrehoz egy databindingot
                //mivel a datagridünk itemssource-a az observablecollection ami tételeket tartalmaz, a binding útvonala a tétel classon belül fog keresni ilyen nevű propertyt, amihez hozzá tudja kötni az értéket
                dataGrid.Columns.Add(dataGridColumn); //végül hozzáadjuk a datagridünkhöz ezt az oszlopot
            }
            

            ex.Content = dataGrid; //az expanderünk contentje a datagridünk lesz ami a collectionből tétel adataival lesz feltöltve

            sp.Children.Add(ex);
            return sp;

        }
    }
}
