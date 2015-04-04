namespace Creek.Text
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public delegate String StringFunction(string[] parameters);

    public sealed class StringFormatter
    {
        private readonly Regex m_FuncRegex = new Regex(@"^(?<Name>[A-Z][A-Z0-9]*)(?:\s*'(?<Parameter>[^']*)')*$",
                                                       RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public StringFormatter()
        {
            this.Functions = new Dictionary<String, StringFunction>();
            this.Variables = new Dictionary<String, String>();

            this.Functions.Add("IF", (args) => Boolean.Parse(args[0]) ? args[1] : args[2]);
            this.Functions.Add("ISMATCH", (args) => Regex.IsMatch(args[0], args[1]).ToString());
            this.Functions.Add("REGREPLACE", (args) => Regex.Replace(args[0], args[1], args[2]));
        }

        public Dictionary<String, StringFunction> Functions { get; private set; }

        public Dictionary<String, String> Variables { get; private set; }

        public event UnknownIdentifierEventHandler UnknownFunction;
        public event UnknownIdentifierEventHandler UnknownVariable;

        public String Format(String input)
        {
            return this.Eval(input, false);
        }

        private String Eval(String input, Boolean quote)
        {
            // Is it required to perform formatting?
            if (input.Contains("$"))
            {
                // Get the first index of $-sign
                int index = input.IndexOf('$');

                // Do evaluating while input contains statements
                do
                {
                    int idx = index + 1;

                    // Is this a statement with braces?
                    if (input[idx] == '(')
                    {
                        int num = 1;

                        // Compute length of statement
                        while (idx < input.Length && num != 0)
                        {
                            idx++;

                            if (input[idx] == '(')
                                num++;
                            else if (input[idx] == ')')
                                num--;
                        }

                        idx++;
                    }
                        // Or is it a simple identifier?
                    else
                    {
                        // Compute length of statement
                        while (idx < input.Length && Char.IsLetterOrDigit(input[idx]))
                        {
                            idx++;
                        }
                    }

                    // Compute absolute length
                    int len = idx - index;
                    if (index + len > input.Length)
                    {
                        len = input.Length - index;
                    }

                    // Evaluate statement
                    string str = this.Eval(input.Substring(index, len));
                    // Remove statement text
                    input = input.Remove(index, len);
                    // Insert statement result (if quoting enabled, with single quotes)
                    input = input.Insert(index, quote ? String.Format("'{0}'", str) : str);

                    // Get next index of $-sign
                    index = input.IndexOf('$');
                } while (index != -1);
            }

            // return input
            return input;
        }

        private String Eval(String exp)
        {
            // Is this statement a function call?
            if (exp.StartsWith("$(") && exp.EndsWith(")"))
            {
                // Get function text
                exp = exp.Substring(2);
                exp = exp.Substring(0, exp.Length - 1);

                // Evaluate function
                return this.EvalFunc(exp);
            }
                // Or is it just a variable?
            else if (exp.StartsWith("$"))
            {
                return this.EvalVar(exp.Substring(1));
            }

            throw new InvalidStatementException("The statement '{0}' is not a valid statement!");
        }

        private String EvalFunc(String exp)
        {
            // Evaluate statement, enable quoting (can contain other statements)
            exp = this.Eval(exp, true);

            Match match = this.m_FuncRegex.Match(exp);

            // Is this a valid function call?
            if (match.Success)
            {
                // Get the name of the Function
                string name = match.Groups["Name"].Value;

                // Is the function registered?
                if (this.Functions.ContainsKey(name))
                {
                    // Compute function arguments
                    var args = new List<String>();

                    foreach (Capture cap in match.Groups["Parameter"].Captures)
                    {
                        args.Add(cap.Value);
                    }

                    // Call the function and return the value
                    return this.Functions[name](args.ToArray());
                }
                else
                {
                    var e = new UnknownIdentifierEventArgs
                                {
                                    ErrorBehaviour = ErrorBehaviour.Throw,
                                    Identifier = name,
                                    Replacement = ""
                                };

                    // Raise UnknownFunction event
                    this.OnUnknownFunction(e);

                    switch (e.ErrorBehaviour)
                    {
                        case ErrorBehaviour.Ignore:
                            return "";
                        case ErrorBehaviour.Replace:
                            return e.Replacement;
                        case ErrorBehaviour.Throw:
                        default:
                            throw new UnknownIdentifierException(
                                String.Format("The function '{0}' is not a registered function!", name));
                    }
                }
            }

            throw new InvalidStatementException(String.Format("The statement '{0}' is not a valid function call!", exp));
        }

        private String EvalVar(String exp)
        {
            // Is the variable registered?
            if (this.Variables.ContainsKey(exp.ToLower()))
            {
                // Return variable value
                return this.Variables[exp.ToLower()];
            }

            var e = new UnknownIdentifierEventArgs
                        {
                            ErrorBehaviour = ErrorBehaviour.Throw,
                            Identifier = exp.ToLower(),
                            Replacement = ""
                        };

            // Raise UnknownVariable event
            this.OnUnknownVariable(e);

            switch (e.ErrorBehaviour)
            {
                case ErrorBehaviour.Ignore:
                    return "";
                case ErrorBehaviour.Replace:
                    return e.Replacement;
                case ErrorBehaviour.Throw:
                default:
                    throw new UnknownIdentifierException(
                        String.Format("The statement '{0}' is not a registered variable!", exp));
            }
        }

        private void OnUnknownFunction(UnknownIdentifierEventArgs e)
        {
            if (this.UnknownFunction != null)
            {
                this.UnknownFunction(this, e);
            }
        }

        private void OnUnknownVariable(UnknownIdentifierEventArgs e)
        {
            if (this.UnknownVariable != null)
            {
                this.UnknownVariable(this, e);
            }
        }
    }

    public delegate void UnknownIdentifierEventHandler(StringFormatter sender, UnknownIdentifierEventArgs e);

    public class UnknownIdentifierEventArgs
    {
        public ErrorBehaviour ErrorBehaviour { get; set; }

        public String Replacement { get; set; }

        public String Identifier { get; set; }
    }

    public enum ErrorBehaviour
    {
        Throw,
        Ignore,
        Replace
    }

    [Serializable]
    public class UnknownIdentifierException : Exception
    {
        public UnknownIdentifierException(String text)
            : base(text)
        {
        }
    }

    [Serializable]
    public class InvalidStatementException : Exception
    {
        public InvalidStatementException(String text)
            : base(text)
        {
        }
    }
}