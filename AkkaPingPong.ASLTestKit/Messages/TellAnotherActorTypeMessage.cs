using AkkaPingPong.ActorSystemLib;
using System;

namespace AkkaPingPong.ASLTestKit.Messages
{
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
}