using GenericEventRunner.ForEntities;
using System;

namespace HBD.EfCore.DDD.Attributes
{
    /// <summary>
    /// This only available when using I
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class EventAttribute : Attribute
    {
        #region Constructors

        public EventAttribute(Type eventType)
        {
            if (!typeof(IDomainEvent).IsAssignableFrom(eventType))
                throw new ArgumentException("The Event Type must be an IDomainEvent.");

            EventType = eventType;
        }

        #endregion Constructors

        #region Properties

        public EventToSend EventToSend { get; set; } = EventToSend.BeforeAndAfterSave;

        public Type EventType { get; }

        #endregion Properties
    }
}