namespace Creek.MVC
{
    using System.Windows.Forms;

    public abstract class IModel<TView, TModel>
        where TView : IView, new() where TModel : IModel<TView, TModel>, new()
    {
        private IController<TView, TModel> Controller;

        public void setController();
    }
}
