using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
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

namespace _1.semesterprojekt
{
    class BrugerVM : INotifyPropertyChanged
    {
        #region Property
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public static ObservableCollection<Bruger> BrugerCollection { get; set; }
        public Bruger SelectedBruger { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Kodeord { get; set; }
        public string GettetKode { get; set; }

        public Frame frame = (Frame)Window.Current.Content;

        public BrugerVM()
        {
            BrugerCollection = new ObservableCollection<Bruger>();
            string curFile = @"C:\Users\rasmu\AppData\Local\Packages\347085fc-770a-47bf-87e0-14e3e41dd8a6_2g8htre5d9nf2\LocalState\Brugere.json";
            if (!File.Exists(curFile))
            {
                BrugerCollection.Add(new Bruger("1", "1"));
                SaveBruger();
            }
            Load();
        }

        public BrugerVM(ObservableCollection<Bruger> brugerCollection, int id, string email, string kodeord)
        {
            BrugerCollection = brugerCollection;
            Id = id;
            Email = email;
            Kodeord = kodeord;
        }

        public void Logind()
        {
            foreach (var bruger in BrugerCollection)
            {
                if (Email == bruger.Email && Kodeord == bruger.Kodeord)
                {               
                    frame.Navigate(typeof(Forside));
                }
            }
        }

        public void OpretBruger()
        {
            BrugerCollection.Add(new Bruger(Email, Kodeord));
            OnPropertyChanged();
            SaveBruger();
        }

        public void OpdaterBruger()
        {
            if (SelectedBruger != null)
            {
                var GetBruger = (from bruger in BrugerCollection where bruger.Id == SelectedBruger.Id select bruger).FirstOrDefault();
                GetBruger.Email = Email;
                SaveBruger();
                OnPropertyChanged();
            }
        }

        public void SletBruger()
        {
            if (SelectedBruger != null)
            {
                BrugerCollection.Remove(SelectedBruger);
                SaveBruger();
            }
        }

        public void SaveBruger()
        {
            GemBruger.SaveBrugerAsJsonAsync(BrugerCollection);
        }

        public async void Load()
        {
            string curFile = @"C:\Users\rasmu\AppData\Local\Packages\347085fc-770a-47bf-87e0-14e3e41dd8a6_2g8htre5d9nf2\LocalState\Brugere.json";
            if (File.Exists(curFile))
            {
                var brugerCollection = await GemBruger.LoadBrugerFromJsonAsync();
                BrugerCollection.Clear();
                foreach (var bruger in brugerCollection)
                {
                    BrugerCollection.Add(bruger);
                }
            }
        }

        public void GetKode()
        {
            
        }
    }
}
