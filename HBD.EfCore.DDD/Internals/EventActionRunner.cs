using AutoMapper;
using GenericEventRunner.ForEntities;
using HBD.Actions.Runner;
using HBD.Actions.Runner.Internals;
using HBD.EfCore.DDD.Attributes;
using HBD.EfCore.DDD.Domains;
using System.Linq;

namespace HBD.EfCore.DDD.Internals
{
    public class EventActionRunner : ActionRunnerService
    {
        #region Fields

        private readonly IMapper autoMapper;

        #endregion Fields

        #region Constructors

        public EventActionRunner(IMapper autoMapper, IMethodMatchProvider methodMatchProvider) : base(methodMatchProvider)
        {
            this.autoMapper = autoMapper;
        }

        #endregion Constructors

        #region Methods

        protected override void AfterRunMethod(MethodMatchInfo methodInfo, object dto, object entity, object result)
        {
            if (methodInfo is null)
                throw new System.ArgumentNullException(nameof(methodInfo));

            base.AfterRunMethod(methodInfo, dto, entity, result);

            if (entity is AggregateRoot aggregateRoot)
            {
                var rs = result ?? entity;
                if (rs == null) return;

                var atts = methodInfo.Method.GetCustomAttributes(false).OfType<EventAttribute>();

                foreach (var att in atts)
                {
                    var @event = autoMapper.Map(rs, rs.GetType(), att.EventType) as IDomainEvent;
                    aggregateRoot.AddEvent(@event, att.EventToSend);
                }
            }
        }

        #endregion Methods
    }
}