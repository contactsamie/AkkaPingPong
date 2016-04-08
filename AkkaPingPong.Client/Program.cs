using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core;
using AkkaPingPong.Core.Actors;
using AkkaPingPong.Core.Messages;
using AkkaPingPong.DependencyLib;
using Autofac;
using System;

namespace AkkaPingPong.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = DependencyResolver.GetContainer();
            var actorSystemfactory = DependencyResolver.GetContainer().Resolve<IActorSystemFactory>();
            actorSystemfactory.Register(container);

            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                actorSystemfactory.ActorSystem.LocateActor<PingPongActor<PingCoordinatorActor<PingActor, PingBlockingActor>>>().Tell(new PingMessage());
            }
            Console.ReadLine();
        }
    }
}