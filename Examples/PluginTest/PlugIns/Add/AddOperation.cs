using CalculatorPlugInLib;
using Creek.Extensibility.Plugins;

namespace Add
{
    [PlugIn("Addition")]
    public class AddOperation : PlugIn<ICalculatorApplication>, ICalculatorOperationPlugIn
    {
        public string OperationSign
        {
            get { return "+"; }
        }

        public double DoOperation(double number1, double number2)
        {
            this.Application.ApplicationProxy.ShowMessage("Calced :D");
            return number1 + number2;
        }
    }
}
