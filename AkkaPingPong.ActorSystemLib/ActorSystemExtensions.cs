using Akka.Actor;

namespace AkkaPingPong.ActorSystemLib
{
    public static class ActorSystemExtensions
    {
        //todo maybe cache actor selection

        internal static ISelectableActor CreateActorSelector<T>(this ActorSystem actorSystem, ActorMetaData parentActorMetaData = null, string user = null) where T : ActorBase
        {
            return new SelectableActor().SetUp<T>(actorSystem, null, parentActorMetaData);
        }

        /// <summary>
        /// Selects an existing action i.e an actor that has already been created
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actorSystem"></param>
        /// <param name="context"></param>
        /// <param name="parentActorMetaData"></param>
        /// <returns></returns>
        public static ActorSelection LocateActor<T>(this ActorSystem actorSystem, ActorMetaData parentActorMetaData = null, string user = null) where T : ActorBase
        {
            return actorSystem.CreateActorSelector<T>(parentActorMetaData, user).Select();
        }

        public static string GetActorName<T>(this ActorSystem actorSystem) where T : ActorBase
        {
            return actorSystem.CreateActorSelector<T>().ActorName;
        }

        public static IActorRef CreateActor<T>(this ActorSystem actorSystem, ActorSetUpOptions option = null, ActorMetaData parentActorMetaData = null) where T : ActorBase
        {
            return actorSystem.CreateActorSelector<T>(parentActorMetaData).Create(actorSystem, option);
        }

        public static IActorRef CreateActor<T>(this IActorContext actorContext, ActorSetUpOptions option = null, ActorMetaData parentActorMetaData = null) where T : ActorBase
        {
            return actorContext.System.CreateActorSelector<T>(parentActorMetaData).Create(actorContext, option);
        }
    }
}