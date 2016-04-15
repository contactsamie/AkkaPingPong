using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core.Actors;
using Autofac;
using System;

namespace AkkaPingPong.Core
{
    public class ActorSystemFactory : IActorSystemFactory

    {
        public ActorSystemFactory()
        {
            ApplicationActorSystem = new ApplicationActorSystem();
        }

        public ApplicationActorSystem ApplicationActorSystem { set; get; }

        public void Register(IContainer container, Action<ContainerBuilder> postBuildOperation = null, ActorSystem actorSystem = null)
        {
            ApplicationActorSystem.Register(container, postBuildOperation, actorSystem);
            ApplicationActorSystem.ActorSystem.CreateActor<PingPongActor<PingCoordinatorActor<PingActor, PingBlockingActor>>>();
        }

        public ActorSystem ActorSystem => ApplicationActorSystem.ActorSystem;

        public void ShutDownActorSystem()
        {
            ApplicationActorSystem.ShutDownActorSystem();
        }
    }
}