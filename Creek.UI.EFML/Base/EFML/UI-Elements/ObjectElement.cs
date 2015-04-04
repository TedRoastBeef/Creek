using System;
using Creek.UI.EFML.Base;

namespace Creek.UI.EFML.UI_Elements
{
    public class ObjectElement : UiElement
    {
        public Type Type;

        public ObjectElement()
        {
            Type = GetType();
        }
    }
}