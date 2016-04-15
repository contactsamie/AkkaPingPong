using System;

namespace AkkaPingPong.ASLTestKit.Messages
{
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
}