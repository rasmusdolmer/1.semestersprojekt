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

    class GemDeaktiveredeOrdrer
    {
        private static string JsonFileName = "DeaktiveredeOrdrer.json";

        public static async void SaveOrdreAsJsonAsync(ObservableCollection<Ordre> ordrer)
        {
            string ordreJsonString = JsonConvert.SerializeObject(ordrer);
            SerializeOrdreFileAsync(ordreJsonString, JsonFileName);
        }

        public static async Task<List<Ordre>> LoadOrdreFromJsonAsync()
        {
            string ordreJsonString = await DeserializeOrdreFileAsync(JsonFileName);
            if (ordreJsonString != null)
                return (List<Ordre>)JsonConvert.DeserializeObject(ordreJsonString, typeof(List<Ordre>));
            return null;
        }



        private static async void SerializeOrdreFileAsync(string ordreJsonString, string fileName)
        {
            StorageFile localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(localFile, ordreJsonString);
        }


        private static async Task<string> DeserializeOrdreFileAsync(string fileName)
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