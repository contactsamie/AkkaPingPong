using AkkaPingPong.AkkaTestBase;
using AkkaPingPong.ASLTestKit.Mocks;
using AkkaPingPong.Common;
using AkkaPingPong.Core.Actors;
using AkkaPingPong.Core.Messages;
using Autofac;
using System;
using Xunit;

namespace AkkaPingPong.Tests
{
    public class when_it_gets_a_ping : TestKitTestBase
    {
        [Fact]
        public void it_should_do_a_pong()
        {
            //Arrange
            //  var container = new ContainerBuilder().Build();
            // mockFactory = new AkkaMockFactory(container, Sys);
            MockFactory.UpdateContainer((builder) =>
            {
                builder.Register<IPingPongService>(b => new FakePingPongService());
            });
            MockFactory.CreateActor<PingPongActor<MockActor>>();

            //Act
            MockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void it_should_do_a_pong_integration()
        {
            //Arrange
            //var container = new ContainerBuilder().Build();
            // mockFactory = new AkkaMockFactory(container, Sys);
            MockFactory.UpdateContainer((builder) =>
            {
                builder.Register<IPingPongService>(b => new FakePingPongService());
            });
            MockFactory.CreateActor<PingPongActor<PingCoordinatorActor<PingActor, PingBlockingActor>>>();

            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                MockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(20));
        }

        [Fact]
        public void it_should_do_a_pong_unit1()
        {
            // var container = new ContainerBuilder().Build();
            //  mockFactory = new AkkaMockFactory(container, Sys);
            MockFactory.UpdateContainer((builder) =>
            {
                builder.Register<IPingPongService>(b => new FakePingPongService());
            });
            //Arrange
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
            // var container = new ContainerBuilder().Build();
            //  mockFactory = new AkkaMockFactory(container, Sys);
            MockFactory.UpdateContainer((builder) =>
            {
                builder.Register<IPingPongService>(b => new FakePingPongService());
            });
            //Arrange
            MockFactory.CreateActor<PingCoordinatorActor<MockActor1, MockActor2>>();

            //Act
            MockFactory.LocateActor(typeof(PingCoordinatorActor<,>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<SorryImStashing>(), TimeSpan.FromSeconds(20));
        }
    }
}