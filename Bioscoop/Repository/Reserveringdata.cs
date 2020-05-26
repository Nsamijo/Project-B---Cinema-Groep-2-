using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Bioscoop.Models;
using System.Linq;
namespace Bioscoop.Repository
{
    class Reserveringdata
    {
        public static string reserveringPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Filmschema.json";

        public static string KeyGenerator()
        {
            string res = "";
            Random rnd = new Random();
            List<ReserveringModel> reserveringen = LoadData();
            List<string> codes = new List<string>();
            
            foreach(ReserveringModel r in reserveringen)
            {
                codes.Add(r.Code);
            }
            char[] alpha = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            
            for(int i = 0; i < 5; i++)
            {
                int n = rnd.Next(0, alpha.Length);
                res += alpha[n];
            }
            while(codes.Contains(res))
            {
                res = KeyGenerator();
            }
            return res;
        }
        public static List<ReserveringModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = reserveringPath;
            string _json = File.ReadAllText(jsonFilePath);

            List<ReserveringModel> res = JsonConvert.DeserializeObject<List<ReserveringModel>>(_json);
            return res;
        }
        public static void SaveData(List<ReserveringModel> lis)
        {
            var jsondata = JsonConvert.SerializeObject(lis, Formatting.Indented);
            System.IO.File.WriteAllText(reserveringPath, jsondata);
        }
        public static ReserveringModel VindReservering(string code)
        {
            foreach(ReserveringModel r in LoadData())
            {
                if(r.Code == code)
                {
                    return r;
                }
            }
            return null;
        }
    }
}
