﻿namespace Creek.IO.Binary
{
    public static class Extensions
    {

        public static T To<T>(this object o)
        {
            return (T) o;
        }

    }
}
