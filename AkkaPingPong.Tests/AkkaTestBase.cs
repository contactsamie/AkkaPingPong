//using Akka.TestKit;
//using Akka.TestKit.NUnit;
//using Akka.TestKit.TestActors;
//using AkkaPingPong.Common;
//using AkkaPingPong.Core;
//using AkkaPingPong.DependencyLib;
//using Autofac;
//using NUnit.Framework;
//using System;
//using Akka.Actor;

//namespace AkkaPingPong.Tests
//{
//    public abstract class AkkaTestBase : TestKit
//    {
//        public class BlackHoleActor1 : BlackHoleActor { }

//        public class BlackHoleActor2 : BlackHoleActor { }

//        public class BlackHoleActor3 : BlackHoleActor { }

//        public class BlackHoleActor4 : BlackHoleActor { }

//        public class BlackHoleActor5 : BlackHoleActor { }

//        protected TestProbe Subscriber { set; get; }

//        protected IActorSystemFactory ActorSystemfactory { set; get; }

//        protected ActorSystem ActorSystem { set; get; }

//        [SetUp]
//        public void SetUp()
//        {
//            var preBuilder = new ContainerBuilder();
//            preBuilder.Register(x => new FakeActorSystemFactory()).As<IActorSystemFactory>().SingleInstance();
//            preBuilder.Update(DependencyResolver.GetContainer());

//            ActorSystemfactory = DependencyResolver.GetContainer().Resolve<IActorSystemFactory>();

//            ActorSystemfactory.Register(DependencyResolver.GetContainer(), (builder) =>
//            {
//                builder.Register<IPingPongService>(b => new FakePingPongService());
//            }, Sys);

//            //just to reduce typing
//            ActorSystem = ActorSystemfactory.ActorSystem;

//            Subscriber = CreateTestProbe();
//            Sys.EventStream.Subscribe(Subscriber.Ref, typeof(object));
//        }

//        [TearDown]
//        protected void TearDown()
//        {
//            ActorSystemfactory.ShutDownActorSystem();
//        }

//        public void JustWait(int durationMilliseconds = 600000)
//        {
//            var now = DateTime.Now;
//            var counter = 0;
//            while ((DateTime.Now - now).TotalMilliseconds < durationMilliseconds)
//            {
//                System.Threading.Thread.Sleep(10 * counter);
//                counter++;
//            }
//        }
//    }
//}