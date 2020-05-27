using System;
using System.Collections.Generic;
using Bioscoop.Helpers;
using Bioscoop.Models;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Bioscoop.Repository
{
    class ReserveringData
    {
        public static string jsonPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Reservering.json";

        public static List<ReserveringModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = jsonPath;
            string _json = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<List<ReserveringModel>>(_json);
        }
        private static void SaveData(List<ReserveringModel> reserveringData) //opslaan en schrijven naar json functie
        {
            var jsondata = JsonConvert.SerializeObject(reserveringData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
        public static int getId() //hoogste id opzoek functie
        {
            var reserveringData = LoadData();
            int reservatieId = reserveringData.Max(r => r.ReserveringId) + 1;

            return reservatieId;
        }
        public static void AddData(ReserveringModel data) //toevoeg functie
        {
            List<ReserveringModel> reserveringData = LoadData();
            data.ReserveringId = getId();
            data.Code = CodeGenerator();
            reserveringData.Add(data);

            // Update json data string
            SaveData(reserveringData);
        }
        public static void RemoveData(ReserveringModel data) //verwijder functie
        {
            List<ReserveringModel> reserveringData = LoadData();

            var toRemove = reserveringData.Where(a => a.ReserveringId == data.ReserveringId).ToList();
            foreach (var remove in toRemove) reserveringData.Remove(remove);

            // Update json data string
            SaveData(reserveringData);
        }
        public static string CodeGenerator()
        {
            string res = "";
            Random rnd = new Random();
            List<ReserveringModel> reserveringen = LoadData();
            List<string> codes = new List<string>();

            foreach (ReserveringModel r in reserveringen)
            {
                codes.Add(r.Code);
            }
            char[] alpha = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

            for (int i = 0; i < 5; i++)
            {
                int n = rnd.Next(0, alpha.Length);
                res += alpha[n];
            }
            while (codes.Contains(res))
            {
                res = CodeGenerator();
            }
            return res;
        }

        public static void SortData() //data sorteer functie
        {
            var reserveringData = LoadData();
            var orderedData = reserveringData.OrderBy(x => x.ProgrammaId);

            var jsondata = JsonConvert.SerializeObject(orderedData, Formatting.Indented);
            System.IO.File.WriteAllText(jsonPath, jsondata);
        }
    }
}
