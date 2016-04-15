using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core.Messages;
using AkkaPingPong.Core.States;
using System;

namespace AkkaPingPong.Core.Actors
{
    public class PingPongActor<TPingCoordinatorActor> : StatefullReceiveActor<PingPongActorState> where TPingCoordinatorActor : ActorBase
    {
        private IActorRef PingPongCoordinatorActorRef { set; get; }

        public PingPongActor()
        {
            PingPongCoordinatorActorRef = Context.CreateActor<TPingCoordinatorActor>();
            Console.WriteLine(typeof(TPingCoordinatorActor).FullName);
            Receive<PingMessage>(message =>
            {
                SetState(new PingPongActorState() { Message = message });
                PingPongCoordinatorActorRef.Forward(message);
                Sender.Tell(new PingMessageCompleted());
            });

            ReceiveAny(message => Sender.Tell(new UnHandledMessageReceived()));
        }
    }
}