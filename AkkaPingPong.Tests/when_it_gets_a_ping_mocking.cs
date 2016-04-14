using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit;
using AkkaPingPong.Core.Actors;
using AkkaPingPong.Core.Messages;
using AkkaPingPong.DependencyLib;
using NUnit.Framework;
using System;
using Akka.Actor;
using Akka.TestKit.NUnit;
using AkkaPingPong.ASLTestKit.Mocks;
using Autofac;

namespace AkkaPingPong.Tests
{
    [TestFixture]
    public class when_it_gets_a_ping_mocking :TestKit
    {

        [Test]
        [ExpectedException]
        public void it_should_do_a_pong()
        {
            //Arrange
          
            var mockFactory = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);

            mockFactory.CreateActor<PingPongActor<MockActor>>();

            //Act
            mockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

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

            test.CreateActor<PingPongActor<MockActor>>();

            //Act
            test.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(5));
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
 
            AwaitAssert(() => test.ExpectMockActor(typeof(MockActor)).WhoseParentIs(test.LocateActor(typeof(PingPongActor<>))) .ToHaveReceivedMessage<PingMessage>(), TimeSpan.FromSeconds(5));
        }

        [Test]
        public void it_should_do_a_pong2()
        {
            //Arrange
            var test = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            var pingActor = test.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor1>();
            var pingPongActor = test.WhenActorReceives<PingMessage>().ItShouldForwardItTo(pingActor,new PingMessage()).CreateMockActor<MockActor2>();
            var pingPongActorSelection = test.LocateActor(pingPongActor);

            //Act
            pingPongActorSelection.Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromMinutes(1));
        }

        [Test]
        public void it_should_do_a_pong_integration()
        {
            //Arrange
            var test = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            var pingActor = test.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor1>();
            var pingPongActor = test.WhenActorReceives<PingMessage>().ItShouldForwardItTo(pingActor, new PingMessage()).CreateMockActor<MockActor2>();
            var pingPongActorSelection = test.LocateActor(pingPongActor);


            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                pingPongActorSelection.Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromMinutes(1));
        }

        [Test]
        public void it_should_do_a_pong_unit1()
        {
            //Arrange
            var test = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);

            test.CreateActor<PingPongActor<MockActor>>();

            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);

                test.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(20));
        }

        [Test]
        public void it_should_do_a_pong_unit2()
        {
            //Arrange
            var test = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            test.CreateActor<PingCoordinatorActor<MockActor1, MockActor2>>();

            //Act
            test.LocateActor(typeof(PingCoordinatorActor<,>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<SorryImStashing>(), TimeSpan.FromSeconds(20));
        }
    }
}