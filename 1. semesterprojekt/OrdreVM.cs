using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using _1.semesterprojekt.Annotations;
using Windows.UI.Xaml.Data;

namespace _1.semesterprojekt
{
    class OrdreVM : INotifyPropertyChanged
    {
        #region Property
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
       
        public Produkt SelectedProdukt { get; set; }
        public int SelectedIndex { get; set; }

        public static ObservableCollection<Ordre> DeaktiveredeOrdrerCollection { get; set; }
        public static ObservableCollection<Ordre> OrdrerCollection { get; set; }
        public string OrdreNr { get; set; }
        public string KundeCVRnummer { get; set; }
        public Ordre SelectedOrdre { get; set; }
        public DateTime DateTime { get; set; }
        private DateTimeOffset _date;

        public DateTimeOffset Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public bool Laminering { get; set; }
        public bool Fragt { get; set; }
        public bool OpTil10 { get; set; }
        public bool Montering { get; set; }
        public bool Afhentes { get; set; }
        public bool Accepteret { get; set; }
        public bool Aktiveret { get; set; }

        public static ObservableCollection<Produkt> ProduktCollection { get; set; }
        public string ProduktNavn { get; set; }
        public string Produkttype { get; set; }
        public string Medie { get; set; }
        public string Folie { get; set; }
        public string Farve { get; set; }
        public string Længde { get; set; }
        public string Bredde { get; set; }
        public string Antal { get; set; }
        public double KvadratM { get; set; }
        public string Kommentar { get; set; }

        public ObservableCollection<Kunde> KundeCollection { get; set; }
        public string Navn { get; set; }
        public string Firma { get; set; }
        public string Adresse { get; set; }
        public string By { get; set; }
        public string Postnr { get; set; }
        public string Mail { get; set; }
        public string Telefonnr { get; set; }

        private ICommand _loadCommand;
        private ICommand _saveCommand;

        public ICommand LoadCommand
        {
            get { return _loadCommand; }
            set { _loadCommand = value; }
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand; }
            set { _saveCommand = value; }
        }

        public Frame frame = (Frame)Window.Current.Content;

        public OrdreVM()
        {
            OrdrerCollection = new ObservableCollection<Ordre>();
            ProduktCollection = new ObservableCollection<Produkt>();
            KundeCollection = new ObservableCollection<Kunde>();
            DeaktiveredeOrdrerCollection = new ObservableCollection<Ordre>();

            DateTime dt = DateTime.Now;
            
            _date = new DateTimeOffset(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0, new TimeSpan());

            Load();
        }

        public OrdreVM(ObservableCollection<Ordre> ordrerCollection, DateTimeOffset deadline, ObservableCollection<Produkt> produktCollection, string ordreNr, string kundeCVRnummer, bool laminering, bool fragt, bool opTil10, bool montering, bool afhentes)
        {
            OrdrerCollection = ordrerCollection;
            ProduktCollection = produktCollection;
            OrdreNr = ordreNr;
            Laminering = laminering;
            Fragt = fragt;
            OpTil10 = opTil10;
            Montering = montering;
            Afhentes = afhentes;
            Date = deadline;
            KundeCVRnummer = kundeCVRnummer;
            ProduktCollection.Clear();
        }

        public void OpretProdukt()
        {
            ProduktCollection.Add(new Produkt(ProduktNavn, Produkttype, Medie, Folie, Farve, Længde, Bredde, Antal, Kommentar));
            OnPropertyChanged();
        }

        public void OpretOrdre()
        {
            KundeCollection.Add(new Kunde(KundeCVRnummer, Navn, Firma, Adresse, By, Postnr, Mail, Telefonnr));
            OrdrerCollection.Add(new Ordre(Date, Laminering, Fragt, OpTil10, Montering, Afhentes, KundeCVRnummer, ProduktCollection));
            OnPropertyChanged();
            SaveOrdrer();
            SaveKunder();
            frame.Navigate(typeof(Forside));
        }

        public void SaveOrdrer()
        {
            foreach (var ordre in OrdrerCollection)
            {
                if (ordre.Aktiveret == false)
                {
                    OrdrerCollection.Remove(ordre);
                    DeaktiveredeOrdrerCollection.Add(ordre);
                }
                else
                {
                    DeaktiveredeOrdrerCollection.Remove(ordre);
                }
            }
            GemAktiveredeOrdre.SaveOrdreAsJsonAsync(OrdrerCollection);
            GemDeaktiveredeOrdrer.SaveOrdreAsJsonAsync(DeaktiveredeOrdrerCollection);
        }
        public void SaveKunder()
        {
            GemKunde.SaveKundeAsJsonAsync(KundeCollection);
        }

