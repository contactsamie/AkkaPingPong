using Akka.Actor;
using AkkaPingPong.AkkaTestBase;
using AkkaPingPong.ASLTestKit.Mocks;
using AkkaPingPong.Core.Actors;
using AkkaPingPong.Core.Messages;
using System;
using Xunit;
using Xunit.Sdk;

namespace AkkaPingPong.Tests
{
    public class when_it_gets_a_ping_mocking : TestKitTestBase
    {
        [Fact]
        public void can_send()
        {
            //Arrange
            var mockActor = MockFactory.WhenActorReceives<PingMessage>()
              .ItShouldTellSender(new PongMessage())
              .CreateMockActorRef<MockActor>();

            var mockActor1 = MockFactory.WhenActorReceives<PingMessage>()
                .ItShouldForwardItTo<MockActor>(new PingMessage())
                .CreateMockActorRef<MockActor1>();

            var mockActor2 = MockFactory.WhenActorReceives<PingMessage>()
                .ItShouldForwardItTo<MockActor1>(new PingMessage())
                .CreateMockActor<MockActor2>();

            var mockActor3 = MockFactory.WhenActorReceives<PingMessage>()
                .ItShouldForwardItTo<MockActor2>(new PingMessage())
                .CreateMockActor<MockActor3>();

            var mockActor4 =
                      MockFactory.WhenActorReceives<PingMessage>()
                .ItShouldForwardItTo<MockActor3>(new PingMessage())
                .CreateMockActorRef<MockActor4>();

            mockActor4.Tell(new PingMessage());

            MockFactory.ExpectMockActor(mockActor4).ToHaveReceivedMessage<PingMessage>();
            MockFactory.ExpectMockActor(mockActor3).ToHaveReceivedMessage<PingMessage>();
            MockFactory.ExpectMockActor(mockActor2).ToHaveReceivedMessage<PingMessage>();
            MockFactory.ExpectMockActor(mockActor1).ToHaveReceivedMessage<PingMessage>();
            MockFactory.ExpectMockActor(mockActor).ToHaveReceivedMessage<PingMessage>();

            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromMinutes(1));
          
        }

