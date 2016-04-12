using System;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class ItTellAnotherActorMessage
    {
        public ItTellAnotherActorMessage(Type actor)
        {
            Actor = actor;
        }

        public Type Actor { private set; get; }
    }
}