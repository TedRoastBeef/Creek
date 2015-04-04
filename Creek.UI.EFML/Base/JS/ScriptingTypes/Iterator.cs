using System.Collections;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public abstract class Aggregate<T>
    {
        public abstract Iterator CreateIterator();
    }

    public abstract class Iterator
    {
        public abstract object First();
        public abstract object Next();
        public abstract object Last();
        public abstract bool IsDone();
        public abstract object CurrentItem();
    }

    public class DefaultAggregate : Aggregate<object>
    {
        private readonly ArrayList items = new ArrayList();

        // Property 
        public int Count
        {
            get { return items.Count; }
        }

        // Indexer 
        public object this[int index]
        {
            get { return items[index]; }
            set { items.Insert(index, value); }
        }

        public override Iterator CreateIterator()
        {
            return new DefaultIterator(this);
        }
    }

    public class DefaultIterator : Iterator
    {
        private readonly DefaultAggregate aggregate;
        private int current;

        // Constructor 
        public DefaultIterator(DefaultAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        public override object First()
        {
            return aggregate[0];
        }

        public override object Next()
        {
            object ret = null;
            if (current < aggregate.Count - 1)
            {
                ret = aggregate[++current];
            }

            return ret;
        }

        public override object Last()
        {
            return aggregate[aggregate.Count - 1];
        }

        public override object CurrentItem()
        {
            return aggregate[current];
        }

        public override bool IsDone()
        {
            return current >= aggregate.Count;
        }
    }
}