namespace Creek.Text
{
    /// <summary>
    /// 	The 'Context' class
    /// </summary>
    public class Context
    {
        // Constructor

        #region Constructors and Destructors

        public Context(string input)

        {
            this.Input = input;
        }

        #endregion

        // Gets or sets input

        #region Public Properties

        public string Input { get; set; }

        // Gets or sets output

        public int Output { get; set; }

        #endregion
    }

    /// <summary>
    /// 	The 'AbstractExpression' class
    /// </summary>
    public abstract class RomanExpression
    {
        #region Public Methods and Operators

        public abstract string Five();

        public abstract string Four();

        public void Interpret(Context context)
        {
            if (context.Input.Length == 0)
            {
                return;
            }

            if (context.Input.StartsWith(this.Nine()))

            {
                context.Output += (9 * this.Multiplier());

                context.Input = context.Input.Substring(2);
            }

            else if (context.Input.StartsWith(this.Four()))

            {
                context.Output += (4 * this.Multiplier());

                context.Input = context.Input.Substring(2);
            }

            else if (context.Input.StartsWith(this.Five()))

            {
                context.Output += (5 * this.Multiplier());

                context.Input = context.Input.Substring(1);
            }

            while (context.Input.StartsWith(this.One()))

            {
                context.Output += (1 * this.Multiplier());

                context.Input = context.Input.Substring(1);
            }
        }

        public abstract int Multiplier();

        public abstract string Nine();

        public abstract string One();

        #endregion
    }

    /// <summary>
    /// 	A 'TerminalExpression' class
    /// 	<remarks>
    /// 		Thousand checks for the Roman Numeral M
    /// 	</remarks>
    /// </summary>
    public class ThousandExpression : RomanExpression
    {
        #region Public Methods and Operators

        public override string Five()
        {
            return " ";
        }

        public override string Four()
        {
            return " ";
        }

        public override int Multiplier()
        {
            return 1000;
        }

        public override string Nine()
        {
            return " ";
        }

        public override string One()
        {
            return "M";
        }

        #endregion
    }

    /// <summary>
    /// 	A 'TerminalExpression' class
    /// 	<remarks>
    /// 		Hundred checks C, CD, D or CM
    /// 	</remarks>
    /// </summary>
    public class HundredExpression : RomanExpression
    {
        #region Public Methods and Operators

        public override string Five()
        {
            return "D";
        }

        public override string Four()
        {
            return "CD";
        }

        public override int Multiplier()
        {
            return 100;
        }

        public override string Nine()
        {
            return "CM";
        }

        public override string One()
        {
            return "C";
        }

        #endregion
    }

    /// <summary>
    /// 	A 'TerminalExpression' class
    /// 	<remarks>
    /// 		Ten checks for X, XL, L and XC
    /// 	</remarks>
    /// </summary>
    public class TenExpression : RomanExpression
    {
        #region Public Methods and Operators

        public override string Five()
        {
            return "L";
        }

        public override string Four()
        {
            return "XL";
        }

        public override int Multiplier()
        {
            return 10;
        }

        public override string Nine()
        {
            return "XC";
        }

        public override string One()
        {
            return "X";
        }

        #endregion
    }

    /// <summary>
    /// 	A 'TerminalExpression' class
    /// 	<remarks>
    /// 		One checks for I, II, III, IV, V, VI, VI, VII, VIII, IX
    /// 	</remarks>
    /// </summary>
    public class OneExpression : RomanExpression
    {
        #region Public Methods and Operators

        public override string Five()
        {
            return "V";
        }

        public override string Four()
        {
            return "IV";
        }

        public override int Multiplier()
        {
            return 1;
        }

        public override string Nine()
        {
            return "IX";
        }

        public override string One()
        {
            return "I";
        }

        #endregion
    }
}