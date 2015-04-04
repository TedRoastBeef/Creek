using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Creek.Scripting.Commandparser.Common;
using Creek.Scripting.Commandparser.TP;
using Creek.Scripting.Commandparser.Types;

namespace Creek.Scripting.Commandparser
{
    public class Interpreter
    {
        public List<Section> Sections { get; set; }
        public List<IPropertyObject> DataTypes;
        public List<Class> Classes;
        internal List<string> ImportedNamespaces;
        internal Dictionary<string, IPropertyObject> Variables { get; set; }
        
        public object GetVariable(string name)
        {
            if(Variables[name] is @array)
            {
                return Variables[name];
            }
            return Variables[name].Value;
        }

        public Interpreter()
        {
            // initialize properties
            Sections = new List<Section>();
            DataTypes = new List<IPropertyObject>();
            Classes = new List<Class>();
            ImportedNamespaces = new List<string>();

            // add standard types
            DataTypes.Add(new @integer());
            DataTypes.Add(new @decimal());
            DataTypes.Add(new @string());
            DataTypes.Add(new @expression());
            DataTypes.Add(new CreateObjectMethod());
            DataTypes.Add(new @newLiteral(DataTypes));

            DataTypes.Add(new @array(DataTypes));

        }

       // [DebuggerStepThrough]
        public void Parse(string s)
        {
            // replace disturbing chars
            string src = s.Replace("\n", "");
            src = src.Replace("\t", "");
            src = Regex.Replace(src, "~.+~", ""); // remove comments

            // transform source
            string[] cmdss = src.Split(Convert.ToChar("{"));
            src = src.Remove(0, cmdss[0].Length+1);
            src = src.Remove(src.Length-2, 2);
            src = src.Replace("}", "\n");

            UseStatementParser.Parse(cmdss[0], this);

            //parsing commands
            foreach (string cm in src.Split(Convert.ToChar("\n")))
            {
                var sh = BlockHeaderParser.Parse(cm.Split(Convert.ToChar("{"))[0]);

                var cmd = new Section { Header = { Name = sh.Name } };

                VarDefParser.Parse(DataTypes, cm, cmd);
                FunctionCallParser.Parse(cm, cmd, this);
                VarSetParser.Parse(DataTypes, cm, cmd);
                ClassParser.Parse(cm, this);

                Sections.Add(cmd);
            }

        }

/*
        private bool Is<t>(object input)
        {
            return ReferenceEquals(input.GetType(), typeof(t));
        }
*/
    }
}
