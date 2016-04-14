using AkkaPingPong.ASLTestKit.Messages;
using System.Collections.Generic;

namespace AkkaPingPong.ASLTestKit.State
{
    public interface IMockActorState
    {
        List<MockSetUpMessage> MockSetUpMessages { set; get; }
    }
}