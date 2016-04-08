using Akka.Actor;
using Akka.DI.Core;
using System;

namespace AkkaPingPong.ActorSystemLib
{
    public class SelectableActor : ISelectableActor
    {
        public string ActorName { private set; get; }

        public Type Actortype { private set; get; }

        public ISelectableActor SetUp<T>(ActorSystem system, string actorName = null, ActorMetaData parentActorMetaData = null) where T : ActorBase
        {
            Actortype = typeof(T);

            ActorName = string.IsNullOrEmpty(actorName) ? Actortype.Name.Split('`')[0] : actorName;

            ActorMetaData = new ActorMetaData(ActorName, parentActorMetaData);

            return this;
        }

        public ActorMetaData ActorMetaData { get; set; }

        public ActorSelection Select(IActorContext context = null)
        {
            return context == null ?
                ApplicationActorSystem.ActorSystem.ActorSelection(ActorMetaData.Path) :
                context.ActorSelection(ActorMetaData.Path);
        }

        public IActorRef Create(ActorSystem system, ActorSetUpOptions options = null)
        {
            var props = system.DI().Props(Actortype);

            props = PrepareProps(options, props);

            return system.ActorOf(props, name: ActorName);
        }

        public IActorRef Create(IActorContext actorContext, ActorSetUpOptions options = null)
        {
            var props = actorContext.DI().Props(Actortype);

            props = PrepareProps(options, props);

            return actorContext.ActorOf(props, name: ActorName);
        }

        private static Props PrepareProps(ActorSetUpOptions options, Props props)
        {
            if (options != null)
            {
                if (options.RouterConfig != null)
                {
                    props = props.WithRouter(options.RouterConfig);
                }
                if (options.SupervisoryStrategy != null)
                {
                    props = props.WithSupervisorStrategy(options.SupervisoryStrategy);
                }
                if (options.Dispatcher != null)
                {
                    props = props.WithDispatcher(options.Dispatcher);
                }
                if (options.MailBox != null)
                {
                    props = props.WithMailbox(options.MailBox);
                }
            }
            return props;
        }
    }
}