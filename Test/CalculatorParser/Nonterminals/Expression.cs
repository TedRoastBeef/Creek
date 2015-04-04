/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace LinguaDemo.Calculator
{
    public class Expression : CalculatorNonterminal
    {
        #region Fields

        #endregion

        #region Rules

        public static void Rule(Expression result, Expression expression, ExpressionOperator op, Term term)
        {
            result.Value = op.Function(expression.Value, term.Value);
        }

        public static void Rule(Expression result, Term term)
        {
            result.Value = term.Value;
        }

        #endregion

        #region Public Properties

        public int Value { get; protected set; }

        #endregion
    }
}