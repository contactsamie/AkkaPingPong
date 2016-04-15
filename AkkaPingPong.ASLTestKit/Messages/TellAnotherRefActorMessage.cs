using Akka.Actor;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class TellAnotherRefActorMessage
    {
        public TellAnotherRefActorMessage(IActorRef actorRef, object message)
        {
            ActorRef = actorRef;
            Message = message;
        }

        public object Message { get; private set; }
        public IActorRef ActorRef { private set; get; }
    }
}