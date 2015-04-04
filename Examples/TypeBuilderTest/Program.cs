using System;
using System.IO;
using Lib.Tools.TypeBuilder;

namespace TypeBuilderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = null;
            string line = null;
            StreamReader streamReader = null;

            var m =
              new FluentMethodBuilder(typeof(void)).
              AddParameter(() => fileName).
              AddLocal(() => streamReader).
              AddLocal(() => line).
              Body.
                Using(() => streamReader, () => new StreamReader(fileName)).
                  Loop().
                    Assign(() => line, () => streamReader.ReadLine()).
                    If(() => line == null).
                      Break().
                    EndIf().
                    If(() => line.Length > 0).
                      Do(() => Console.WriteLine(line)).
                    EndIf().
                  EndLoop().
                EndUsing().
              EndBody().
              Compile<Action<string>>();

        }
    }
}
