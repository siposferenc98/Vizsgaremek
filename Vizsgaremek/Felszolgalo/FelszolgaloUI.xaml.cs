﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using MySql.Data.MySqlClient;

namespace Vizsgaremek.Felszolgalo
{
    /// <summary>
    /// Interaction logic for FelszolgaloUI.xaml
    /// </summary>
    public partial class FelszolgaloUI : Window
    {
        
        public FelszolgaloUI()
        {
            InitializeComponent();
            asztalokComboBoxFeltolt();
            foglalasComboBoxFeltolt();
            Task.Run(() => listboxokFrissitAsync());
            
        }

        //Async frissítések
        private async void listboxokFrissitAsync()
        {

            while (true)
            {
                keszListboxFrissit();
                await Task.Delay(10000);
            }
            
        }

        private void keszListboxFrissit()
        {
            Dispatcher.Invoke(() =>
            {
                Rendelesek.rendelesekFrissit();
                listboxokFeltolt();
            });
            
        }

        //Listboxok,comboboxok feltöltése
        private void listboxokFeltolt()
        {
            felvettRendelesek.Items.Clear();
            keszRendelesek.Items.Clear();
            

            if (Rendelesek.rendelesekLista.Any())
                foreach (Rendeles r in Rendelesek.rendelesekLista)
                {
                    if (r.etelstatus == 1 && r.italstatus == 1)
                        felvettRendelesek.Items.Add(r);
                    else if(r.etelstatus < 4 && r.italstatus < 4)
                        keszRendelesek.Items.Add(r);

                    if(!rendelesekComboBox.Items.Contains(r.razon) && r.etelstatus < 3 && r.italstatus < 3)
                        rendelesekComboBox.Items.Add(r.razon);
                }
        }

        private void asztalokComboBoxFeltolt()
        {
            asztalokComboBox.Items.Clear();
            Rendelesek.rendelesekFrissit();
            for (int i = 0; i < 10; i++)
            {
                if (Rendelesek.rendelesekLista.Any(x => x.asztal == i && x.etelstatus < 3))
                    continue;
                asztalokComboBox.Items.Add(i);
            }
        }

        private void foglalasComboBoxFeltolt()
        {
            foglalasComboBox.Items.Clear();
            Felhasznalok.felhasznalokFrissit();
            Foglalasok.foglalasokFrissit();
            foreach (Foglalas f in Foglalasok.foglalasLista.Where(x=>x.megjelent == false))
            {
                foglalasComboBox.Items.Add(f);
            }
        }


        //Onselectionchanged eventekre 
        private void tetelUIIsEnabled(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.SelectedItem is not null)
                tetelUIGomb.IsEnabled = true;
            else
                tetelUIGomb.IsEnabled = false;
        }
        private void listBoxVezerloIsEnabled(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            StackPanel vezerlo = (StackPanel)FindName(lb.Name+"Vezerlo");
            if (lb.SelectedItem is not null)
                vezerlo.IsEnabled = true;
            else
                vezerlo.IsEnabled = false;
        }
        private void rendelesFelvetelIsEnabled(object sender, SelectionChangedEventArgs e)
        {
            if (foglalasComboBox.SelectedItem is not null && asztalokComboBox.SelectedItem is not null)
                rendelesFelvetelGomb.IsEnabled = true;
            else
                rendelesFelvetelGomb.IsEnabled = false;
        }


        //Új ablakok megnyitása
        private void tetelUiMegnyit(object sender, RoutedEventArgs e)
        {
            Window tetelUI = new TetelUI((int)rendelesekComboBox.SelectedItem);
            tetelUI.Owner = this;
            tetelUI.Show();
        }
        private void rendelesReszletek(object sender, RoutedEventArgs e)
        {
            Button reszletekgomb = (Button)sender;
            string nev = reszletekgomb.Name;
            Rendeles rendeles = nev == "felvettRendelesekReszletek" ? (Rendeles)felvettRendelesek.SelectedItem : (Rendeles)keszRendelesek.SelectedItem;

            Window reszletek = new RendelesReszletekUI(rendeles);
            reszletek.Owner = this;
            reszletek.Show();
        }


        //Mysql törlés,beszúrás
        private void rendelesFelvetel(object sender, RoutedEventArgs e)
        {
            Foglalas foglalas = (Foglalas)foglalasComboBox.SelectedItem;
            MySqlParameter foglalasparam = new("@fazon", foglalas.azon);
            MySqlParameter asztalparam = new("@asztal", asztalokComboBox.SelectedValue);
            List<MySqlParameter> rendelesFelvetelParams = new() { foglalasparam, asztalparam };
            List<string> eredmeny = MySQL.query("rendelesfelvetel", true, rendelesFelvetelParams);
            MessageBox.Show(eredmeny[0]);
            asztalokComboBoxFeltolt();
            listboxokFeltolt();
        }


        private void rendelesTorles(object sender, RoutedEventArgs e)
        {
            Button torlesGomb = (Button)sender;
            string nev = torlesGomb.Name;
            Rendeles rendeles = nev == "felvettRendelesekTorles" ? (Rendeles)felvettRendelesek.SelectedItem : (Rendeles)keszRendelesek.SelectedItem;

            MessageBoxResult megerosites = MessageBox.Show($"Biztosan törölni akarod a {rendeles.razon} számú rendelést?", "Rendelés Törlése", MessageBoxButton.OKCancel);
            if (megerosites == MessageBoxResult.OK)
            {
                MySqlParameter razonparam = new("@razon", rendeles.razon);
                List<MySqlParameter> rendelesTorlesParams = new() { razonparam };
                List<string> eredmeny = MySQL.query("rendelestorles", true, rendelesTorlesParams);
                MessageBox.Show(eredmeny[0]);
                asztalokComboBoxFeltolt();
                listboxokFeltolt();
            }

        }

        private void fizetesreVar(object sender, RoutedEventArgs e)
        {
            Rendeles rendeles = (Rendeles)keszRendelesek.SelectedItem;
            Button allapotvaltoztatgomb = (Button)sender;
            string nev = allapotvaltoztatgomb.Name;
            int allapot = nev == "fVar" ? 3 : 4;

            MessageBoxResult megerosites = MessageBox.Show($"Biztosan meg akarod változtatni a {rendeles.razon} számú rendelés állapotát?", "Rendelés állapot", MessageBoxButton.OKCancel);
            if(megerosites == MessageBoxResult.OK)
            {
                MySqlParameter razonparam = new("@razon", rendeles.razon);
                MySqlParameter allapotparam = new("@allapot", allapot);
                List<MySqlParameter> rendelesAllapotValtoztat = new() { razonparam, allapotparam};
                List<string> eredmeny = MySQL.query("rendelesallapotvaltoztat", true, rendelesAllapotValtoztat);
                MessageBox.Show(eredmeny[0]);
            }
        }
    }
}
