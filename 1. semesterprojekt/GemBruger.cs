using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Newtonsoft.Json;
using _1.semesterprojekt;

namespace _1.semesterprojekt
{
    //Json.Net er downloaded til projektet via NuGet: Højreklik på projektet -> Manage NuGet Package

    class GemBruger
    {
        private static string JsonFileName = "Brugere.json";

        public static async void SaveBrugerAsJsonAsync(ObservableCollection<Bruger> Brugere)
        {
            string brugerJsonString = JsonConvert.SerializeObject(Brugere);
            SerializebrugereFileAsync(brugerJsonString, JsonFileName);
        }

        public static async Task<List<Bruger>> LoadBrugerFromJsonAsync()
        {
            string brugerJsonString = await DeserializeBrugerFileAsync(JsonFileName);
            if (brugerJsonString != null)
                return (List<Bruger>)JsonConvert.DeserializeObject(brugerJsonString, typeof(List<Bruger>));
            return null;
        }



        private static async void SerializebrugereFileAsync(string brugerJsonString, string fileName)
        {
            StorageFile localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(localFile, brugerJsonString);
        }


        private static async Task<string> DeserializeBrugerFileAsync(string fileName)
        {
            try
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(localFile);
            }
            catch (FileNotFoundException ex)
            {
                MessageDialogHelper.Show("Loading for the first time? - Try Add and Save some Notes before trying to Save for the first time", "File not Found");
                return null;
            }
        }


        private class MessageDialogHelper
        {
            public static async void Show(string content, string title)
            {
                MessageDialog messageDialog = new MessageDialog(content, title);
                await messageDialog.ShowAsync();
            }
        }

    }
}