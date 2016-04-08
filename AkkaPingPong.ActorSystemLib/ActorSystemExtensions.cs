using System;
using System.Linq;
using Akka.Actor;

namespace AkkaPingPong.ActorSystemLib
{
    public static class ActorSystemExtensions
    {
        //todo maybe cache actor selection

        internal static ISelectableActor CreateActorSelector<T>(this ActorSystem actorSystem, ActorMetaData parentActorMetaData = null) where T : ActorBase
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
        public static ActorSelection LocateActor<T>(this ActorSystem actorSystem, ActorMetaData parentActorMetaData = null) where T : ActorBase
        {
            return actorSystem.LocateActor(typeof(T), parentActorMetaData);
        }

        public static ActorSelection LocateActor<T>(this ActorSystem actorSystem, ActorSelection parentActorSelection) where T : ActorBase
        {
            return actorSystem.LocateActor(typeof(T), parentActorSelection);
        }

        public static ActorSelection LocateActor(this ActorSystem actorSystem, Type type, ActorMetaData parentActorMetaData = null)
        {
            return SelectableActor.Select(type, parentActorMetaData, actorSystem);
        }

        public static ActorSelection LocateActor(this ActorSystem actorSystem, Type type, ActorSelection parentActorSelection)
        {
            if (parentActorSelection == null) throw new ArgumentNullException(nameof(parentActorSelection));
            if (string.IsNullOrEmpty(parentActorSelection.PathString))
            {
                throw new Exception("Invalid parent actor path");
            }

            var parentActorMetaData = new ActorMetaData(parentActorSelection.PathString.Split('/').Last());

            return actorSystem.LocateActor(type, parentActorMetaData);
        }

        public static ActorSelection LocateActor<T, TP>(this ActorSystem actorSystem) where T : ActorBase where TP : ActorBase
        {
            var parentActorSelection = actorSystem.LocateActor(typeof(TP));

            if (string.IsNullOrEmpty(parentActorSelection.PathString))
            {
                throw new Exception("Invalid parent actor path");
            }

            var parentActorMetaData = new ActorMetaData(parentActorSelection.PathString.Split('/').Last());

            return actorSystem.CreateActorSelector<T>(parentActorMetaData).Select();
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