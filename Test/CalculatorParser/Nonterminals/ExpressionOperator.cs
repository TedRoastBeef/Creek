/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace LinguaDemo.Calculator
{
    public class ExpressionOperator : CalculatorNonterminal
    {
        #region Fields

        #endregion

        #region Rules

        public static void Rule(ExpressionOperator result, OperatorAddition op)
        {
            result.Function = (lhs, rhs) => lhs + rhs;
        }

        public static void Rule(ExpressionOperator result, OperatorSubtraction op)
        {
            result.Function = (lhs, rhs) => lhs - rhs;
        }

        #endregion

        #region Public Properties

        public Func<int, int, int> Function { get; private set; }

        #endregion
    }
}