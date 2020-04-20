using System;
using System.Collections.Generic;
using System.Text;

public class Checker
{
    
    public bool TijdSyntax(string s)
    {
        char[] arr = s.ToCharArray();
        if(arr.Length == 5) {
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
        } else
        {
            return false;
        }
        return true;
    }

    public bool DatumSyntax(string s)
    {
        char[] arr = s.ToCharArray();
        if(arr.Length == 10)
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

}