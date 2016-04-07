using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core.Actors;

namespace AkkaPingPong.Core
{
    public class PingPongActorSelectors : IPingPongActorSelectors
    {
        public ISelectableActor PingActorSelector { set; get; }
        public ISelectableActor PingBlockingActorSelector { set; get; }
        public ISelectableActor PingPongActorSelector { get; set; }
        public ISelectableActor PingCoordinatorActorSelector { get; set; }

        public IApplicationActorSelectors SetUpActors(ActorSystem system)
        {
            PingActorSelector = new SelectableActor().SetUp<PingActor>(system, typeof(PingActor).Name);

            PingBlockingActorSelector = new SelectableActor().SetUp<PingBlockingActor>(system, typeof(PingBlockingActor).Name);

            PingPongActorSelector = new SelectableActor().SetUp<PingPongActor>(system, typeof(PingPongActor).Name);

            PingCoordinatorActorSelector = new SelectableActor().SetUp<PingCoordinatorActor>(system, typeof(PingCoordinatorActor).Name);

            Initialize(system);
            return this;
        }

        public void Initialize(ActorSystem system)
        {
            PingPongActorSelector.Create(system);
        }
    }
}