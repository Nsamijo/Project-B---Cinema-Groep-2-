using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Bioscoop.Models;

namespace Bioscoop.Repository
{
    class FilmData //data ophaal functies
    {
        public static string jsonPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Film.json";

        public static List<FilmModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = jsonPath;
            string _json = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<List<FilmModel>>(_json);
        }
        private static void SaveData(List<FilmModel> filmData) //opslaan en schrijven naar json functie
        {
            var jsondata = JsonConvert.SerializeObject(filmData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
        public static int getId() //hoogste id opzoek functie
        {
            var filmData = LoadData();
            int filmId = filmData.Max(r => r.FilmId) + 1;

            return filmId;
        }
        public static void AddData(FilmModel data) //toevoeg functie
        {
            List<FilmModel> filmData = LoadData();
            data.FilmId = getId();
            filmData.Add(data);

            // Update json data string
            SaveData(filmData);
        }
        public static void RemoveData(FilmModel data) //verwijder functie
        {
            List<FilmModel> filmData = LoadData();
            List<FilmModel> filmschemaData = LoadData();

            var toRemove = filmData.Where(a => a.FilmId == data.FilmId).ToList();
            foreach (var remove in toRemove) filmData.Remove(remove);

            //voorfilmschema
            for (int i = 0; i < filmschemaData.Count; i++) if (filmschemaData[i].FilmId == data.FilmId)
                    if (filmschemaData[i].FilmId == data.FilmId) FilmschemaData.VerwijderProgramma(i);

            // Update json data string
            SaveData(filmData);
        }
        public static void EditData(FilmModel data) //aanpas functie
        {
            List<FilmModel> filmData = LoadData();

            var toEdit = filmData.Where(a => a.FilmId == data.FilmId).ToList();

            if (toEdit.Count() == 1){
                foreach(var x in toEdit){
                    x.Naam = data.Naam;
                    x.Omschrijving = data.Omschrijving;
                    x.Genre = data.Genre;
                    x.Duur = data.Duur;
                    x.Kijkwijzer = data.Kijkwijzer;
                    x.Status = data.Status;
                }
            }

            // Update json data string
            SaveData(filmData);
        }
        public static void SortData() //data sorteer functie
        {
            var filmData = LoadData();
            var orderedData = filmData.OrderBy(x => x.Status);

            var jsondata = JsonConvert.SerializeObject(orderedData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
    }
}
