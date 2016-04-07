using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core;
using AkkaPingPong.Core.Messages;
using AkkaPingPong.DependencyLib;
using System;

namespace AkkaPingPong.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = DependencyResolver.GetContainer();
            var selectors = ApplicationActorSystem.Register<PingPongActorSelectors, IPingPongActorSelectors>(container);

            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                selectors.PingPongActorSelector.Select().Tell(new PingMessage());
            }
            Console.ReadLine();
        }
    }
}