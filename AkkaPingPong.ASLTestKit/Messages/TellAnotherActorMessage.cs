using AkkaPingPong.ActorSystemLib;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class TellAnotherActorMessage
    {
        public TellAnotherActorMessage(ActorMetaData actor, object message)
        {
            Actor = actor;
            Message = message;
        }

        public ActorMetaData Actor { private set; get; }

        public object Message { get; private set; }
    }
}