using System;
using System.Collections.Generic;
using System.Text;

public class Checker
{
    public static bool TimeSyntax(string s)
    {
        string[] arr = new[] { s };
        if(arr[2] != ":")
        {
            return false;
        } else
        {
            try{
                foreach(string i in arr)
                {
                    if(i != ":")
                    {
                        Int32.Parse(i);
                    }
                }
            } catch
            {
                return false;
            }
            if(Int32.Parse(arr[0]+arr[1]) >= 24 || Int32.Parse(arr[3]+arr[4]) >= 60)
            {
                return false;
            }
        }
        return true;
    }

}