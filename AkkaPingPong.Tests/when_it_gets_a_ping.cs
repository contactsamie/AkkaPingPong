using Akka.TestKit.TestActors;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core.Actors;
using AkkaPingPong.Core.Messages;
using NUnit.Framework;
using System;

namespace AkkaPingPong.Tests
{
    public class when_it_gets_a_ping : AkkaTestBase
    {
        [Test]
        public void it_should_do_a_pong_integration()
        {
            ActorSystemfactory.ActorSystem.CreateActor<PingPongActor<PingCoordinatorActor
                    <PingActor, PingBlockingActor>>>();

            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                ActorSystemfactory.ActorSystem.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());
            }
            JustWait(5000);
            //Assert
            AwaitAssert(() => Subscriber.ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(20));
        }

        [Test]
        public void it_should_do_a_pong_unit1()
        {
            ActorSystemfactory.ActorSystem.CreateActor<PingPongActor<BlackHoleActor>>();
            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);

                ActorSystemfactory.ActorSystem.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(20));
        }

        [Test]
        public void it_should_do_a_pong_unit2()
        {
            //Act
            ActorSystemfactory.ActorSystem.CreateActor<PingCoordinatorActor<BlackHoleActor1, BlackHoleActor2>>();

            ActorSystemfactory.ActorSystem.LocateActor(typeof(PingCoordinatorActor<,>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<SorryImStashing>(), TimeSpan.FromSeconds(20));
        }
    }
}