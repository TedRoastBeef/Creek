using System.Collections;

namespace Creek.UI.PropertyGridEx
{
    public class CustomPropertyCollectionSet : CollectionBase
    {
        public virtual CustomPropertyCollection this[int index]
        {
            get { return ((CustomPropertyCollection) base.List[index]); }
            set { base.List[index] = value; }
        }

        public virtual int Add(CustomPropertyCollection value)
        {
            return base.List.Add(value);
        }

        public virtual int Add()
        {
            return base.List.Add(new CustomPropertyCollection());
        }

        public virtual object ToArray()
        {
            var list = new ArrayList();
            list.AddRange(base.List);
            return list.ToArray(typeof (CustomPropertyCollection));
        }
    }
}