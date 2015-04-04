using System;
using Creek.MVC.Configuration.Tasks;
using Creek.MVC.Tasks;
using MVCSharp.Examples.Basics.Model;

namespace MVCSharp.Examples.Basics.ApplicationLogic
{
    public class MainTask : TaskBase
    {
        [InteractionPoint(typeof(CustomersController), true)]
        public const string Customers = "Customers";

        [InteractionPoint(typeof(OrdersController), true)]
        public const string Orders = "Orders";

        private Customer currentCustomer = Customer.AllCustomers[0];

        public event EventHandler CurrentCustomerChanged;

        public Customer CurrentCustomer
        {
            get { return currentCustomer; }
            set
            {
                currentCustomer = value;
                if (CurrentCustomerChanged != null)
                    CurrentCustomerChanged(this, EventArgs.Empty);
            }
        }

        public override void OnStart(object param)
        {
            Navigator.NavigateDirectly(Customers);
        }
    }
}
