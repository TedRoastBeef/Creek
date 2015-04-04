namespace Creek.MVC
{
    using System;

    public interface IView
    {
        event EventHandler ViewChanged;
    }
}
