using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Bioscoop.Models;

namespace Bioscoop.Repository
{
    class FilmschemaData
    {

        public static string filmschemaPath => Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + @"Data\Filmschema.json";

        public static List<FilmschemaModel> LoadData() //ophalen json data als list functie
        {
            string jsonFilePath = filmschemaPath;
            string _json = File.ReadAllText(jsonFilePath);

            List<FilmschemaModel> res = JsonConvert.DeserializeObject<List<FilmschemaModel>>(_json);
            res = Sort(res);
            return res;
        }
        private static void SaveData(List<FilmschemaModel> FilmschemaData) //opslaan en schrijven naar json functie
        {
            var jsondata = JsonConvert.SerializeObject(FilmschemaData, Formatting.Indented);
            System.IO.File.WriteAllText(filmschemaPath, jsondata);
        }
        public static void MaakProgramma(int programmaid, string datum, string tijd, int zaalid, int filmid)
        {
            List<FilmschemaModel> filmSchema = LoadData();
            filmSchema.Add(new FilmschemaModel(programmaid, datum, tijd, filmid, zaalid));
            SaveData(filmSchema);
        }
        public static void VerwijderProgramma(int index)
        {
            List<FilmschemaModel> filmschema = LoadData();
            filmschema.RemoveAt(index);
            SaveData(filmschema);
        }
        public static int getId() //hoogste id opzoek functie
        {
            var filmschemaData = LoadData();
            int zaalId = 0;
            if (filmschemaData.Count != 0)
            {
                zaalId = filmschemaData.Max(r => r.ProgrammaId) + 1;
            }

            return zaalId;
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
        public static bool TimeCollides(string datum, int zaalid, string tijd)
        {
            List<FilmschemaModel> filmschema = LoadData();
            foreach (FilmschemaModel programma in filmschema)
            {
                if (programma.Datum == datum && programma.ZaalId == zaalid && programma.Tijd == tijd)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool HallCollides(string datum, int zaalid)
        {
            List<FilmschemaModel> filmschema = LoadData();
            string[] tijden = new string[4] { "10:00", "13:30", "17:00", "20:30" };
            foreach (FilmschemaModel programma in filmschema)
            {
                foreach (string tijd in tijden)
                {
                    if (TimeCollides(datum, zaalid, tijd) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool DateCollides(string datum)
        {
            List<FilmschemaModel> filmschema = LoadData();
            List<ZaalModel> zalen = ZaalData.LoadData();
            foreach (FilmschemaModel programma in filmschema)
            {
                foreach (ZaalModel zaal in zalen)
                {
                    if (HallCollides(datum, zaal.ZaalId) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static string CompareDates(string d1, string d2)
        {
            string[] d1sarr = d1.Split("/");
            string[] d2sarr = d2.Split("/");
            int[] d1iarr = new int[3];
            int[] d2iarr = new int[3];
            for (int i = 0; i < 2; i++)
            {
                if (d1sarr[i].ToCharArray()[0] == '0')
                {
                    d1iarr[i] = (int)Char.GetNumericValue(d1sarr[i].ToCharArray()[1]);
                }
                else
                {
                    d1iarr[i] = Int32.Parse(d1sarr[i]);
                }
                if (d2sarr[i].ToCharArray()[0] == '0')
                {
                    d2iarr[i] = (int)Char.GetNumericValue(d2sarr[i].ToCharArray()[1]);
                }
                else
                {
                    d2iarr[i] = Int32.Parse(d2sarr[i]);
                }
            }
            d1iarr[2] = Int32.Parse(d1sarr[2]);
            d2iarr[2] = Int32.Parse(d2sarr[2]);

            for (int i = 2; i >= 0; i--)
            {
                if (d1iarr[i] > d2iarr[i])
                {
                    return d1;
                }
                if (d1iarr[i] < d2iarr[i])
                {
                    return d2;
                }
            }
            return "";
        }
        public static string CompareTimes(string t1, string t2)
        {
            string[] t1sarr = t1.Split(":");
            string[] t2sarr = t2.Split(":");
            int[] t1iarr = new int[2];
            int[] t2iarr = new int[2];
            for (int i = 0; i < 2; i++)
            {
                if (t1sarr[i].ToCharArray()[0] == '0')
                {
                    t1iarr[i] = (int)Char.GetNumericValue(t1sarr[i].ToCharArray()[1]);
                }
                else
                {
                    t1iarr[i] = Int32.Parse(t1sarr[i]);
                }
                if (t2sarr[i].ToCharArray()[0] == '0')
                {
                    t1iarr[i] = (int)Char.GetNumericValue(t1sarr[i].ToCharArray()[1]);
                }
                else
                {
                    t2iarr[i] = Int32.Parse(t2sarr[i]);
                }
            }

            for (int i = 0; i < 2; i++)
            {
                if (t1iarr[i] > t2iarr[i])
                {
                    return t1;
                }
                if (t1iarr[i] < t2iarr[i])
                {
                    return t2;
                }
            }
            return "";


        }
        public static List<FilmschemaModel> Sort(List<FilmschemaModel> arr)
        {
            List<FilmschemaModel> res = new List<FilmschemaModel>();

            foreach (FilmschemaModel f in arr)
            {
                if (res.Count > 0)
                {
                    for (int i = 0; i <= res.Count; i++)
                    {
                        if (i != res.Count)
                        {
                            if (CompareDates(f.Datum, res[i].Datum) == res[i].Datum)
                            {
                                res.Insert(i, f);
                                break;
                            }
                            else if (CompareDates(f.Datum, res[i].Datum) == "")
                            {
                                if (CompareTimes(f.Tijd, res[i].Tijd) == res[i].Tijd)
                                {
                                    res.Insert(i, f);
                                    break;
                                }
                                else if (CompareTimes(f.Tijd, res[i].Tijd) == "")
                                {
                                    res.Insert(i, f);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            res.Add(f);
                            break;
                        }
                    }
                }
                else
                    res.Add(f);
            }
            return res;
        }
    }
}
