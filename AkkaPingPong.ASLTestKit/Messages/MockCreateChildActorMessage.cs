using AkkaPingPong.ActorSystemLib;
using System;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class MockCreateChildActorMessage
    {
        public MockCreateChildActorMessage(Type childActorType, ActorSetUpOptions options)
        {
            ChildActorType = childActorType;
            Options = options;
        }

        public ActorSetUpOptions Options { get; private set; }
        public Type ChildActorType { private set; get; }
    }
}