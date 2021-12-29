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
            Task.Run(() => listboxokFrissitAsync());
            
        }

        private async void listboxokFrissitAsync()
        {

            while (true)
            {
                keszListboxFrissit();
                await Task.Delay(5000);
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

        private void listboxokFeltolt()
        {
            nyitottListBox.Items.Clear();
            zartListBox.Items.Clear();

            if (Rendelesek.rendelesekLista.Any())
                foreach (Rendeles r in Rendelesek.rendelesekLista)
                {
                    if (r.etelstatus > 1 || r.italstatus> 1)
                        zartListBox.Items.Add(r);
                    else
                        nyitottListBox.Items.Add(r);
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

        private void tetelUiMegnyit(object sender, RoutedEventArgs e)
        {
            Window tetelUI = new TetelUI();
            tetelUI.Owner = this;
            tetelUI.Show();
        }
    }
}
