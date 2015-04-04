using System;
using System.Collections.Generic;
using System.Text;

namespace MVCSharp.Examples.AdvancedCustomization.ApplicationLogic
{
    public interface IMailView
    {
        string RecipientAddress { get; }

        string SenderAddress { get; set; }
    }
}
