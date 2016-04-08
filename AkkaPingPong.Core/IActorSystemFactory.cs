using Akka.Actor;
using Autofac;
using System;

namespace AkkaPingPong.Core
{
    public interface IActorSystemFactory
    {
        ActorSystem ActorSystem { get; }

        void Register(IContainer container, Action<ContainerBuilder> postBuildOperation = null, ActorSystem actorSystem = null);

        void ShutDownActorSystem();
    }
}