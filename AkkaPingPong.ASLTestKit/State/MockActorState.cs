using AkkaPingPong.ASLTestKit.Messages;
using System.Collections.Generic;

namespace AkkaPingPong.ASLTestKit.State
{
    public class MockActorState : IMockActorState
    {
        public List<MockSetUpMessage> MockSetUpMessages { set; get; }
    }
}