using System;
using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core;
using Autofac;

namespace AkkaPingPong.AkkaTestBase
{
    public class FakeActorSystemFactory : IActorSystemFactory

    {
        public void Register(IContainer container,
            Action<ContainerBuilder> postBuildOperation = null, ActorSystem actorSystem = null)
        {
            ApplicationActorSystem.Register(container, postBuildOperation, actorSystem);
        }

        public ActorSystem ActorSystem => ApplicationActorSystem.ActorSystem;

        public void ShutDownActorSystem()
        {
            ApplicationActorSystem.ShutDownActorSystem();
        }
    }
}