using System;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.Tools
{
    public class Functions
    {
        public static string InputBox(string title, string promptText, string value)
        {
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] {label, textBox, buttonOk, buttonCancel});
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            form.ShowDialog();

            return textBox.Text;
        }

        public static dynamic CreateObject(string id)
        {
            return Activator.CreateInstance(Type.GetTypeFromProgID(id));
        }

        public static dynamic trycatch(Func<object> func)
        {
            var ob = new ObjectBuilder();
            bool has = false;
            try
            {
                ob.Add("Result", func());
                ob.Add("Func", func);
            }
            catch (Exception exception)
            {
                has = true;
                ob.Add("Exception", exception.Message);
            }
            ob.Add("hasException", has);

            return ob.Build();
        }

        #region Nested type: TryCatch

        public class TryCatch
        {
            public Exception Exception;
            public Action Func;
            public bool HasException;
        }

        #endregion
    }
}