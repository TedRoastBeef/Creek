using System;
using System.Linq;

namespace Access_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            NorthWindContext context = new NorthWindContext(@"Driver={Microsoft Access Driver (*.mdb, *.accdb)}; DBQ=.\Nwind.mdb");
        
            var titles = context.Employees.Select(x => x.Title).Distinct().OrderBy(y => y);

            foreach (var title in titles)
            {
                Console.WriteLine(title);
            }
        }
    }
}