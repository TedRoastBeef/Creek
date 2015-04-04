using System;

namespace BehaviorTest
{
    class Program 
    {
        
        static void Main()
        {
            var evT = new EventTest();
            evT.AddEventListener<EventTest, EventArgs>("OnChange", changed);
            evT.Change();

            var con = new ConvertTest();
            var a = con.ArrayTest(new object[] {10,9,5,6,7,3,8,2,4,1});
            var p = con.ParseTest("12,3").GetType().Name;
            var tp = con.TryParseTest("23.07.1997");

            Console.WriteLine(a);
            Console.WriteLine(p);
            Console.WriteLine(tp);

            Console.ReadLine();
        }

        static void changed(EventTest sender, EventArgs e)
        {
            Console.WriteLine("changed");
        }
    }
}
