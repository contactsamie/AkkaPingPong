using System;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class MockSetUpMessage
    {
        public MockSetUpMessage(Type owner, Type whenInComing, object respondWith)
        {
            Owner = owner;
            WhenInComing = whenInComing;
            RespondWith = respondWith;
        }

        public Type Owner { private set; get; }
        public Type WhenInComing { private set; get; }
        public object RespondWith { private set; get; }
    }
}