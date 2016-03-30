using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Common;
using AkkaPingPong.Core;
using AkkaPingPong.Core.Messages;
using AkkaPingPong.DependencyLib;
using Autofac;

namespace AkkaPingPong.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationActorSystem.StartUpActorSystem<PingPongActor, PingPongSubscriber>(DependencyResolver.GetContainer());

            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                ApplicationActorSystem.AppActorRef.Tell(new PingMessage());
            }
            Console.ReadLine();
        }
    }
}
