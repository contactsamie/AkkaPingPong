using AkkaPingPong.Common;
using AkkaPingPong.Core;
using Autofac;
using System;

namespace AkkaPingPong.DependencyLib
{
    public class DependencyResolver
    {
        private static Autofac.IContainer Container { set; get; }

        public static IContainer GetContainer(Action<ContainerBuilder> builderFunc = null)
        {
            if (Container != null)
            {
                return Container;
            }
            var builder = new ContainerBuilder();
            builder.RegisterType<PingPongService>().As<IPingPongService>();
            builder.Register(x => new ActorSystemFactory()).As<IActorSystemFactory>().SingleInstance();

            builderFunc?.Invoke(builder);

            Container = builder.Build();
            return Container;
        }
    }
}