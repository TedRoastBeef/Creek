namespace Creek.Tools
{
    public abstract class Prototype
    {
        private string id;

        // Constructor 
        public Prototype(string id)
        {
            this.id = id;
        }

        // Property 
        public string Id
        {
            get { return this.id; }
        }

        public abstract Prototype Clone();
    }
}