using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Test
    {
        static void Main(string[] args)
        {
            List<List<string>> locations = new List<List<string>>();
            List<string> location = new List<string>();
            location.Add("A0");
            locations.Add(location);

            WHO who = new WHO(locations);
            //who.testTotal();
            who.WHOActionLogic();


        }
    }
}
