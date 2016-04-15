using Akka.Actor;
using AkkaPingPong.AkkaTestBase;
using AkkaPingPong.ASLTestKit.Mocks;
using AkkaPingPong.Core.Actors;
using AkkaPingPong.Core.Messages;
using System;
using Xunit;

namespace AkkaPingPong.Tests
{
    public class when_it_gets_a_ping_mocking : TestKitTestBase
    {
        //[TearDown]
        //public void TearDown()
        //{
        //    if (mockFactory != null)
        //    {
        //        mockFactory.Dispose();
        //    }
        //}

        [Fact]
        //  [ExpectedException]
        public void it_should_do_a_pong()
        {
            //Arrange

            //mockFactory = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);

            mockFactory.CreateActor<PingPongActor<MockActor>>();

            //Act
            mockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

            try
            {
                //Assert
                AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(5));
                AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
                throw new Exception();
            }
            catch (Exception)
            {
            }
        }

        [Fact]
        public void it_should_do_a_pong1()
        {
            //Arrange
            //mockFactory = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();

            mockFactory.CreateActor<PingPongActor<MockActor>>();

            //Act
            mockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(5));
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));

            AwaitAssert(() => mockFactory.ExpectMockActor(typeof(MockActor)).WhoseParentIs(mockFactory.LocateActor(typeof(PingPongActor<>))).ToHaveReceivedMessage<PingMessage>(), TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void it_should_do_a_pong2()
        {
            //Arrange
            //mockFactory = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            var pingActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor1>();
            var pingPongActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItTo(pingActor, new PingMessage()).CreateMockActor<MockActor2>();
            var pingPongActorSelection = mockFactory.LocateActor(pingPongActor);

            //Act
            pingPongActorSelection.Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromMinutes(1));
        }

        [Fact]
        public void it_should_do_a_pong_integration()
        {
            //Arrange
            //mockFactory = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            var pingActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor1>();
            var pingPongActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItTo(pingActor, new PingMessage()).CreateMockActor<MockActor2>();
            var pingPongActorSelection = mockFactory.LocateActor(pingPongActor);

            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                pingPongActorSelection.Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromMinutes(1));
        }

        [Fact]
        public void it_should_do_a_pong_unit1()
        {
            //Arrange
            //mockFactory = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);

            mockFactory.CreateActor<PingPongActor<MockActor>>();

            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);

                mockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(20));
        }

        [Fact]
        public void it_should_do_a_pong_unit2()
        {
            //Arrange
            //mockFactory = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            mockFactory.CreateActor<PingCoordinatorActor<MockActor1, MockActor2>>();

            //Act
            mockFactory.LocateActor(typeof(PingCoordinatorActor<,>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<SorryImStashing>(), TimeSpan.FromSeconds(20));
        }

        //=====================================
        [Fact]
        public void actor_mocking014()
        {
            //Arrange
            //  mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();
            var pingPongActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldTellAnotherActor(typeof(MockActor), new PingMessage()).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            mockFactory.ExpectMockActor(pingActor).NotToHaveReceivedMessage<PongMessage>();
            mockFactory.ExpectMockActor(pingActor).ToHaveReceivedMessage<PingMessage>();
        }

        [Fact]
        //  [ExpectedException]
        public void actor_mocking013()
        {
            //Arrange

            //  mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();
            var pingPongActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItToChildActor(typeof(MockActor), new PingMessage()).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            mockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).NotToHaveReceivedMessage<PongMessage>();
            Xunit.Assert.Throws<Exception>(() => mockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).ToHaveReceivedMessage<PingMessage>());
        }

        [Fact]
        //  [ExpectedException]
        public void actor_mocking012()
        {
            //Arrange
            //   mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();
            var pingPongActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItToChildActor(typeof(MockActor), new PingMessage()).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            mockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).NotToHaveReceivedMessage<PongMessage>();
            Xunit.Assert.Throws<Exception>(() => mockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).ToHaveReceivedMessage<PingMessage>());
        }

        [Fact]
        // [ExpectedException]
        public void actor_mocking011()
        {
            //Arrange
            //  mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).SetUpMockActor<MockActor>();
            mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();
            var pingPongActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItToChildActor(typeof(MockActor), new PingMessage()).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            mockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).NotToHaveReceivedMessage<PongMessage>();
            Xunit.Assert.Throws<Exception>(() => mockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).ToHaveReceivedMessage<PingMessage>());
        }

        [Fact]
        // [ExpectedException]
        public void actor_mocking01()
        {
            //Arrange
            // mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).SetUpMockActor<MockActor>();
            var pingPongActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItToChildActor(typeof(MockActor), new PingMessage()).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            mockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).NotToHaveReceivedMessage<PongMessage>();
            Xunit.Assert.Throws<Exception>(() => mockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).ToHaveReceivedMessage<PingMessage>());
        }

        [Fact]
        public void actor_mocking0()
        {
            //Arrange
            // mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).SetUpMockActor<MockActor>();
            var pingPongActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItToChildActor(typeof(MockActor), new PingMessage()).WhenActorInitializes().ItShouldCreateChildActor(pingActor).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            mockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).NotToHaveReceivedMessage<PongMessage>(10000);
            mockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).ToHaveReceivedMessage<PingMessage>(10000);
        }

        [Fact]
        public void actor_mocking()
        {
            //Arrange
            // mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActorRef<MockActor>();
            var pingPongActor = mockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItTo(pingActor, new PingMessage()).CreateMockActorRef<MockActor2>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
            mockFactory.ExpectMockActor(pingActor).NotToHaveReceivedMessage<PongMessage>();

            mockFactory.ExpectMockActor(pingActor).ToHaveReceivedMessage<PingMessage>(10000000);
        }

        [Fact]
        public void supervision()
        {
            //Arrange
            //  mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var called = false;
            var pingPongActor = mockFactory
                .WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage())
                .WhenActorReceive<PingMessage>().ItShouldDo((context, injectedActors) =>
                {
                    called = true;
                }).CreateMockActorRef<MockActor2>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            AwaitCondition(() => called, TimeSpan.FromMinutes(1));
            mockFactory.ExpectMockActor(pingPongActor).ToHaveReceivedMessage<PingMessage>();
        }
    }
}