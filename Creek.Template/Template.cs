using System.Collections.Generic;
using System.Text;

namespace Creek.Template
{
    /// <summary>
    /// This class implements a Django-like simple templating system for creating clean dynamic html or other text
    /// without using asp.net server controls or string builders that require html and other text to be written
    /// and escaped in .cs files.
    /// 
    /// The template is very high performance, since it indexes a template once (lazy loading the first time)
    /// and uses a string builder insert after that to keep memory use and copy time to a minimum.
    /// 
    /// Example text file:
    /// "Hey {{ Name }}, what's up?"
    /// Example Template.ToString() output after calling Set("Name", "Craig"); :
    /// "Hey Craig, what's up?"
    /// </summary>
    public class Template
    {
        private readonly Dictionary<string, TemplateValue> Variables;
        public string FilePath;
        public TemplateFrame Frame;

        /// <summary>
        /// Passing debug = true in ensures template modifications are noticed.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="debug"></param>
        public Template(string filePath, bool debug)
        {
            FilePath = filePath;
            if (Frame == null && findPreviouslyBuiltFrame() == false)
            {
                Frame = new TemplateFrame(filePath, debug);
            }
            Variables = new Dictionary<string, TemplateValue>();
        }

        private bool findPreviouslyBuiltFrame()
        {
            foreach (TemplateFrame htf in TemplateFrames.List)
            {
                if (htf.FilePath == FilePath)
                {
                    Frame = htf;
                    return true;
                }
            }
            return false;
        }

        public void Set(string name, string value)
        {
            if (value == null)
            {
                value = "";
            }

            if (Variables.ContainsKey(name))
            {
                Variables[name].Value = value;
            }
            else
            {
                Variables.Add(name, new TemplateValue(value, Frame.Variables[name]));
            }
        }

        private int[] CopyIndicies(TemplateFrameVariable tfv)
        {
            var indexArry = new int[tfv.Indicies.Count];
            tfv.Indicies.CopyTo(indexArry);
            return indexArry;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder(Frame.Text);

            foreach (TemplateValue tv in Variables.Values)
            {
                // Copy indicies so that the global TemplateFrame stays unchanged when values are inserted and indicies need to be shifted. 
                tv.Indicies = CopyIndicies(tv.FrameVar);
            }
            foreach (TemplateValue tv in Variables.Values)
            {
                for (int j = 0; j < tv.Indicies.Length; j++)
                {
                    stringBuilder.Insert(tv.Indicies[j], tv.Value);
                    UpdateIndicies(tv.FrameVar.Positions[j], tv.Value.Length);
                }
            }
            return stringBuilder.ToString();
        }

        private void UpdateIndicies(int position, int length)
        {
            foreach (TemplateValue tv in Variables.Values)
            {
                for (int i = 0; i < tv.FrameVar.Positions.Count; i++)
                {
                    if (tv.FrameVar.Positions[i] > position)
                    {
                        tv.Indicies[i] = tv.Indicies[i] + length;
                    }
                }
            }
        }
    }

    public class TemplateValue
    {
        public readonly TemplateFrameVariable FrameVar;
        public int[] Indicies;
        public string Value;

        public TemplateValue(string value, TemplateFrameVariable fv)
        {
            Value = value;
            FrameVar = fv;
        }
    }
}