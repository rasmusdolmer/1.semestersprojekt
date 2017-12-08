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

    class GemKunde
    {
        private static string JsonFileName = "Kunder.json";

        public static async void SaveKundeAsJsonAsync(ObservableCollection<Kunde> kunder)
        {
            string kundeJsonString = JsonConvert.SerializeObject(kunder);
            SerializeKunderFileAsync(kundeJsonString, JsonFileName);
        }

        public static async Task<List<Kunde>> LoadKunderFromJsonAsync()
        {
            string kundeJsonString = await DeserializeKundeFileAsync(JsonFileName);
            if (kundeJsonString != null)
                return (List<Kunde>)JsonConvert.DeserializeObject(kundeJsonString, typeof(List<Kunde>));
            return null;
        }



        private static async void SerializeKunderFileAsync(string kundeJsonString, string fileName)
        {
            StorageFile localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(localFile, kundeJsonString);
        }


        private static async Task<string> DeserializeKundeFileAsync(string fileName)
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