        [Fact]
        public void can_send_to_all_child_actors_1()
        {
            //Arrange
            var mockActor1 = MockFactory.WhenActorReceives<PingMessage1>().ItShouldTellSender(new PongMessage())
                .SetUpMockActor<MockActor1>();

            var mockActor2 = MockFactory.WhenActorReceives<PingMessage2>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor2>();

            var mockActor3 = MockFactory.WhenActorReceives<PingMessage3>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor3>();

            var mockActor4 = MockFactory.WhenActorReceives<PingMessage4>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor4>();

            var mockActor = MockFactory
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor1))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor2))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor3))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor4))
                .WhenActorReceives<PingMessage1>().ItShouldTellItToChildActor<MockActor1>(new PingMessage1())
                .WhenActorReceives<PingMessage2>().ItShouldTellItToChildActor<MockActor2>(new PingMessage2())
                .WhenActorReceives<PingMessage3>().ItShouldTellItToChildActor<MockActor3>(new PingMessage3())
                .WhenActorReceives<PingMessage4>().ItShouldTellItToChildActor<MockActor4>(new PingMessage4())
                .CreateMockActorRef<MockActor<MockActor1, MockActor2, MockActor3, MockActor4>>();

            //Act
            // mockActor.Tell(new PingMessage1());
            mockActor.Tell(new PingMessage2());
            mockActor.Tell(new PingMessage3());
            mockActor.Tell(new PingMessage4());

            MockFactory.ExpectMockActor(mockActor1).WhoseParentIs(mockActor).NotToHaveReceivedMessage<PingMessage1>();
            MockFactory.ExpectMockActor(mockActor2).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage2>();
            MockFactory.ExpectMockActor(mockActor3).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage3>();
            MockFactory.ExpectMockActor(mockActor4).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage4>();
        }

        [Fact]
        public void can_send_to_all_child_actors_2()
        {
            //Arrange
            var mockActor1 = MockFactory.WhenActorReceives<PingMessage1>().ItShouldTellSender(new PongMessage())
                .SetUpMockActor<MockActor1>();

            var mockActor2 = MockFactory.WhenActorReceives<PingMessage2>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor2>();

            var mockActor3 = MockFactory.WhenActorReceives<PingMessage3>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor3>();

            var mockActor4 = MockFactory.WhenActorReceives<PingMessage4>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor4>();

            var mockActor = MockFactory
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor1))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor2))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor3))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor4))
                .WhenActorReceives<PingMessage1>().ItShouldTellItToChildActor<MockActor1>(new PingMessage1())
                .WhenActorReceives<PingMessage2>().ItShouldTellItToChildActor<MockActor2>(new PingMessage2())
                .WhenActorReceives<PingMessage3>().ItShouldTellItToChildActor<MockActor3>(new PingMessage3())
                .WhenActorReceives<PingMessage4>().ItShouldTellItToChildActor<MockActor4>(new PingMessage4())
                .CreateMockActorRef<MockActor<MockActor1, MockActor2, MockActor3, MockActor4>>();

            //Act
            mockActor.Tell(new PingMessage1());
            // mockActor.Tell(new PingMessage2());
            mockActor.Tell(new PingMessage3());
            mockActor.Tell(new PingMessage4());

            MockFactory.ExpectMockActor(mockActor1).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage1>();
            MockFactory.ExpectMockActor(mockActor2).WhoseParentIs(mockActor).NotToHaveReceivedMessage<PingMessage2>();
            MockFactory.ExpectMockActor(mockActor3).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage3>();
            MockFactory.ExpectMockActor(mockActor4).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage4>();
        }

        [Fact]
        public void can_send_to_all_child_actors_3()
        {
            //Arrange
            var mockActor1 = MockFactory.WhenActorReceives<PingMessage1>().ItShouldTellSender(new PongMessage())
                .SetUpMockActor<MockActor1>();

            var mockActor2 = MockFactory.WhenActorReceives<PingMessage2>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor2>();

            var mockActor3 = MockFactory.WhenActorReceives<PingMessage3>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor3>();

            var mockActor4 = MockFactory.WhenActorReceives<PingMessage4>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor4>();

            var mockActor = MockFactory
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor1))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor2))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor3))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor4))
                .WhenActorReceives<PingMessage1>().ItShouldTellItToChildActor<MockActor1>(new PingMessage1())
                .WhenActorReceives<PingMessage2>().ItShouldTellItToChildActor<MockActor2>(new PingMessage2())
                .WhenActorReceives<PingMessage3>().ItShouldTellItToChildActor<MockActor3>(new PingMessage3())
                .WhenActorReceives<PingMessage4>().ItShouldTellItToChildActor<MockActor4>(new PingMessage4())
                .CreateMockActorRef<MockActor<MockActor1, MockActor2, MockActor3, MockActor4>>();

            //Act
            mockActor.Tell(new PingMessage1());
            mockActor.Tell(new PingMessage2());
            // mockActor.Tell(new PingMessage3());
            mockActor.Tell(new PingMessage4());

            MockFactory.ExpectMockActor(mockActor1).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage1>();
            MockFactory.ExpectMockActor(mockActor2).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage2>();
            MockFactory.ExpectMockActor(mockActor3).WhoseParentIs(mockActor).NotToHaveReceivedMessage<PingMessage3>();
            MockFactory.ExpectMockActor(mockActor4).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage4>();
        }

        [Fact]
        public void can_send_to_all_child_actors_4()
        {
            //Arrange
            var mockActor1 = MockFactory.WhenActorReceives<PingMessage1>().ItShouldTellSender(new PongMessage())
                .SetUpMockActor<MockActor1>();

            var mockActor2 = MockFactory.WhenActorReceives<PingMessage2>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor2>();

            var mockActor3 = MockFactory.WhenActorReceives<PingMessage3>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor3>();

            var mockActor4 = MockFactory.WhenActorReceives<PingMessage4>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor4>();

            var mockActor = MockFactory
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor1))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor2))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor3))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor4))
                .WhenActorReceives<PingMessage1>().ItShouldTellItToChildActor<MockActor1>(new PingMessage1())
                .WhenActorReceives<PingMessage2>().ItShouldTellItToChildActor<MockActor2>(new PingMessage2())
                .WhenActorReceives<PingMessage3>().ItShouldTellItToChildActor<MockActor3>(new PingMessage3())
                .WhenActorReceives<PingMessage4>().ItShouldTellItToChildActor<MockActor4>(new PingMessage4())
                .CreateMockActorRef<MockActor<MockActor1, MockActor2, MockActor3, MockActor4>>();

            //Act
            mockActor.Tell(new PingMessage1());
            mockActor.Tell(new PingMessage2());
            mockActor.Tell(new PingMessage3());
            // mockActor.Tell(new PingMessage4());

            MockFactory.ExpectMockActor(mockActor1).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage1>();
            MockFactory.ExpectMockActor(mockActor2).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage2>();
            MockFactory.ExpectMockActor(mockActor3).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage3>();
            MockFactory.ExpectMockActor(mockActor4).WhoseParentIs(mockActor).NotToHaveReceivedMessage<PingMessage4>();
        }

        [Fact]
        public void can_send_to_all_child_actors1()
        {
            //Arrange
            var mockActor1 = MockFactory.WhenActorReceives<PingMessage1>().ItShouldTellSender(new PongMessage())
                .SetUpMockActor<MockActor1>();

            var mockActor2 = MockFactory.WhenActorReceives<PingMessage2>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor2>();

            var mockActor3 = MockFactory.WhenActorReceives<PingMessage3>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor3>();

            var mockActor4 = MockFactory.WhenActorReceives<PingMessage4>().ItShouldTellSender(new PongMessage())
               .SetUpMockActor<MockActor4>();

            var mockActor = MockFactory
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor1))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor2))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor3))
                .WhenActorInitializes().ItShouldCreateChildActor(typeof(MockActor4))
                .WhenActorReceives<PingMessage1>().ItShouldTellItToChildActor<MockActor1>(new PingMessage1())
                .WhenActorReceives<PingMessage2>().ItShouldTellItToChildActor<MockActor2>(new PingMessage2())
                .WhenActorReceives<PingMessage3>().ItShouldTellItToChildActor<MockActor3>(new PingMessage3())
                .WhenActorReceives<PingMessage4>().ItShouldTellItToChildActor<MockActor4>(new PingMessage4())
                .CreateMockActorRef<MockActor<MockActor1, MockActor2, MockActor3, MockActor4>>();

            //Act
            mockActor.Tell(new PingMessage1());
            mockActor.Tell(new PingMessage2());
            mockActor.Tell(new PingMessage3());
            mockActor.Tell(new PingMessage4());

            MockFactory.ExpectMockActor(mockActor1).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage1>();
            MockFactory.ExpectMockActor(mockActor2).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage2>();
            MockFactory.ExpectMockActor(mockActor3).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage3>();
            MockFactory.ExpectMockActor(mockActor4).WhoseParentIs(mockActor).ToHaveReceivedMessage<PingMessage4>();
        }

        [Fact]
        public void ItShouldTellAnotherActor1()
        {
            //Arrange
            var mock2 = MockFactory
                          .WhenActorReceives<PingMessage>()
                          .ItShouldTellSender(new PongMessage())
                          .CreateMockActorRef<MockActor2>();

            var mock1 = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellAnotherActor(mock2, new PingMessage())
                .CreateMockActorRef<MockActor1>();

            //Act
            mock1.Tell(new PingMessage());

            // AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
            Assert.Throws<TrueException>(() => AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5)));
            MockFactory.ExpectMockActor(mock2).ToHaveReceivedMessage<PingMessage>();
        }

        [Fact]
        public void ItShouldTellAnotherActor2()
        {
            //Arrange

            var mock1 = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellAnotherActor(typeof(MockActor2), new PingMessage())
                .CreateMockActorRef<MockActor1>();

            var mock2 = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellSender(new PongMessage())
                .CreateMockActorRef<MockActor2>();

            //Act
            mock1.Tell(new PingMessage());

            // AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
            Assert.Throws<TrueException>(() => AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5)));
            MockFactory.ExpectMockActor(mock2).ToHaveReceivedMessage<PingMessage>();
        }

        [Fact]
        public void ItShouldTellAnotherActor3()
        {
            //Arrange
            var mock2 = MockFactory
                          .WhenActorReceives<PingMessage>()
                          .ItShouldTellSender(new PongMessage())
                          .CreateMockActor<MockActor2>();

            var mock1 = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellAnotherActor(mock2, new PingMessage())
                .CreateMockActorRef<MockActor1>();

            //Act
            mock1.Tell(new PingMessage());

            // AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
            Assert.Throws<TrueException>(() => AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5)));
            MockFactory.ExpectMockActor(mock2).ToHaveReceivedMessage<PingMessage>();
        }

        [Fact]
        public void ItShouldForwardItToChildActor()
        {
            //Arrange

            var mock1 = MockFactory
                .WhenActorInitializes()
                .ItShouldCreateChildActor(typeof(MockActor2))
                .WhenActorReceives<PingMessage>()
                .ItShouldForwardItToChildActor(typeof(MockActor2), new PingMessage())
                .CreateMockActorRef<MockActor1<MockActor2>>();

            var mock2 = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellSender(new PongMessage())
                .SetUpMockActor<MockActor2>();

            //Act
            mock1.Tell(new PingMessage());

            // mockFactory.JustWait();
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
            MockFactory.ExpectMockActor(mock2).WhoseParentIs(mock1).ToHaveReceivedMessage<PingMessage>();
        }

        [Fact]
        public void ItShouldForwardItToChildActor2()
        {
            //Arrange

            var mock1 = MockFactory
                .WhenActorInitializes()
                .ItShouldCreateChildActor(typeof(MockActor2))
                .WhenActorReceives<PingMessage>()
                .ItShouldForwardItToChildActor(typeof(MockActor2), new PingMessage())
                .CreateMockActorRef<MockActor1<MockActor2>>();

            var mock2 = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellSender(new PongMessage())
                .SetUpMockActor<MockActor2>();

            //Act
            mock1.Tell(new PingMessage());

            // mockFactory.JustWait();
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
            Assert.Throws<Exception>(() => MockFactory.ExpectMockActor(mock2).ToHaveReceivedMessage<PingMessage>());
        }

        [Fact]
        public void ItShouldTellItToChildActor()
        {
            //Arrange

            var mock2 = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellSender(new PongMessage())
                .SetUpMockActor<MockActor2>();

            var mock1 = MockFactory
                .WhenActorInitializes()
                .ItShouldCreateChildActor(typeof(MockActor2))
                .WhenActorReceives<PingMessage>()
                .ItShouldTellItToChildActor(typeof(MockActor2), new PingMessage())
                .CreateMockActorRef<MockActor1<MockActor2>>();

            //Act
            mock1.Tell(new PingMessage());

            // mockFactory.JustWait();

            MockFactory.ExpectMockActor(mock2).WhoseParentIs(mock1).ToHaveReceivedMessage<PingMessage>();
        }

        [Fact]
        public void ItShouldTellItToChildActor2()
        {
            //Arrange

            var mock1 = MockFactory
                .WhenActorInitializes()
                .ItShouldCreateChildActor(typeof(MockActor2))
                .WhenActorReceives<PingMessage>()
                .ItShouldForwardItToChildActor(typeof(MockActor2), new PingMessage())
                .CreateMockActorRef<MockActor1<MockActor2>>();

            var mock2 = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellSender(new PongMessage())
                .SetUpMockActor<MockActor2>();

            //Act
            mock1.Tell(new PingMessage());

            // mockFactory.JustWait();

            Assert.Throws<Exception>(() => MockFactory.ExpectMockActor(mock2).ToHaveReceivedMessage<PingMessage>());
        }

        [Fact]
        public void ItShouldForwardItToAnotherActor()
        {
            //Arrange

            var mock1 = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldForwardItTo(typeof(MockActor2), new PingMessage())
                .CreateMockActorRef<MockActor1>();

            var mock2 = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellSender(new PongMessage())
                .CreateMockActorRef<MockActor2>();

            //Act
            mock1.Tell(new PingMessage());

            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
            MockFactory.ExpectMockActor(mock2).ToHaveReceivedMessage<PingMessage>();
        }

        [Fact]
        public void ItShouldTellSender()
        {
            //Arrange

            var mock = MockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActorRef<MockActor>();

            //Act
            mock.Tell(new PingMessage());

            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void it_should_do_a_pong()
        {
            //Arrange

            MockFactory.CreateActor<PingPongActor<MockActor>>();

            //Act
            MockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

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
            MockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();

            MockFactory.CreateActor<PingPongActor<MockActor>>();

            //Act
            MockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(5));
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));

            AwaitAssert(() => MockFactory.ExpectMockActor(typeof(MockActor)).WhoseParentIs(MockFactory.LocateActor(typeof(PingPongActor<>))).ToHaveReceivedMessage<PingMessage>(), TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void it_should_do_a_pong2()
        {
            //Arrange

            var pingActor = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellSender(new PongMessage())
                .CreateMockActor<MockActor1>();
            var pingPongActor = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldForwardItTo(pingActor, new PingMessage())
                .CreateMockActor<MockActor2>();

            var pingPongActorSelection = MockFactory.LocateActor(pingPongActor);

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
            var pingActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor1>();
            var pingPongActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItTo(pingActor, new PingMessage()).CreateMockActor<MockActor2>();
            var pingPongActorSelection = MockFactory.LocateActor(pingPongActor);

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

            MockFactory.CreateActor<PingPongActor<MockActor>>();

            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);

                MockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(20));
        }

        [Fact]
        public void it_should_do_a_pong_unit2()
        {
            //Arrange
            //mockFactory = new AkkaMockFactory(DependencyResolver.GetContainer(), Sys);
            MockFactory.CreateActor<PingCoordinatorActor<MockActor1, MockActor2>>();

            //Act
            MockFactory.LocateActor(typeof(PingCoordinatorActor<,>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<SorryImStashing>(), TimeSpan.FromSeconds(20));
        }

        //=====================================
        [Fact]
        public void actor_mocking014()
        {
            //Arrange
            //  mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();
            var pingPongActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldTellAnotherActor(typeof(MockActor), new PingMessage()).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            MockFactory.ExpectMockActor(pingActor).NotToHaveReceivedMessage<PongMessage>();
            MockFactory.ExpectMockActor(pingActor).ToHaveReceivedMessage<PingMessage>();
        }

        [Fact]
        //  [ExpectedException]
        public void actor_mocking013()
        {
            //Arrange

            //  mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();
            var pingPongActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItToChildActor(typeof(MockActor), new PingMessage()).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            MockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).NotToHaveReceivedMessage<PongMessage>();
            Xunit.Assert.Throws<Exception>(() => MockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).ToHaveReceivedMessage<PingMessage>());
        }

        [Fact]
        //  [ExpectedException]
        public void actor_mocking012()
        {
            //Arrange
            //   mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();
            var pingPongActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItToChildActor(typeof(MockActor), new PingMessage()).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            MockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).NotToHaveReceivedMessage<PongMessage>();
            Xunit.Assert.Throws<Exception>(() => MockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).ToHaveReceivedMessage<PingMessage>());
        }

        [Fact]
        // [ExpectedException]
        public void actor_mocking011()
        {
            //Arrange
            //  mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).SetUpMockActor<MockActor>();
            MockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActor<MockActor>();
            var pingPongActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItToChildActor(typeof(MockActor), new PingMessage()).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            MockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).NotToHaveReceivedMessage<PongMessage>();
            Xunit.Assert.Throws<Exception>(() => MockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).ToHaveReceivedMessage<PingMessage>());
        }

        [Fact]
        // [ExpectedException]
        public void actor_mocking01()
        {
            //Arrange
            // mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldTellSender(new PongMessage())
                .SetUpMockActor<MockActor>();

            var pingPongActor = MockFactory
                .WhenActorReceives<PingMessage>()
                .ItShouldForwardItToChildActor(typeof(MockActor), new PingMessage())
                .CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            MockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).NotToHaveReceivedMessage<PongMessage>();
            Xunit.Assert.Throws<Exception>(() => MockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).ToHaveReceivedMessage<PingMessage>());
        }

        [Fact]
        public void actor_mocking0()
        {
            //Arrange
            // mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).SetUpMockActor<MockActor>();
            var pingPongActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItToChildActor(typeof(MockActor), new PingMessage()).WhenActorInitializes().ItShouldCreateChildActor(pingActor).CreateMockActorRef<MockActor2<MockActor>>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            MockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).NotToHaveReceivedMessage<PongMessage>(20000);
            MockFactory.ExpectMockActor(pingActor).WhoseParentIs(pingPongActor).ToHaveReceivedMessage<PingMessage>(20000);
        }

        [Fact]
        public void actor_mocking()
        {
            //Arrange
            // mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var pingActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage()).CreateMockActorRef<MockActor>();
            var pingPongActor = MockFactory.WhenActorReceives<PingMessage>().ItShouldForwardItTo(pingActor, new PingMessage()).CreateMockActorRef<MockActor2>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(5));
            MockFactory.ExpectMockActor(pingActor).NotToHaveReceivedMessage<PongMessage>();

            MockFactory.ExpectMockActor(pingActor).ToHaveReceivedMessage<PingMessage>();
        }

        [Fact]
        public void Supervision()
        {
            //Arrange
            //  mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
            var called = false;
            var pingPongActor = MockFactory
                .WhenActorReceives<PingMessage>().ItShouldTellSender(new PongMessage())
                .WhenActorReceives<PingMessage>().ItShouldDo((context, injectedActors, actorInstance) =>
                {
                    called = true;
                }).CreateMockActorRef<MockActor2>();

            //Act
            pingPongActor.Tell(new PingMessage());

            //Assert
            AwaitCondition(() => called, TimeSpan.FromMinutes(1));
            MockFactory.ExpectMockActor(pingPongActor).ToHaveReceivedMessage<PingMessage>();
        }
    }
}