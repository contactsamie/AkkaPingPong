using Akka.Actor;
using Akka.DI.Core;
using AkkaPingPong.Core.Actors;
using AkkaPingPong.Core.Messages;

namespace AkkaPingPong.Core
{
    public class PingPongActor : ReceiveActor
    {
        private static IActorRef PingPongCoordinatorActorRef { set; get; }

        public PingPongActor()
        {
            PingPongCoordinatorActorRef = Context.ActorOf(Context.System.DI().Props<PingCoordinatorActor>());
            Receive<PingMessage>(message => PingPongCoordinatorActorRef.Forward(message));

            ReceiveAny(message => Context.System.EventStream.Publish(new UnHandledMessageReceived()));
        }
    }
}