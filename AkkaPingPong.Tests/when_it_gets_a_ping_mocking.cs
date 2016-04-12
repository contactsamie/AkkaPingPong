using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit;
using AkkaPingPong.ASLTestKit.Mocks;
using AkkaPingPong.Core.Actors;
using AkkaPingPong.Core.Messages;
using AkkaPingPong.DependencyLib;
using NUnit.Framework;
using System;

namespace AkkaPingPong.Tests
{
    [TestFixture]
    public class when_it_gets_a_ping_mocking : AkkaTestBase.AkkaTestBase
    {

        [Test]
        [ExpectedException]
        public void it_should_do_a_pong()
        {
            //Arrange
            ActorSystem.CreateActor<PingPongActor<MockActor>>();

            //Act
            ActorSystem.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(5));
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
        }
        [Test]
        public void it_should_do_a_pong1()
        {
            //Arrange
            var test = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            test.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();

            ActorSystem.CreateActor<PingPongActor<MockActor>>();

            //Act
            ActorSystem.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(5));
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));

            AwaitAssert(() => test.ExpectMockActorToReceiveMessage<PingMessage>(typeof(MockActor), ActorSystem.LocateActor(typeof(PingPongActor<>))), TimeSpan.FromSeconds(5));
        }

        [Test]
        public void it_should_do_a_pong2()
        {
            //Arrange
            var test = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            var pingActor = test.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor1>();
            var pingPongActor = test.WhenActorReceives<PingMessage>().ItShouldTellAnotherActor(pingActor).CreateMockActor<MockActor2>();
            var pingPongActorSelection = ActorSystem.LocateActor(pingPongActor);

            //Act
            pingPongActorSelection.Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
        }

        [Test]
        public void it_should_do_a_pong_integration()
        {
            //Arrange
            var test = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            var pingActor = test.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor1>();
            var pingPongActor = test.WhenActorReceives<PingMessage>().ItShouldTellAnotherActor(pingActor).CreateMockActor<MockActor2>();
            var pingPongActorSelection = ActorSystem.LocateActor(pingPongActor);

          
            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                pingPongActorSelection.Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
        }

        [Test]
        public void it_should_do_a_pong_unit1()
        {
            //Arrange
            ActorSystem.CreateActor<PingPongActor<MockActor>>();

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
            ActorSystem.CreateActor<PingCoordinatorActor<MockActor1, MockActor2>>();

            //Act
            ActorSystem.LocateActor(typeof(PingCoordinatorActor<,>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<SorryImStashing>(), TimeSpan.FromSeconds(20));
        }
    }
}