using System;
using System.IO;
using System.Reflection;

public class Finder
{
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
                foreach(string j in Directory.GetFiles(i))
                {
                    if (j.Contains(file))
                    {
                        return j;
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
