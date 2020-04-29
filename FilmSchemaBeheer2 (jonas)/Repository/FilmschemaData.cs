using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Bioscoop.Repository
{
    class FilmschemaData
    {

        // al die functies die in je 10000 models staan hier in zetten los in public statics. Ook de functies die in je helpers folder staan



        public static string filmschemaPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Filmschema.json";

        public static List<FilmschemaModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = filmschemaPath;
            string _json = File.ReadAllText(jsonFilePath);

            return JsonConvert.DeserializeObject<List<FilmschemaModel>>(_json);
        }
        private static void SaveData(List<FilmschemaModel> FilmschemaData) //opslaan en schrijven naar json functie
        {
            var jsondata = JsonConvert.SerializeObject(FilmschemaData, Formatting.Indented);
            System.IO.File.WriteAllText(filmschemaPath, jsondata);
        }

        //Returned een boolean die false is als de string niet volgens de syntax van de tijd is
        //en true als die dat wel is
        public static bool TijdSyntax(string s)
        {
            char[] arr = s.ToCharArray();
            if (arr.Length == 5)
            {
                if (arr[2] == ':')
                {
                    string[] splitted = s.Split(":");
                    try
                    {
                        foreach (string i in splitted)
                        {
                            Int32.Parse(i);
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    if (Int32.Parse(splitted[0]) >= 24 || Int32.Parse(splitted[1]) >= 60)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        //Hetzelfde als TijdSyntax() maar dan voor de datum
        public static bool DatumSyntax(string s)
        {
            char[] arr = s.ToCharArray();
            if (arr.Length == 10)
            {
                if (arr[2] == '/' && arr[5] == '/')
                {
                    string[] splitted = s.Split("/");
                    foreach (string n in splitted)
                    {
                        try
                        {
                            Int32.Parse(n);
                        }
                        catch
                        {
                            return false;
                        }
                    }

                    if (Int32.Parse(splitted[0]) > 31 || Int32.Parse(splitted[1]) > 12 || Int32.Parse(splitted[2]) > 2100)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        
        //Returned een array van strings in syntax "DD/MM/YYYY"
        //van vandaag + start tot en met vandaag+eind   
        public static string[] VolgendeDagen(int start, int eind)
        {
            DateTime vandaag = DateTime.Today;
            DateTime dag = vandaag.AddDays(start);
            string[] res = new string[eind - start];
            for (int i = start; i < eind; i++)
            {
                string dagnul = dag.Day / 10 == 0 ? "0" : "";
                string maandnul = dag.Month / 10 == 0 ? "0" : "";
                res[i - start] = $"{dagnul}{dag.Day}/{maandnul}{dag.Month}/{dag.Year}";
                dag = dag.AddDays(1);

            }
            return res;

        }
        //Returned een string die geprint wordt met de dagen van VolgendeDagen()
        public static string PrintVolgendeDagen(int start, int eind)
        {

            string res = "";
            int i = 1;
            foreach (string dag in VolgendeDagen(start, eind))
            {

                res += $"{i}. {dag}\n";
                i++;
            }
            return res;
        }

        
    }
}
