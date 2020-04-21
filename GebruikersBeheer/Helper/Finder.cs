using System;
using System.IO;
using System.Reflection;
/// <summary>
/// Deze file heeft alleen maar 1 class:
/// Finder =>
/// Deze class heeft 1 functie die als doel heeft om door een
/// specifieke file de path (locatie) van deze te vinden
/// </summary>
public class Finder
{
    public string SearchFile(string file)
    {
        ///Zoek naar de path van een specifieke document
        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //vraag de naam op van de solution
        string name = Assembly.GetCallingAssembly().GetName().Name;
        //index opzoeken naar de laatste voorkoming van de naam
        int last = path.LastIndexOf(name);
        //de path bijwerken om zo in project map te komen
        path = path.Substring(0, last);

        ///loop door alle mappen en zoek naar de file
        try
        {
            foreach(string i in Directory.GetDirectories(path))
            {
                foreach(string j in Directory.GetDirectories(i))
                {
                    foreach(string k in Directory.GetFiles(j))
                    {
                        if(k.Contains(file))
                        {
                            return k;
                        }
                    }
                }
            }
        }catch(Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }
}