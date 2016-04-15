using AkkaPingPong.AkkaTestBase;
using AkkaPingPong.ASLTestKit.Mocks;
using AkkaPingPong.Common;
using AkkaPingPong.Core.Actors;
using AkkaPingPong.Core.Messages;
using Autofac;
using System;
using Xunit;

//[assembly: CollectionBehavior(DisableTestParallelization = true)]

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
            mockFactory.UpdateContainer((builder) =>
            {
                builder.Register<IPingPongService>(b => new FakePingPongService());
            });
            mockFactory.CreateActor<PingPongActor<MockActor>>();

            //Act
            mockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void it_should_do_a_pong_integration()
        {
            //Arrange
            //var container = new ContainerBuilder().Build();
            // mockFactory = new AkkaMockFactory(container, Sys);
            mockFactory.UpdateContainer((builder) =>
            {
                builder.Register<IPingPongService>(b => new FakePingPongService());
            });
            mockFactory.CreateActor<PingPongActor<PingCoordinatorActor<PingActor, PingBlockingActor>>>();

            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                mockFactory.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());
            }

            //Assert
            AwaitAssert(() => ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(20));
        }

        [Fact]
        public void it_should_do_a_pong_unit1()
        {
            // var container = new ContainerBuilder().Build();
            //  mockFactory = new AkkaMockFactory(container, Sys);
            mockFactory.UpdateContainer((builder) =>
            {
                builder.Register<IPingPongService>(b => new FakePingPongService());
            });
            //Arrange
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
            // var container = new ContainerBuilder().Build();
            //  mockFactory = new AkkaMockFactory(container, Sys);
            mockFactory.UpdateContainer((builder) =>
            {
                builder.Register<IPingPongService>(b => new FakePingPongService());
            });
            //Arrange
            mockFactory.CreateActor<PingCoordinatorActor<MockActor1, MockActor2>>();

            //Act
            mockFactory.LocateActor(typeof(PingCoordinatorActor<,>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<SorryImStashing>(), TimeSpan.FromSeconds(20));
        }
    }
}