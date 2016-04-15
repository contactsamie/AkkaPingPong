using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core.Messages;
using System;

namespace AkkaPingPong.Core.Actors
{
    public class PingCoordinatorActor<TPingActor, TPingBlockingActor> : StatefullReceiveActor, IWithUnboundedStash where TPingActor : ActorBase where TPingBlockingActor : ActorBase
    {
        private IActorRef PingActorRef { set; get; }
        private IActorRef PingBlockingActorRef { set; get; }
        public DateTime StartTime { get; set; }
        private ICancelable UnStashSchedule { set; get; }
        private ILoggingAdapter _logger = Context.GetLogger();

        public PingCoordinatorActor()
        {
            _logger.Debug(GetType().FullName + " Running ...");

            PingActorRef = Context.CreateActor<TPingActor>(new ActorSetUpOptions() { RouterConfig = new RoundRobinPool(5, new DefaultResizer(1, 10)) });

            PingBlockingActorRef = Context.CreateActor<TPingBlockingActor>(new ActorSetUpOptions() { RouterConfig = new RoundRobinPool(5, new DefaultResizer(1, 10)) });

            Console.WriteLine(typeof(TPingBlockingActor).FullName);
            Console.WriteLine(typeof(TPingActor).FullName);

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
                Sender.Tell(new SorryImStashing());
            });
        }

        public void Processing()
        {
            _logger.Debug(GetType().FullName + " becoming processing ...");
            Receive<PingMessage>((message) =>
            {
                Console.WriteLine("About to ping ...");
                PingActorRef.Forward(message);
                PingBlockingActorRef.Forward(message);
            });
            Receive<IAllPongMessage>((message) =>
            {
                Console.WriteLine("I got a pong ...");
                Sender.Tell(message);
            });

            ReceiveAny(message => Sender.Tell(new UnHandledMessageReceived()));
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