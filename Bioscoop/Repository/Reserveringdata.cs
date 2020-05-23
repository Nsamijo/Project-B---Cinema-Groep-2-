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
            List<ReservatieModel> reserveringen = LoadData();
            List<string> codes = new List<string>();
            
            foreach(ReservatieModel r in reserveringen)
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
        public static List<ReservatieModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = reserveringPath;
            string _json = File.ReadAllText(jsonFilePath);

            List<ReservatieModel> res = JsonConvert.DeserializeObject<List<ReservatieModel>>(_json);
            return res;
        }
        public static void SaveData(List<ReservatieModel> lis)
        {
            var jsondata = JsonConvert.SerializeObject(lis, Formatting.Indented);
            System.IO.File.WriteAllText(reserveringPath, jsondata);
        }
        public static ReservatieModel VindReservering(string code)
        {
            foreach(ReservatieModel r in LoadData())
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
