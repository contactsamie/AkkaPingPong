using Akka.Actor;
using System;

namespace AkkaPingPong.ActorSystemLib
{
    public interface ISelectableActor
    {
        string ActorName { get; }

        Type Actortype { get; }

        ActorSelection Select(IActorContext context = null);

        ActorMetaData ActorMetaData { set; get; }

        ISelectableActor SetUp<T>(ActorSystem system, string actorName, ActorMetaData parentActorMetaData = null) where T : ActorBase;

        IActorRef Create(ActorSystem system, ActorSetUpOptions options = null);

        IActorRef Create(IActorContext actorContext, ActorSetUpOptions options = null);
    }
}