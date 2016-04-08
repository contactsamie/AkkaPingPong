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

            ActorName = GetActorNameByType(actorName, Actortype);

            ActorMetaData = ActorMetaDataByName(ActorName, parentActorMetaData);

            return this;
        }

        public static string GetActorNameByType(string actorName, Type actorType)
        {
            if (!typeof(ActorBase).IsAssignableFrom(actorType))
            {
                throw new Exception("Object supplied is not an actor");
            }
            return string.IsNullOrEmpty(actorName) ? actorType.Name.Split('`')[0] : actorName;
        }

        public static ActorMetaData ActorMetaDataByName(string actorName, ActorMetaData parentActorMetaData = null)
        {
            return new ActorMetaData(actorName, parentActorMetaData);
        }

        public static ActorSelection Select(Type type, ActorMetaData parentActorMetaData, ActorSystem actorSystem)
        {
            var metaData = ActorMetaDataByName(GetActorNameByType(null, type), parentActorMetaData);
            return actorSystem.ActorSelection(metaData.Path);
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