using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Bioscoop.Models;
namespace Bioscoop.Repository
{
    class Reserveringdata
    {
        public static string reserveringPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Filmschema.json";

        public static string KeyGenerator()
        {
            string res = "";
            Random rnd = new Random();
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            for(int i = 0; i < 6; i++)
            {
                int n = rnd.Next(0, alpha.Length);
                res += alpha[n];
            }
            return res;
        }
        public static List<ReservatieModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = reserveringPath;
            string _json = File.ReadAllText(jsonFilePath);

            List<ReservatieModel> res = JsonConvert.DeserializeObject<List<ReservatieModel>>(_json);
            return res;
        }
    }
}
