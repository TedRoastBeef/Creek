using System;
using System.CodeDom.Compiler;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Creek.Dynamics
{
    //ToDo: continue
    public class TypeGen
    {
        private static string typename;
        public dynamic Instance;
        public StringBuilder src;

        public static TypeGen Create(string typname, string inherit = "object")
        {
            var ret = new TypeGen {src = new StringBuilder()};
            typename = typname;
            ret.src.AppendLine("imports System");
            ret.src.AppendLine("public class " + typname + " : inherits " + inherit);

            return ret;
        }

        public void Property(string name, Type typ)
        {
            src.AppendLine("public property " + name + " as " + typ.Name);
        }

        public void Property(string name, Type typ, string value)
        {
            src.AppendLine("public property " + name + " as " + typ.Name + " = " + value);
        }

        public Method Function<typ>(string name, params string[] arguments)
        {
            src.Append("\rpublic function " + name + "(");
            foreach (string a in arguments)
            {
                src.Append(a.Split(':')[0] + " as " + a.Split(':')[1]);
            }
            src.Append(") as " + typeof (typ).Name);
            var returns = new Method {src = src, methodtyp = 0};

            return returns;
        }

        public Method Sub<typ>(string name, params string[] arguments)
        {
            src.Append("\rpublic sub " + name + "(");
            foreach (string a in arguments)
            {
                src.Append(a.Split(':')[0] + " as " + a.Split(':')[1]);
            }
            src.Append(") as " + typeof (typ).Name);
            var returns = new Method {src = src, methodtyp = 1};

            return returns;
        }

        public dynamic Get()
        {
            src.AppendLine("end class");

            var cc = new VBCodeProvider();
            var cp = new CompilerParameters {GenerateInMemory = true};

            cp.ReferencedAssemblies.AddRange(new[] {"System.dll", "System.Windows.Forms.dll"});
            CompilerResults res = cc.CompileAssemblyFromSource(cp, src.ToString());

            if (res.Errors.HasErrors)
            {
                foreach (CompilerError er in res.Errors)
                {
                    MessageBox.Show(er.ErrorText + " on line " + er.Line);
                }
            }

            Instance = res.CompiledAssembly.CreateInstance(typename);
            return Instance;
        }

        #region Nested type: Method

        public class Method
        {
            internal int methodtyp = 0;
            public StringBuilder src;

            public void End()
            {
                src.AppendLine(methodtyp == 0 ? "end function" : "end sub");
            }

            public void Return(string stmt)
            {
                src.AppendLine(methodtyp == 0 ? "\rreturn " + stmt : "");
            }

            public void SetVar(string name, string value)
            {
                src.AppendLine("\r" + name + " = " + value);
            }

            public void Var<t>(string name, object value)
            {
                src.AppendLine("\rDim " + name + " as " + typeof (t).Name + " = " + value);
            }

            public void Call<T1>(string n, params object[] args)
            {
                src.AppendLine("\r" + typeof (T1).Namespace + "." + typeof (T1).Name + "." + n +
                               "(");
                foreach (object o in args)
                {
                    if (args.Length != 1)
                        src.Append(o + ",");
                    else
                    {
                        src.Append(o);
                    }
                }
                src.Append(")\r");
            }
        }

        #endregion
    }
}