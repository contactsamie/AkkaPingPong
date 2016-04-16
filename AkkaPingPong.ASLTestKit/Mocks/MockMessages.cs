using System;

namespace AkkaPingPong.ASLTestKit.Mocks
{
    public class MockMessages
    {
        public MockMessages(string actorPath, Type message)
        {
            Message = message;
            ActorPath = actorPath;
        }

        public string ActorPath { private set; get; }

        public Type Message { private set; get; }
    }
}