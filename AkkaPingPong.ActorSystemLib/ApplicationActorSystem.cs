using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AkkaPingPong.ActorSystemLib
{
    public class ApplicationActorSystem
    {
        public static IContainer Container { set; get; }

        public static ActorSystem ActorSystem { get; set; }

        public static TIActorsSelectors Register<TActorsSelectors, TIActorsSelectors>(IContainer container, Action<ContainerBuilder> postBuildOperation = null, ActorSystem actorSystem = null) where TActorsSelectors : IApplicationActorSelectors, new() where TIActorsSelectors : IApplicationActorSelectors
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            ActorSystem = actorSystem ?? ActorSystem.Create(typeof(ApplicationActorSystem).Name);
            var builder = new ContainerBuilder();
            builder.Register(x => (TActorsSelectors)new TActorsSelectors().SetUpActors(ActorSystem)).As<TIActorsSelectors>().SingleInstance();
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).Where(t => typeof(ReceiveActor).IsAssignableFrom(t));
            postBuildOperation?.Invoke(builder);
            builder.Update(container);
            IDependencyResolver resolver = new AutoFacDependencyResolver(container, ActorSystem);
            Container = container;
            return container.Resolve<TIActorsSelectors>();
        }

        public static void ShutDownActorSystem()
        {
            Task.WaitAll(ActorSystem != null ? ActorSystem.Terminate() : Task.FromResult(true));
        }
    }
}