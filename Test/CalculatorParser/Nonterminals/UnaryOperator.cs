/* Copyright (c) 2009 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace LinguaDemo.Calculator
{
    public class UnaryOperator : CalculatorNonterminal
    {
        #region Fields

        #endregion

        #region Rules

        public static void Rule(UnaryOperator result, OperatorSubtraction op)
        {
            result.Function = (value) => -value;
        }

        #endregion

        #region Public Properties

        public Func<int, int> Function { get; private set; }

        #endregion
    }
}