using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using System;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class TellAnotherRefActorMessage
    {
        public TellAnotherRefActorMessage(IActorRef actorRef, object message)
        {
            ActorRef = actorRef;
            Message = message;
        }

        public object Message { get; private set; }
        public IActorRef ActorRef { private set; get; }
    }

    public class TellAnotherActorMessage
    {
        public TellAnotherActorMessage(ActorMetaData actor, object message)
        {
            Actor = actor;
            Message = message;
        }

        public ActorMetaData Actor { private set; get; }

        public object Message { get; private set; }
    }

    public class TellAnotherActorTypeMessage
    {
        public TellAnotherActorTypeMessage(Type actor, object message, ActorMetaData parent)
        {
            ActorType = actor;
            Message = message;
            Parent = parent;
        }

        public Type ActorType { private set; get; }
        public ActorMetaData Parent { private set; get; }
        public object Message { get; private set; }
    }

    public class ForwardToChildActorTypeMessage
    {
        public ForwardToChildActorTypeMessage(Type childActorType, object message)
        {
            ChildActorType = childActorType;
            Message = message;
        }

        public Type ChildActorType { private set; get; }
        public object Message { get; private set; }
    }

    public class TellChildActorTypeMessage
    {
        public TellChildActorTypeMessage(Type childActorType, object message)
        {
            ChildActorType = childActorType;
            Message = message;
        }

        public Type ChildActorType { private set; get; }
        public object Message { get; private set; }
    }
}