using System;
using System.Windows.Forms;
using Furesoft.Creek.Emit;

namespace EmitExample
{
    class Program
    {
        static void Main(string[] args)
        {
            AssemblyGen ag = new AssemblyGen("hello.exe");
            TypeGen Test = ag.Public.Class("Test");
            {
                CodeGen g = Test.Public.Method(typeof(void), "Main");
                {
                    g.Invoke(typeof(Console), "WriteLine", "Hello World!");
                    g.Invoke(typeof(Console), "ReadLine");
                }
            }

            var ass = ag.GetAssembly();
            var t = ass.GetType("Test");
                
            var main = t.GetMethod("Main").Invoke(Activator.CreateInstance(t), null);
            

           // ag.Save();
        }
    }
}