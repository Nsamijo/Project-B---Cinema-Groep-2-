using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Bioscoop.Repository
{
    class FilmData
    {
        public static string filmPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Film.json";

        public static List<FilmModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = filmPath;
            string _json = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<List<FilmModel>>(_json);
        }
        private static void SaveData(List<FilmModel> FilmschemaData) //opslaan en schrijven naar json functie
        {
            var jsondata = JsonConvert.SerializeObject(FilmschemaData, Formatting.Indented);
            System.IO.File.WriteAllText(filmPath, jsondata);
        }
    }
}
