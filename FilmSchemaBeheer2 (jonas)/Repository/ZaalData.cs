using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Bioscoop.Repository
{
    class ZaalData //data ophaal functies
    {
        public static string jsonPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Zaal.json";

        public static List<ZaalModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = jsonPath;
            string _json = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<List<ZaalModel>>(_json);
        }
        private static void SaveData(List<ZaalModel> zaalData) //opslaan en schrijven naar json functie
        {
            var jsondata = JsonConvert.SerializeObject(zaalData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
        public static int getId() //hoogste id opzoek functie
        {
            var zaalData = LoadData();
            int zaalId = zaalData.Max(r => r.ZaalId) + 1;

            return zaalId;
        }
        public static void AddData(ZaalModel data) //toevoeg functie
        {
            List<ZaalModel> zaalData = LoadData();
            data.ZaalId = getId();
            zaalData.Add(data);

            // Update json data string
            SaveData(zaalData);
        }
        public static void RemoveData(ZaalModel data) //verwijder functie
        {
            List<ZaalModel> zaalData = LoadData();

            var toRemove = zaalData.Where(a => a.ZaalId == data.ZaalId).ToList();
            foreach (var remove in toRemove) zaalData.Remove(remove);

            // Update json data string
            SaveData(zaalData);
        }
        public static void EditData(ZaalModel data) //aanpas functie
        {
            List<ZaalModel> zaalData = LoadData();

            var toEdit = zaalData.Where(a => a.ZaalId == data.ZaalId).ToList();

            if (toEdit.Count() == 1){
                foreach(var x in toEdit){
                    x.Omschrijving = data.Omschrijving;
                    x.Status = data.Status;
                    x.Scherm = data.Scherm;
                }
            }

            // Update json data string
            SaveData(zaalData);
        }
        public static void SortData() //data sorteer functie
        {
            var zaalData = LoadData();
            var orderedData = zaalData.OrderBy(x => x.Omschrijving);

            var jsondata = JsonConvert.SerializeObject(orderedData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
    }
}
