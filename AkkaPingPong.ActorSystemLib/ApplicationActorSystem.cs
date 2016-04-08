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
        public static ActorSystem ActorSystem { get; set; }

        public static void Register(IContainer container, Action<ContainerBuilder> postBuildOperation = null, ActorSystem actorSystem = null)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            ActorSystem = actorSystem ?? ActorSystem.Create(typeof(ApplicationActorSystem).Name);
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).Where(t => typeof(ReceiveActor).IsAssignableFrom(t));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes().Where(type => type.IsAssignableTo<ActorBase>()))
                {
                    if (type.IsGenericType)
                    {
                        builder.RegisterGeneric(type);
                    }
                    else
                    {
                        builder.RegisterType(type);
                    }
                }
            }

            postBuildOperation?.Invoke(builder);
            builder.Update(container);
            IDependencyResolver resolver = new AutoFacDependencyResolver(container, ActorSystem);
        }

        public static void ShutDownActorSystem()
        {
            Task.WaitAll(ActorSystem != null ? ActorSystem.Terminate() : Task.FromResult(true));
        }
    }
}