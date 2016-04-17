using System;
using Akka.Actor;
using AkkaPingPong.ASLTestKit.Mocks;
using AkkaPingPong.ASLTestKit.Models;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class ActorAccess
    {
        public ActorAccess(IUntypedActorContext context, Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors> actorChildren, MockActorBase actorItSelf, IStash stash, Action<Receive> become )
        {
            Context = context;
            ActorItSelf = actorItSelf;
            ActorChildren = actorChildren;
            Stash = stash;
            Become = become;
        }

        public IUntypedActorContext Context { private set; get; }
        public Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors> ActorChildren { private set; get; }
        public MockActorBase ActorItSelf { private set; get; }
        public IStash Stash { private set; get; }
        public Action<Receive> Become { private set; get; }
    }
}