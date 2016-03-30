using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AkkaPingPong.ActorSystemLib
{
    public static class ApplicationActorSystem
    {
        private static IContainer Container { set; get; }

        public static IActorRef AppActorRef { get; set; }

        public static IActorRef AppActorSubscriberRef { get; set; }

        public static ActorSystem ActorSystem { get; set; }

        public static void StartUpActorSystem<T, TSub>(IContainer container, ActorSystem actorSystem = null, Action<ContainerBuilder> postBuildOperation = null) where T : ActorBase where TSub : ActorBase
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).Where(t => typeof(ReceiveActor).IsAssignableFrom(t));
            postBuildOperation?.Invoke(builder);
            builder.Update(container);
            Container = container;
            ActorSystem = actorSystem ?? ActorSystem.Create(typeof(ApplicationActorSystem).Name);
            IDependencyResolver resolver = new AutoFacDependencyResolver(container, ActorSystem);
            AppActorRef = ActorSystem.ActorOf(ActorSystem.DI().Props<T>(), typeof(T).Name);
            AppActorSubscriberRef = ActorSystem.ActorOf(ActorSystem.DI().Props<TSub>(), typeof(TSub).Name);
            ActorSystem.EventStream.Subscribe(AppActorSubscriberRef, typeof(object));
        }
        
        public static void ShutDownActorSystem()
        {
            Task.WaitAll(ActorSystem != null ? ActorSystem.Terminate() : Task.FromResult(true));
        }
    }
}