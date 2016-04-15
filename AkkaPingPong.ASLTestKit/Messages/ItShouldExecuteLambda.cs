using Akka.Actor;
using AkkaPingPong.ASLTestKit.Models;
using System;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class ItShouldExecuteLambda
    {
        public ItShouldExecuteLambda(Action<IUntypedActorContext, Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>> operation)
        {
            Operation = operation;
        }

        public Action<IUntypedActorContext, Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>> Operation { private set; get; }
    }
}