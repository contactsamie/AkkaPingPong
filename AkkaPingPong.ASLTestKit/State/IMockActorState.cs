using System.Collections.Generic;
using AkkaPingPong.ASLTestKit.Messages;

namespace AkkaPingPong.ASLTestKit.State
{
    public interface IMockActorState
    {
        List<MockSetUpMessage> MockSetUpMessages { set; get; }
    }
}