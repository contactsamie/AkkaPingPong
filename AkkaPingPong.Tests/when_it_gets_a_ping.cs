using AkkaPingPong.Core.Messages;
using NUnit.Framework;
using System;

namespace AkkaPingPong.Tests
{
    public class when_it_gets_a_ping : AkkaTestBase
    {
        [Test]
        public void it_should_do_a_pong()
        {
            //Act
            for (var i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                Selector.PingPongActorSelector.Select().Tell(new PingMessage());
            }
            JustWait(5000);
            //Assert
            AwaitAssert(() => Subscriber.ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(20));
        }
    }
}