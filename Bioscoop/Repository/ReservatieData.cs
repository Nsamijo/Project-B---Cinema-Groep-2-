using System;
using System.Collections.Generic;
using Bioscoop.Helpers;
using Bioscoop.Models;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Bioscoop.Repository
{
    class ReservatieData
    {
        public static string jsonPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Reservatie.json";

        public static List<ReservatieModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = jsonPath;
            string _json = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<List<ReservatieModel>>(_json);
        }
        private static void SaveData(List<ReservatieModel> reservatieData) //opslaan en schrijven naar json functie
        {
            var jsondata = JsonConvert.SerializeObject(reservatieData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
        public static int getId() //hoogste id opzoek functie
        {
            var reservatieData = LoadData();
            int reservatieId = reservatieData.Max(r => r.ReservatieId) + 1;

            return reservatieId;
        }
        public static void AddData(ReservatieModel data) //toevoeg functie
        {
            List<ReservatieModel> reservatieData = LoadData();
            data.ReservatieId = getId();
            reservatieData.Add(data);

            // Update json data string
            SaveData(reservatieData);
        }
        public static void RemoveData(ReservatieModel data) //verwijder functie
        {
            List<ReservatieModel> reservatieData = LoadData();

            var toRemove = reservatieData.Where(a => a.ReservatieId == data.ReservatieId).ToList();
            foreach (var remove in toRemove) reservatieData.Remove(remove);

            // Update json data string
            SaveData(reservatieData);
        }
        public static void EditData(ReservatieModel data) //aanpas functie
        {
            List<ReservatieModel> reservatieData = LoadData();

            var toEdit = reservatieData.Where(a => a.ReservatieId == data.ReservatieId).ToList();

            if (toEdit.Count() == 1)
            {
                foreach (var x in toEdit)
                {
                    //x.Omschrijving = data.Omschrijving;
                    //x.Status = data.Status;
                    //x.Scherm = data.Scherm;
                }
            }

            // Update json data string
            SaveData(reservatieData);
        }
        public static void SortData() //data sorteer functie
        {
            //var reservatieData = LoadData();
            //var orderedData = reservatieData.OrderBy(x => x.Omschrijving);

            //var jsondata = JsonConvert.SerializeObject(orderedData, Formatting.Indented);
            //System.IO.File.WriteAllText(jsonPath, jsondata);
        }
    }
}
