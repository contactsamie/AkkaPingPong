using System;
using System.Collections.Generic;
using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.Core;
using AkkaPingPong.Core.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace AkkaPingPong.Tests
{

    public class when_it_gets_a_ping:AkkaTestBase
    {

        [Test]
        public void it_should_do_a_pong()
        {
            //Act
            for (int i = 0; i < 10; i++)
            {
                 System.Threading.Thread.Sleep(1000);
                 ApplicationActorSystem.AppActorRef.Tell(new PingMessage());
            }
           

            JustWait(5000);
            //Assert
            AwaitAssert(() => Subscriber.ExpectMsg<PongMessage>(), TimeSpan.FromSeconds(20));

        }
    }
}
