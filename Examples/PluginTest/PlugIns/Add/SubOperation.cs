using CalculatorPlugInLib;
using Creek.Extensibility.Plugins;

namespace Add
{
    [PlugIn("Subtraction")]
    public class SubOperation : PlugIn<ICalculatorApplication>, ICalculatorOperationPlugIn
    {
        public string OperationSign
        {
            get { return "-"; }
        }

        public double DoOperation(double number1, double number2)
        {
            return number1 - number2;
        }
    }
}
