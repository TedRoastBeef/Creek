using System;
using System.Linq;
using m = System.Math;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public class Math
    {
        private readonly Random rndm = new Random();

        public double pi
        {
            get { return m.PI; }
        }

        public double random()
        {
            return rndm.NextDouble();
        }

        public double sqrt(double i)
        {
            return m.Sqrt(i);
        }

        public double round(double i)
        {
            return m.Round(i);
        }

        public double log(double d)
        {
            return m.Log(d);
        }

        public double cos(double d)
        {
            return m.Cos(d);
        }

        public double tan(double d)
        {
            return m.Tan(d);
        }

        public double sin(double d)
        {
            return m.Sin(d);
        }

        public double pow(double x, double y)
        {
            return m.Pow(x, y);
        }

        public double max(double first, double second)
        {
            return m.Max(first, second);
        }

        public double min(double first, double second)
        {
            return m.Min(first, second);
        }

        public double average(params double[] o)
        {
            return o.Sum()/o.Length;
        }
    }
}