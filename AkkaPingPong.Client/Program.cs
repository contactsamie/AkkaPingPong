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
            var container =new  DependencyResolver().GetContainer();
            var actorSystemfactory = container.Resolve<IActorSystemFactory>();
            actorSystemfactory.Register(container);

            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                actorSystemfactory.ActorSystem.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());
            }
            Console.ReadLine();
        }
    }
}