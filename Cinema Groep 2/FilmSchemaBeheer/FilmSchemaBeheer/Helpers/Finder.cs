using System;
using System.IO;
using System.Reflection;

public class Finder 
{
    //Functie van Nathan, zoekt een bepaalde file met een naam in dezelfde directory
    public string SearchFile(string file)
    {
        string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        string name = Assembly.GetCallingAssembly().GetName().Name;
        int last = path.LastIndexOf(name);
        path = path.Substring(0, last);

        try
        {
            foreach(string i in Directory.GetDirectories(path))
            {
                foreach(string j in Directory.GetDirectories(i))
                {
                    foreach(string k in Directory.GetFiles(j))
                    {
                        if (k.Contains(file))
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
