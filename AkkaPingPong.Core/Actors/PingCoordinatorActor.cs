using Akka.Actor;
using Akka.DI.Core;
using Akka.Routing;
using AkkaPingPong.Core.Messages;
using System;

namespace AkkaPingPong.Core.Actors
{
    public class PingCoordinatorActor : ReceiveActor, IWithUnboundedStash
    {
        private IActorRef PingActorRef { set; get; }
        public DateTime StartTime { get; set; }
        private ICancelable UnStashSchedule { set; get; }

        public PingCoordinatorActor()
        {
            PingActorRef = Context.ActorOf(Context.System.DI().Props<PingActor>().WithRouter(new RoundRobinPool(5, new DefaultResizer(1, 10))));
            StartTime = DateTime.Now;
            Become(Initializing);
        }

        public void Initializing()
        {
            Receive<ProcessStashedMessage>(message =>
            {
                if (!((DateTime.Now - StartTime).TotalSeconds > 5)) return;
                Console.WriteLine("Now ready to process now, Unstashing ....");
                Stash.UnstashAll();
                Become(Processing);
                UnStashSchedule.Cancel();
            });
            ReceiveAny(message =>
            {
                Console.WriteLine("Sorry, staching for now ...");
                Stash.Stash();
            });
        }

        public void Processing()
        {
            Receive<PingMessage>((message) =>
            {
                Console.WriteLine("About to ping ...");
                PingActorRef.Tell(message);
            });
            Receive<PongMessage>((message) =>
            {
                // Context.Stop(PingActorRef);
                Console.WriteLine("I got a pong ...");
                Context.System.EventStream.Publish(message);
            });

            ReceiveAny(message => Context.System.EventStream.Publish(new UnHandledMessageReceived()));
        }

        protected override void PreStart()
        {
            UnStashSchedule = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),TimeSpan.FromSeconds(1), Self, new ProcessStashedMessage(), Self);
        }

        protected override void PostStop()
        {
          
            UnStashSchedule.CancelIfNotNull();
        }

        public IStash Stash { get; set; }
    }
}