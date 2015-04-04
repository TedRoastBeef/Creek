using System;
using Creek.Database.Api.Triggers;

namespace Creek.Database.Triggers
{
    public interface ITriggersEngine
    {
        void AddUpdateTriggerFor(Type type, UpdateTrigger trigger);
        void AddInsertTriggerFor(Type type, InsertTrigger trigger);
        void AddDeleteTriggerFor(Type type, DeleteTrigger trigger);
        void AddSelectTriggerFor(Type type, SelectTrigger trigger);
    }
}