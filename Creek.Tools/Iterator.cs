using System.Collections;

namespace Creek.Tools
{
    using System.Collections.Generic;

    public abstract class Aggregate<T>
    {
        public abstract Iterator<T> CreateIterator();
    }

    public abstract class Iterator<T>
    {
        public int Step { get; set; }
        public abstract T First();
        public abstract T Next();
        public abstract T Last();
        public abstract bool IsDone();
        public abstract T CurrentItem { get; }
    }

    public class DefaultAggregate<o> : Aggregate<o>
    {
        private readonly List<o> items = new List<o>();

        public DefaultAggregate(o[] o)
        {
            items.AddRange(o);
        }
        public DefaultAggregate()
        {
            
        }

        // Property 
        public int Count
        {
            get { return items.Count; }
        }

        // Indexer 
        public o this[int index]
        {
            get { return items[index]; }
            set { items.Insert(index, value); }
        }

        public override Iterator<o> CreateIterator()
        {
            return new DefaultIterator<o>(this);
        }
    }

    public class DefaultIterator<o> : Iterator<o>
    {
        private readonly DefaultAggregate<o> aggregate;
        private int current;

        // Constructor 
        public DefaultIterator(DefaultAggregate<o> aggregate)
        {
            this.aggregate = aggregate;
            Step = 1;
        }

        public override o First()
        {
            return (o)this.aggregate[0];
        }

        public override o Next()
        {
            o ret = default(o);
            if (current < aggregate.Count - 1)
            {
                current += Step;
                ret = (o)this.aggregate[this.current];
            }

            return ret;
        }

        public override o Last()
        {
            return aggregate[aggregate.Count - 1];
        }

        public override bool IsDone()
        {
            return current >= aggregate.Count;
        }

        public override o CurrentItem
        {
            get
            {
                return aggregate[current];
            }
        }
    }
}