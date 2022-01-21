using System;
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
            comboListBoxokFrissites(); //Feltöltjük a combo meg listboxokat
            jelszoValtoztatMenu.Click += JelszoValtoztatEljaras.jelszoValtoztat;
            Task.Run(() => listboxokFrissitAsync()); //Task.Run egy async eljárást fog elindítani
            
        }

        //Async frissítések
        #region Async funkciok
        /// <summary>
        /// Async eljárás, nem befolyásolja a main szál futását, minden ami itt történik másik CPU thread-en történik.
        /// </summary>
        private async void listboxokFrissitAsync()
        {
            //Egy végtelen ciklusba 
            while (true)
            {
                rendelesekFrissitListBoxokFeltolt(); //frissítünk a rendeléseken és újra feltöltjük a listboxokat
                await Task.Delay(10000); //await - szóval várjon meg egy 10000 milisecondos késleltetést, ez után a végtelen ciklus kezdődik elölről
            }
            
        }

        private void rendelesekFrissitListBoxokFeltolt()
        {
            //Mivel a listboxainkat a main thread birtokolja, és ezt az eljárást egy async eljárás hívja meg, nem tudja csak simán módosítani
            Dispatcher.Invoke(() => // ezért az ablak Dispatcherjének kell odaadni a feladatot
            {
                Rendelesek.rendelesekFrissit(); //ráfrissítünk a rendelések listánkra
                listboxokFeltolt(); //és mostmár ő fel tudja tölteni a listboxainkat
                asztalokRajzol();
            });
            
        }
        #endregion

        //Listboxok,comboboxok feltöltése
        #region List es comboboxok feltoltese
        /// <summary>
        /// Törli a két listbox tartalmát és feltölti a rendelések listából elemekkel
        /// </summary>
        private void listboxokFeltolt()
        {
            felvettRendelesek.Items.Clear();
            keszRendelesek.Items.Clear();

            foreach (Rendeles r in Rendelesek.rendelesekLista)
            {
                if (r.etelstatus == 1 && r.italstatus == 1) //ha mindkettő 1es statusu akkor folyamatban van még, szóval a felvett rendelésekhez megy
                    felvettRendelesek.Items.Add(r);
                else if(r.etelstatus < 4 && r.italstatus < 4)//amint az egyik már nem 1es de kisebb mint 4, akkor átkerül a kész rendelésekhez
                    keszRendelesek.Items.Add(r);
            }
        }

        /// <summary>
        /// Törli a rendelések comboboxunkat és feltölti a rendelések listából
        /// </summary>
        private void rendelesekComboBoxFeltolt()
        {
            rendelesekComboBox.Items.Clear();
            foreach(Rendeles r in Rendelesek.rendelesekLista)
            {
                //csak azokat a rendeléseket rakjuk bele a comboboxba(amikhez tételeket tudunk felvenni) amik még nem lettek kifizetve
                if (r.etelstatus < 4 && r.italstatus < 4) 
                    rendelesekComboBox.Items.Add(r.razon);
            }
        }

        /// <summary>
        /// Feltölti az asztalok comboboxot 10 asztallal.
        /// </summary>
        private void asztalokComboBoxFeltolt()
        {
            asztalokComboBox.Items.Clear();
            Rendelesek.rendelesekFrissit();//ráfrissítünk a rendelésekre
            for (int i = 1; i <= 10; i++)
            {
                //ha az asztal benne van a rendelések listába és még nincs fizetve
                if (Rendelesek.rendelesekLista.Any(x => x.asztal == i && x.etelstatus < 4 && x.italstatus < 4))
                    continue; //akkor az adott számot skippeljük
                asztalokComboBox.Items.Add(i);
            }
        }

        /// <summary>
        /// Feltölti a foglalások comboboxot a mai foglalásokkal.
        /// </summary>
        private void foglalasComboBoxFeltolt()
        {
            foglalasComboBox.Items.Clear();
            Felhasznalok.felhasznalokFrissit();//ráfrissítünk a felhasználókra és a foglalásokra is ha időközben új ember regisztrált és már foglalt is.
            Foglalasok.foglalasokFrissit();
            // végigmegyünk a foglalás listán ahol még nem jelent meg a vendég, és az időpont megegyezik a mai dátummal
            foreach (Foglalas f in Foglalasok.foglalasLista
            .Where(x=>x.megjelent == false &&
            x.idopontDatum.Month == DateTime.Now.Month && 
            x.idopontDatum.Day == DateTime.Now.Day))
            {
                foglalasComboBox.Items.Add(f);//automatikusan a .tostring lesz meghívva rá
            }
        }

        /// <summary>
        /// Minden comboboxot meg listboxot töröl és újra feltölt, és újra rajzolja az asztalokat.
        /// </summary>
        private void comboListBoxokFrissites()
        {
            asztalokComboBoxFeltolt();
            foglalasComboBoxFeltolt();
            rendelesekComboBoxFeltolt();
            listboxokFeltolt();
            asztalokRajzol();
        }
        #endregion

        //Onselectionchanged,Checked eventekre
        #region Eventek
        private void tetelUIIsEnabled(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender; //Rendelés combobox lesz a sender
            if (cb.SelectedItem is not null) //ha ki van választva valami
                tetelUIGomb.IsEnabled = true; //akkor a tétel felvétele gomb aktiválódik
            else
                tetelUIGomb.IsEnabled = false;
        }
        private void listBoxVezerloIsEnabled(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender; //egyik listbox lesz a sender
            //a listbox neve felvettRendelesek vagy keszRendelesek lehet, megkeressük a keszRendelesekVezerlo vagy felvett stackpanelt.
            StackPanel vezerlo = (StackPanel)FindName(lb.Name+"Vezerlo"); 
            if (lb.SelectedItem is not null) //ha van kiválasztott elem
                vezerlo.IsEnabled = true; //akkor a hozzá tartozó vezérlő stackpanel használható
            else
                vezerlo.IsEnabled = false;
        }
        private void rendelesFelvetelIsEnabled(object sender, SelectionChangedEventArgs e)
        {
            if (asztalokComboBox.SelectedItem is not null) //ha van asztal kiválasztva
                rendelesFelvetelGomb.IsEnabled = true; //akkor tudunk rendelést felvenni
            else
                rendelesFelvetelGomb.IsEnabled = false;
        }
        private void vendegChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender; //a sender a betérő vendég checkbox lesz
            if (cb.IsChecked == true) //ha bepipálja
            {
                foglalasComboBox.SelectedItem = null; //a foglalás combobox selected itemjét ki null-ozzuk
                foglalasComboBox.IsEnabled = false; //és ki szürkítjük
            }
            else
                foglalasComboBox.IsEnabled = true;
        }
        private void vezerlokFrissitese(object sender, RoutedEventArgs e)
        {
            comboListBoxokFrissites();
        }
        #endregion

        //Új ablakok megnyitása
        #region Uj ablakok
        private void tetelUiMegnyit(object sender, RoutedEventArgs e)
        {
            Window tetelUI = new TetelUI((int)rendelesekComboBox.SelectedItem); //átadjuk a rendelés számot az ablaknak
            tetelUI.Owner = this;
            tetelUI.Show();
        }
        private void rendelesReszletek(object sender, RoutedEventArgs e)
        {
            Button reszletekgomb = (Button)sender; //2 részletek gombunk van
            string nev = reszletekgomb.Name; //megnézzük a nevét
            //ha a felvettrendelesreszletek névvel érkezik akkor a felvett rendelések listboxból nézzük a kiválasztott itemet,ha nem akkor a készből
            Rendeles rendeles = nev == "felvettRendelesekReszletek" ? (Rendeles)felvettRendelesek.SelectedItem : (Rendeles)keszRendelesek.SelectedItem;

            Window reszletek = new RendelesReszletekUI(rendeles); //átadjuk a rendelést paraméterként
            reszletek.Owner = this;
            reszletek.Show();
        }
        #endregion

        //Mysql törlés,beszúrás
        #region MySQL eljarasok
        private void rendelesFelvetel(object sender, RoutedEventArgs e)
        {
            Foglalas foglalas = new(); //létrehozunk egy üres konstruktoros foglalást,mert ha betérő vendég érkezik annak nincs még foglalása
            if (foglalasComboBox.SelectedItem is not null) //viszont ha van foglalás kiválasztva
            {
                foglalas = (Foglalas)foglalasComboBox.SelectedItem; //akkor a foglalásunk a kiválaszott foglalás lesz
            }
            else
            {
                MySQL.query("vendegfoglalasbeszur", true); //különben beszúrunk neki egy foglalást az adott időpontra,1es felhasználó idvel (vendég)
                Foglalasok.foglalasokFrissit(); //ráfrissítünk a foglalások listára
                foglalas = Foglalasok.foglalasLista.Last(); //és amiket visszakaptunk a beszúrás után, az utolsó elem kell hogy ez a vendég foglalás legyen
            }

            //Létrehozzuk a paramétereket, konstruktor első paraméter a cserélendő paraméter, a második az érték
            MySqlParameter foglalasparam = new("@fazon", foglalas.fazon);
            MySqlParameter asztalparam = new("@asztal", asztalokComboBox.SelectedValue);

            List<MySqlParameter> rendelesFelvetelParams = new() { foglalasparam, asztalparam }; //létrehozzuk a paraméter listát
            List<string> eredmeny = MySQL.query("rendelesfelvetel", true, rendelesFelvetelParams); //elmentjük az eredményt,vagy hiba vagy hogy a művelet sikeres

            MessageBox.Show(eredmeny[0]);//kiiratjuk akármelyik is az
            comboListBoxokFrissites();//ráfrissítünk a combo és listboxokra
        }


        private void rendelesTorles(object sender, RoutedEventArgs e)
        {
            Button torlesGomb = (Button)sender; //2 törlés gombunk van
            string nev = torlesGomb.Name; // megnézzük a nevét
            //ha felvettrendelesektorles akkor felvett rendelésekből akarjuk a kiválaszott elemet törölni,ha nem akkor készből
            Rendeles rendeles = nev == "felvettRendelesekTorles" ? (Rendeles)felvettRendelesek.SelectedItem : (Rendeles)keszRendelesek.SelectedItem;

            //megerősítés textbox értékét lementjük
            MessageBoxResult megerosites = MessageBox.Show($"Biztosan törölni akarod a {rendeles.razon} számú rendelést?", "Rendelés Törlése", MessageBoxButton.OKCancel);

            if (megerosites == MessageBoxResult.OK) //ha ok-ra nyomott
            {
                //létrehozzuk az razon paramétert
                MySqlParameter razonparam = new("@razon", rendeles.razon);
                List<MySqlParameter> rendelesTorlesParams = new() { razonparam }; //és a paraméterlistát ebből
                List<string> eredmeny = MySQL.query("rendelestorles", true, rendelesTorlesParams); //elmentjük az eredményt
                MessageBox.Show(eredmeny[0]);
                comboListBoxokFrissites(); //ráfrissítünk a combo és listboxokra
            }

        }

        private void fizetesreVar(object sender, RoutedEventArgs e)
        {
            Rendeles rendeles = (Rendeles)keszRendelesek.SelectedItem; //a rendelésünk a készrendelések listboxból a kiválasztott item
            Button allapotvaltoztatgomb = (Button)sender; //2 gomb hívhatja meg ezt, vagy a fizetésre vár, vagy a fizetve gomb
            string nev = allapotvaltoztatgomb.Name; //megnézzük név alapján melyik az
            int allapot = nev == "fVar" ? 3 : 4; //ha az fVar fizetésre várra akarjuk rakni szóval 3as lesz az állapot, különben 4es mint fizetve

            //megerősítés értékét lementjük
            MessageBoxResult megerosites = MessageBox.Show($"Biztosan meg akarod változtatni a {rendeles.razon} számú rendelés állapotát?", "Rendelés állapot", MessageBoxButton.OKCancel);

            if(megerosites == MessageBoxResult.OK)//ha biztosan meg akarja változtatni
            {
                //Létrehozzuk a paramétereket, konstruktor első paraméter a cserélendő paraméter, a második az érték
                MySqlParameter razonparam = new("@razon", rendeles.razon);
                MySqlParameter allapotparam = new("@allapot", allapot);
                List<MySqlParameter> rendelesAllapotValtoztat = new() { razonparam, allapotparam}; //paraméter listát is létrehozzuk
                List<string> eredmeny = MySQL.query("rendelesallapotvaltoztat", true, rendelesAllapotValtoztat); //lementjük az eredményt

                if(eredmeny.Any())
                    MessageBox.Show(eredmeny[0]);

                comboListBoxokFrissites(); //ráfrissítünk a combo és listboxokra
            }
        }
        #endregion

        //Asztal rajzolás, kijelentkezés
        #region Egyeb eljarasok
        private void asztalokRajzol()
        {
            //töröljük a canvas elemeit
            asztalokCanvas.Children.Clear();
            double x = 50; //50pxről kezdünk balról
            bool fentE = true; //és fentről
            for (int i = 1; i <= 10; i++) //10 asztalt rajzolunk, a ciklusváltozónk az asztal száma amit átadunk
            {
                asztalokCanvas.Children.Add(Asztalok.rajzol(x, fentE, i)); //kirajzoltatjuk az asztalt
                fentE = !fentE; //következő az ellentéte lesz 
                if (i > 0 && fentE) //a legelső asztalt még nem akarjuk +35el eltolni, de a következő fentit már mellé akarjuk mindig rakni
                    x += 35;
            }
        }

        private void kijelentkezes(object sender, RoutedEventArgs e)
        {
            //Kijelentkezés felső menü
            AktualisFelhasznalo.felhasznalo = null; //az aktuális felhasználónkat null-ra rakjuk
            Window bejelentkezes = new Bejelentkezes(); //létrehozunk egy új bejelentkezés ablakot
            bejelentkezes.Show(); //előhozzuk
            Close(); //ezt meg bezárjuk
        }
        #endregion


    }
}
