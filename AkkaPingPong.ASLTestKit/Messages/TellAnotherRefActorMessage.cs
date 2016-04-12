using Akka.Actor;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class TellAnotherRefActorMessage
    {
        public TellAnotherRefActorMessage(IActorRef actorRef)
        {
            ActorRef = actorRef;
        }

        public IActorRef ActorRef { private set; get; }
    }
}