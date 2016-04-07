using AkkaPingPong.ActorSystemLib;

namespace AkkaPingPong.Core
{
    public interface IPingPongActorSelectors : IApplicationActorSelectors
    {
        ISelectableActor PingActorSelector { set; get; }

        ISelectableActor PingBlockingActorSelector { set; get; }

        ISelectableActor PingPongActorSelector { set; get; }
        ISelectableActor PingCoordinatorActorSelector { set; get; }
    }
}