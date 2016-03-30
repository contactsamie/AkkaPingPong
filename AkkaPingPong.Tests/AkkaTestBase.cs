using Akka.TestKit;
using Akka.TestKit.NUnit;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Common;
using AkkaPingPong.Core;
using AkkaPingPong.DependencyLib;
using Autofac;
using NUnit.Framework;
using System;

namespace AkkaPingPong.Tests
{
    public abstract class AkkaTestBase : TestKit
    {
        protected TestProbe Subscriber { set; get; }

        [SetUp]
        protected void SetUp()
        {
            ApplicationActorSystem.StartUpActorSystem<PingPongActor, PingPongSubscriber>(DependencyResolver.GetContainer(), Sys, (builder) =>
             {
                 builder.Register<IPingPongService>(b => new FakePingPongService());
             });
            Subscriber = CreateTestProbe();
            Sys.EventStream.Subscribe(Subscriber.Ref, typeof(object));
        }

        [TearDown]
        protected void TearDown()
        {
            ApplicationActorSystem.ShutDownActorSystem();
        }

        public void JustWait(int durationMilliseconds = 600000)
        {
            var now = DateTime.Now;
            var counter = 0;
            while ((DateTime.Now - now).TotalMilliseconds < durationMilliseconds)
            {
                System.Threading.Thread.Sleep(10 * counter);
                counter++;
            }
        }
    }
}