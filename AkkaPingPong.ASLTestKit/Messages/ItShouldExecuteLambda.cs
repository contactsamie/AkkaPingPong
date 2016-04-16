using Akka.Actor;
using AkkaPingPong.ASLTestKit.Models;
using System;
using AkkaPingPong.ASLTestKit.Mocks;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class ItShouldExecuteLambda
    {
        public ItShouldExecuteLambda(Action<IUntypedActorContext, Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>, MockActorBase> operation)
        {
            Operation = operation;
        }

        public Action<IUntypedActorContext, Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>, MockActorBase> Operation { private set; get; }
    }
}