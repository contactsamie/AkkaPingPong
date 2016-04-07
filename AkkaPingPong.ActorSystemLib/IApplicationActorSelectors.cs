using Akka.Actor;

namespace AkkaPingPong.ActorSystemLib
{
    public interface IApplicationActorSelectors
    {
        IApplicationActorSelectors SetUpActors(ActorSystem system);

        void Initialize(ActorSystem system);
    }
}