        public async void Load()
        {
            string curFile = @"C:\Users\rasmu\AppData\Local\Packages\347085fc-770a-47bf-87e0-14e3e41dd8a6_2g8htre5d9nf2\LocalState\Ordrer.json";
            if (File.Exists(curFile))
            {
                var ordrerCollection = await GemAktiveredeOrdre.LoadOrdreFromJsonAsync();
                OrdrerCollection.Clear();
                foreach (var ordre in ordrerCollection)
                {
                    OrdrerCollection.Add(ordre);
                }
            }
            string curFile2 = @"C:\Users\rasmu\AppData\Local\Packages\347085fc-770a-47bf-87e0-14e3e41dd8a6_2g8htre5d9nf2\LocalState\DeaktiveredeOrdrer.json";
            if (File.Exists(curFile2))
            {
                var deaktiveredeOrdrerCollection = await GemDeaktiveredeOrdrer.LoadOrdreFromJsonAsync();
                DeaktiveredeOrdrerCollection.Clear();
                foreach (var ordre in deaktiveredeOrdrerCollection)
                {
                    DeaktiveredeOrdrerCollection.Add(ordre);
                }
            }
            string curFile3 = @"C:\Users\rasmu\AppData\Local\Packages\347085fc-770a-47bf-87e0-14e3e41dd8a6_2g8htre5d9nf2\LocalState\Kunder.json";
            if (File.Exists(curFile3))
            {
                var kundeCollection = await GemKunde.LoadKunderFromJsonAsync();
                KundeCollection.Clear();
                foreach (var kunde in kundeCollection)
                {
                    KundeCollection.Add(kunde);
                }
            }
        }

        public void OpdaterOrdre()
        {
            if (SelectedOrdre != null)
            {
                var GetOrdre = (from ordre in OrdrerCollection where ordre.OrdreNummer == SelectedOrdre.OrdreNummer select ordre).FirstOrDefault();
                //Ordre NewOrdre = new Ordre(GetOrdre.Date, GetOrdre.Laminering, GetOrdre.Fragt, GetOrdre.OpTil10, GetOrdre.Montering, GetOrdre.Afhentes, GetOrdre.KundeCVRnummer, GetOrdre.ProduktCollection);
                //OrdrerCollection.Remove(GetOrdre);
                if (KundeCVRnummer != "")
                {
                    GetOrdre.KundeCVRnummer = KundeCVRnummer;
                }
                if (Date != null)
                {
                    GetOrdre.Date = Date;
                }
                if (Laminering == false && GetOrdre.Laminering == true || Laminering == true && GetOrdre.Laminering == false)
                {
                    GetOrdre.Laminering = Laminering;
                }
                if (Fragt == false && GetOrdre.Fragt == true || Fragt == true && GetOrdre.Fragt == false)
                {
                    GetOrdre.Fragt = Fragt;
                }
                if (OpTil10 == false && GetOrdre.OpTil10 == true || OpTil10 == true && GetOrdre.OpTil10 == false)
                {
                    GetOrdre.OpTil10 = OpTil10;
                }
                if (Montering == false && GetOrdre.Montering == true || Montering == true && GetOrdre.Montering == false)
                {
                    GetOrdre.Montering = Montering;
                }
                if (Afhentes == false && GetOrdre.Afhentes == true || Afhentes == true && GetOrdre.Afhentes == false)
                {
                    GetOrdre.Afhentes = Afhentes;
                }

                //OrdrerCollection.Add(NewOrdre);
            }

            if (SelectedIndex > -1)
            {
                
            }
            SaveOrdrer();
            OnPropertyChanged();
        }

        public void AccepterOrdre()
        {
            if (SelectedOrdre != null)
            {
                var OrdreToAccepter = (from ordre in OrdrerCollection where SelectedOrdre.OrdreNummer == ordre.OrdreNummer select ordre).FirstOrDefault();

                OrdreToAccepter.Accepteret = true;

                SaveOrdrer();
                OnPropertyChanged();
            }
        }

        public void AktiverOrdre()
        {
            if (SelectedOrdre != null)
            {
                var OrdreToActivate = (from ordre in DeaktiveredeOrdrerCollection where SelectedOrdre.OrdreNummer == ordre.OrdreNummer select ordre).FirstOrDefault();

                OrdreToActivate.Aktiveret = true;
                DeaktiveredeOrdrerCollection.Remove(OrdreToActivate);
                OrdrerCollection.Add(OrdreToActivate);
                SaveOrdrer();
                OnPropertyChanged();
            }
        }

        public void DeaktiverOrdre()
        {
            if (SelectedOrdre != null)
            {
                var OrdreToDeactivate = (from ordre in OrdrerCollection where SelectedOrdre.OrdreNummer == ordre.OrdreNummer select ordre).FirstOrDefault();

                OrdreToDeactivate.Aktiveret = false;
                OrdrerCollection.Remove(OrdreToDeactivate);
                DeaktiveredeOrdrerCollection.Add(OrdreToDeactivate);
                SaveOrdrer();
                OnPropertyChanged();
            }
        }

        public void SletOrdre()
        {
            if (SelectedOrdre != null)
            {
                var OrdreToSlet = (from ordre in DeaktiveredeOrdrerCollection where SelectedOrdre.OrdreNummer == ordre.OrdreNummer select ordre).FirstOrDefault();

                DeaktiveredeOrdrerCollection.Remove(OrdreToSlet);

                SaveOrdrer();
                OnPropertyChanged();
            }
        }
    }
}
