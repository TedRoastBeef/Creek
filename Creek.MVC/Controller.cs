namespace Creek.MVC
{
    public class IController<TView,TModel>
        where TView : IView, new() where TModel : IModel, new()
    {
        public TView View { get; set; }

        public TModel Model { get; set; }

        public IController()
        {
            View = new TView();
            Model = new TModel();
        }
    }
}
