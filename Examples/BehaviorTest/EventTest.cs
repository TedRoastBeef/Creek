using System;
using Creek.Behaviors;

namespace BehaviorTest
{
    class EventTest : EventBehavior
    {
        public Event<EventTest, EventArgs> OnChange;

        public EventTest()
        {
            OnChange = Event<EventTest, EventArgs>.Empty;
        }

        public void Change()
        {
            OnChange.Invoke(this, new EventArgs());
        }

    }
}