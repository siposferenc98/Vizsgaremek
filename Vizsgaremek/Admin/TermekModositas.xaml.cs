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
    /// Interaction logic for TermekModositas.xaml
    /// </summary>
    public partial class TermekModositas : Window
    {
        private readonly Termek termek;
        private readonly char tipus;
        public TermekModositas(Termek termek, char tipus)
        {
            InitializeComponent();
            this.termek = termek;
            this.tipus = tipus;
        }


    }
}
