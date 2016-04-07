using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core.Messages;
using System;

namespace AkkaPingPong.Core.Actors
{
    public class PingCoordinatorActor : StatefullReceiveActor, IWithUnboundedStash
    {
        private IActorRef PingActorRef { set; get; }
        private IActorRef PingBlockingActorRef { set; get; }
        public DateTime StartTime { get; set; }
        private ICancelable UnStashSchedule { set; get; }
        private ILoggingAdapter _logger = Context.GetLogger();

        public PingCoordinatorActor(IPingPongActorSelectors pingPongActorSelectors)
        {
            _logger.Debug(GetType().FullName + " Running ...");

            PingActorRef = pingPongActorSelectors.PingActorSelector.Create(Context, new ActorSetUpOptions() { RouterConfig = new RoundRobinPool(5, new DefaultResizer(1, 10)) });

            PingBlockingActorRef = pingPongActorSelectors.PingBlockingActorSelector.Create(Context, new ActorSetUpOptions() { RouterConfig = new RoundRobinPool(5, new DefaultResizer(1, 10)) });

            StartTime = DateTime.Now;

            Become(Initializing);
        }

        public void Initializing()
        {
            _logger.Debug(GetType().FullName + " becoming initializing ...");
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
                _logger.Debug(GetType().FullName + " stashing ...");
                Stash.Stash();
            });
        }

        public void Processing()
        {
            _logger.Debug(GetType().FullName + " becoming processing ...");
            Receive<PingMessage>((message) =>
            {
                Console.WriteLine("About to ping ...");
                PingActorRef.Tell(message);
                PingBlockingActorRef.Tell(message);
            });
            Receive<IAllPongMessage>((message) =>
            {
                Console.WriteLine("I got a pong ...");
                Context.System.EventStream.Publish(message);
            });

            ReceiveAny(message => Context.System.EventStream.Publish(new UnHandledMessageReceived()));
        }

        protected override void PreStart()
        {
            _logger.Debug(GetType().FullName + " prestart schedule check setup ...");
            UnStashSchedule = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), Self, new ProcessStashedMessage(), Self);
        }

        protected override void PostStop()
        {
            UnStashSchedule.CancelIfNotNull();
        }

        public IStash Stash { get; set; }
    }
}