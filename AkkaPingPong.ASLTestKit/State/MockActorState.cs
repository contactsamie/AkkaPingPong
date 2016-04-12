using System.Collections.Generic;
using AkkaPingPong.ASLTestKit.Messages;

namespace AkkaPingPong.ASLTestKit.State
{
    public class MockActorState : IMockActorState
    {
        public List<MockSetUpMessage> MockSetUpMessages { set; get; }
    }
}