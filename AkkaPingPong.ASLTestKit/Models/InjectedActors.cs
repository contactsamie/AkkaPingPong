using Akka.Actor;
using System;

namespace AkkaPingPong.ASLTestKit
{
    public class InjectedActors
    {
        public Type ActorType { set; get; }
        public IActorRef ActorRef { set; get; }
    }
}