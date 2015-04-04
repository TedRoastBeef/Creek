using Creek.MVC;
using MVCSharp.Examples.Basics.Model;

namespace MVCSharp.Examples.Basics.ApplicationLogic
{
    using Creek.MVP;

    public class CustomersController : ControllerBase<MainTask, ICustomersView>
    {
        public override ICustomersView View
        {
            get { return base.View; }
            set
            {
                base.View = value;
                View.SetCustomersList(Customer.AllCustomers);
                View.CurrentCustomer = Task.CurrentCustomer;
            }
        }

        public void ShowOrders()
        {
            Task.Navigator.Navigate(MainTask.Orders);
        }

        public void CurrentCustomerChanged()
        {
            Task.CurrentCustomer = View.CurrentCustomer;
        }
    }
}
