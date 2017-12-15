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

        public string SelectedSortering { get; set; }

        public Produkt SelectedProdukt { get; set; }
        public int SelectedIndex { get; set; }

        public string SearchInput { get; set; }

        public static ObservableCollection<Ordre> DeaktiveredeOrdrerCollection { get; set; }
        public static ObservableCollection<Ordre> OrdrerCollection { get; set; }
        public static ObservableCollection<Ordre> BackUpOrdrerCollection { get; set; }
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
        public string Status { get; set; }

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

        public static ObservableCollection<Kunde> KundeCollection { get; set; }
        public static ObservableCollection<Kunde> BackUpKundeCollection { get; set; }
        public Kunde SelectedKunde { get; set; }
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
            BackUpOrdrerCollection = new ObservableCollection<Ordre>();
            OrdrerCollection = new ObservableCollection<Ordre>();
            DeaktiveredeOrdrerCollection = new ObservableCollection<Ordre>();
            ProduktCollection = new ObservableCollection<Produkt>();
            KundeCollection = new ObservableCollection<Kunde>();
            BackUpKundeCollection = new ObservableCollection<Kunde>();

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
            if (ProduktNavn != null && Produkttype != null && Medie != null && Folie != null && Farve != null && Længde != null && Bredde != null && Antal != null && Kommentar != null)
            {
                ProduktCollection.Add(new Produkt(ProduktNavn, Produkttype, Medie, Folie, Farve, Længde, Bredde, Antal, Kommentar));
                OnPropertyChanged();
            }
        }

        public void OpretOrdre()
        {
            if (KundeCVRnummer != null && Navn != null && Mail != null && Telefonnr != null && Date != null && ProduktCollection.Count > 0 && Mail.Contains("@") && Mail.Contains(".") && !Telefonnr.Any(char.IsLetter) && !KundeCVRnummer.Any(char.IsLetter))
            {
                KundeCollection.Add(new Kunde(KundeCVRnummer, Navn, Firma, Adresse, By, Postnr, Mail, Telefonnr));
                OrdrerCollection.Add(new Ordre(Date, Laminering, Fragt, OpTil10, Montering, Afhentes, KundeCVRnummer, ProduktCollection));
                OnPropertyChanged();
                SaveKunder();
                SaveOrdrer();
                frame.Navigate(typeof(Forside));
            }
        }

        public void SaveOrdrer()
        {
            foreach (var ordre in OrdrerCollection)
            {
                if (ordre.Status == "Deaktiveret")
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

        public void Sorter()
        {
            if (BackUpOrdrerCollection.Count == 0)
            {
                foreach (var ordre in OrdrerCollection)
                {
                    BackUpOrdrerCollection.Add(ordre);
                }
            }
            OrdrerCollection.Clear();
            #region If statements
            if (SelectedSortering == "OrdreNummer")
            {
                var SorteretOrdre = (from ordre in BackUpOrdrerCollection orderby ordre.OrdreNummer descending select ordre);

                foreach (var ordre in SorteretOrdre)
                {
                    OrdrerCollection.Add(ordre);
                }
            }
            if (SelectedSortering == "Deadline")
            {
                var SorteretOrdre = (from ordre in BackUpOrdrerCollection orderby ordre.Date select ordre);

                foreach (var ordre in SorteretOrdre)
                {
                    OrdrerCollection.Add(ordre);
                }
            }
            if (SelectedSortering == "Oprettelsesdato")
            {
                var SorteretOrdre = (from ordre in BackUpOrdrerCollection orderby ordre.DateTime select ordre);

                foreach (var ordre in SorteretOrdre)
                {
                    OrdrerCollection.Add(ordre);
                }
            }
            if (SelectedSortering == "KundeNummer")
            {
                var SorteretOrdre = (from ordre in BackUpOrdrerCollection orderby ordre.KundeCVRnummer select ordre);

                foreach (var ordre in SorteretOrdre)
                {
                    OrdrerCollection.Add(ordre);
                }
            }
            if (SelectedSortering == "Accepteret/annuleret")
            {
                var SorteretOrdre = (from ordre in BackUpOrdrerCollection orderby ordre.Accepteret select ordre);

                foreach (var ordre in SorteretOrdre)
                {
                    OrdrerCollection.Add(ordre);
                }
            }          
            #endregion
        }

        public void SearchFilter()
        {
            if (BackUpOrdrerCollection.Count == 0)
            {
                foreach (var ordre in OrdrerCollection)
                {
                    BackUpOrdrerCollection.Add(ordre);
                }
            }
            var FilteredOrdrer = (from ordre in BackUpOrdrerCollection where ordre.OrdreNummer.Contains(SearchInput) || 
                                  ordre.KundeCVRnummer.Contains(SearchInput) || ordre.Status.Contains(SearchInput) || 
                                  ordre.OrdreNummer.ToLower().Contains(SearchInput) || ordre.KundeCVRnummer.ToLower().Contains(SearchInput) || 
                                  ordre.Status.ToLower().Contains(SearchInput) select ordre).ToList();
            if (SearchInput == "")
            {
                OrdrerCollection.Clear();
                foreach (var ordre in BackUpOrdrerCollection)
                {
                    OrdrerCollection.Add(ordre);
                }
            }
            else
            {
                OrdrerCollection.Clear();
                foreach (var ordre in FilteredOrdrer)
                {
                    OrdrerCollection.Add(ordre);
                }
            }
        }

        public void SearchFilterDeaktiverede()
        {
            if (BackUpOrdrerCollection.Count == 0)
            {
                foreach (var ordre in DeaktiveredeOrdrerCollection)
                {
                    BackUpOrdrerCollection.Add(ordre);
                }
            }
            var FilteredOrdrer = (from ordre in BackUpOrdrerCollection where ordre.OrdreNummer.Contains(SearchInput) || ordre.KundeCVRnummer.Contains(SearchInput) || ordre.Status.Contains(SearchInput) || ordre.OrdreNummer.ToLower().Contains(SearchInput) || ordre.KundeCVRnummer.ToLower().Contains(SearchInput) || ordre.Status.ToLower().Contains(SearchInput) select ordre).ToList();
            if (SearchInput == "")
            {
                DeaktiveredeOrdrerCollection.Clear();
                foreach (var ordre in BackUpOrdrerCollection)
                {
                    DeaktiveredeOrdrerCollection.Add(ordre);
                }
            }
            else
            {
                DeaktiveredeOrdrerCollection.Clear();
                foreach (var ordre in FilteredOrdrer)
                {
                    DeaktiveredeOrdrerCollection.Add(ordre);
                }
            }
        }

        public void SearchFilterKunder()
        {
            if (BackUpKundeCollection.Count == 0)
            {
                foreach (var kunde in KundeCollection)
                {
                    BackUpKundeCollection.Add(kunde);
                }
            }
            var FilteredKunde = (from kunde in BackUpKundeCollection where kunde.KundeCVRnummer.Contains(SearchInput) || kunde.Navn.Contains(SearchInput) || kunde.Firma.Contains(SearchInput) || kunde.Adresse.Contains(SearchInput) || kunde.By.Contains(SearchInput) || kunde.Postnr.Contains(SearchInput) || kunde.Mail.Contains(SearchInput) || kunde.Telefonnr.Contains(SearchInput) || kunde.KundeCVRnummer.ToLower().Contains(SearchInput) || kunde.Navn.ToLower().Contains(SearchInput) || kunde.Firma.ToLower().Contains(SearchInput) || kunde.Adresse.ToLower().Contains(SearchInput) || kunde.By.ToLower().Contains(SearchInput) || kunde.Postnr.ToLower().Contains(SearchInput) || kunde.Mail.ToLower().Contains(SearchInput) || kunde.Telefonnr.ToLower().Contains(SearchInput) select kunde).ToList();
            if (SearchInput == "")
            {
                KundeCollection.Clear();
                foreach (var kunde in BackUpKundeCollection)
                {
                    KundeCollection.Add(kunde);
                }
            }
            else
            {
                KundeCollection.Clear();
                foreach (var kunde in FilteredKunde)
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
                if (KundeCVRnummer != null)
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
                OrdreToAccepter.Status = "Accepteret af kunde. Ordre igangsat";

                SaveOrdrer();
                OnPropertyChanged();
            }
        }
        public void AnnulerOrdre()
        {
            if (SelectedOrdre != null)
            {
                var OrdreToAnnuler = (from ordre in OrdrerCollection where SelectedOrdre.OrdreNummer == ordre.OrdreNummer select ordre).FirstOrDefault();

                OrdreToAnnuler.Accepteret = false;
                OrdreToAnnuler.Status = "Afventer svar fra kunde";

                SaveOrdrer();
                OnPropertyChanged();
            }
        }

        public void AktiverOrdre()
        {
            if (SelectedOrdre != null)
            {
                var OrdreToActivate = (from ordre in DeaktiveredeOrdrerCollection where SelectedOrdre.OrdreNummer == ordre.OrdreNummer select ordre).FirstOrDefault();

                if (OrdreToActivate.Accepteret == true)
                {
                    OrdreToActivate.Status = "Ordre igangsat";
                }
                else
                {
                    OrdreToActivate.Status = "Afventer svar fra kunde";
                }
               
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

                OrdreToDeactivate.Status = "Ordre stoppet";
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

        public void OpretKunde()
        {
            if (KundeCVRnummer != null && Navn != null && Mail != null && Telefonnr != null && Mail.Contains("@") && Mail.Contains(".") && !Telefonnr.Any(char.IsLetter) && !KundeCVRnummer.Any(char.IsLetter))
            {
                KundeCollection.Add(new Kunde(KundeCVRnummer, Navn, Firma, Adresse, By, Postnr, Mail, Telefonnr));
                OnPropertyChanged();
                SaveKunder();
            }
        }

        public void OpdaterKunde()
        {
            var GetKunde = (from kunde in KundeCollection where kunde.Id == SelectedKunde.Id select kunde).FirstOrDefault();

            #region IfStatements (validering på om tekstbokse er udfyldt)
            if (KundeCVRnummer != null)
            {
                GetKunde.KundeCVRnummer = KundeCVRnummer;
            }
            if (Navn != "")
            {
                GetKunde.Navn = Navn;
            }
            if (Firma != "")
            {
                GetKunde.Firma = Firma;
            }
            if (Adresse != "")
            {
                GetKunde.Adresse = Adresse;
            }
            if (By != "")
            {
                GetKunde.By = By;
            }
            if (Postnr != "")
            {
                GetKunde.Postnr = Postnr;
            }
            if (Mail != "")
            {
                GetKunde.Mail = Mail;
            }
            if (Telefonnr != "")
            {
                GetKunde.Telefonnr = Telefonnr;
            }
            #endregion

            OnPropertyChanged();
            SaveKunder();
        }

        public void SletKunde()
        {
            var GetKunde = (from kunde in KundeCollection where kunde.Id == SelectedKunde.Id select kunde).FirstOrDefault();

            KundeCollection.Remove(GetKunde);
            OnPropertyChanged();
            SaveKunder();
        }
    }
}
