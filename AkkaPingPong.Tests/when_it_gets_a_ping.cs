using Akka.TestKit.TestActors;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core.Actors;
using AkkaPingPong.Core.Messages;
using NUnit.Framework;
using System;

namespace AkkaPingPong.Tests
{
    [TestFixture]
    public class when_it_gets_a_ping : AkkaTestBase.AkkaTestBase
    {
        [Test]
        public void it_should_do_a_pong()
        {
            //Arrange
            ActorSystem.CreateActor<PingPongActor<BlackHoleActor>>();

            //Act
            ActorSystem.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(5));
        }

        [Test]
        public void it_should_do_a_pong_integration()
        {
            //Arrange
            ActorSystem.CreateActor<PingPongActor<PingCoordinatorActor<PingActor, PingBlockingActor>>>();

            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                ActorSystem.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => Subscriber.ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(20));
        }

        [Test]
        public void it_should_do_a_pong_unit1()
        {
            //Arrange
            ActorSystem.CreateActor<PingPongActor<BlackHoleActor>>();

            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);

                ActorSystem.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(20));
        }

        [Test]
        public void it_should_do_a_pong_unit2()
        {
            //Arrange
            ActorSystem.CreateActor<PingCoordinatorActor<BlackHoleActor1, BlackHoleActor2>>();

            //Act
            ActorSystem.LocateActor(typeof(PingCoordinatorActor<,>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<SorryImStashing>(), TimeSpan.FromSeconds(20));
        }
    }
}