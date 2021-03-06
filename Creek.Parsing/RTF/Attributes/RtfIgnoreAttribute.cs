﻿using System;

namespace Creek.Parsing.RTF.Attributes
{
    /// <summary>
    /// Specifies that RtfWriter must ignore a member of a class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    internal class RtfIgnoreAttribute : Attribute
    {
    }
}