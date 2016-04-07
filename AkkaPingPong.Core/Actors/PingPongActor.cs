using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core.Messages;
using AkkaPingPong.Core.States;

namespace AkkaPingPong.Core.Actors
{
    public class PingPongActor : StatefullReceiveActor<PingPongActorState>
    {
        private static IActorRef PingPongCoordinatorActorRef { set; get; }

        public PingPongActor(IPingPongActorSelectors pingPongActorSelectors)
        {
            PingPongCoordinatorActorRef = pingPongActorSelectors.PingCoordinatorActorSelector.Create(Context);

            Receive<PingMessage>(message =>
            {
                SetState(new PingPongActorState() { Message = message});
                PingPongCoordinatorActorRef.Forward(message);
            });

            ReceiveAny(message => Context.System.EventStream.Publish(new UnHandledMessageReceived()));
        }
    }
}