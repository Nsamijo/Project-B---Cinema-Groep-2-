using System;
using System.IO;
using System.Reflection;

public class Finder
{
    public string SearchFile(string file)
    {
        ///Zoek naar de path van een specifieke document
        string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        string name = Assembly.GetCallingAssembly().GetName().Name;
        int last = path.LastIndexOf(name);
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