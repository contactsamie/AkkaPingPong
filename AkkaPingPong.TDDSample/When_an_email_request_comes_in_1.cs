using Akka.Actor;
using Akka.TestKit.NUnit;
using AkkaPingPong.ActorSystemLib;
using Autofac;
using NUnit.Framework;

namespace AkkaPingPong.TDDSample
{
    /// <summary>
    /// Failed: Timeout 00:00:03 while waiting for a message of type AkkaPingPong.Tests.TDD_Sample.Step1.EmailSentMessage
    /// </summary>
    [TestFixture]
    public class When_an_email_request_comes_in_1 : TestKit
    {
        [Test]
        [ExpectedException]
        public void it_should_send_it_out()
        {
            //Arrange
            ApplicationActorSystem.Register(new ContainerBuilder().Build(), null, Sys);
            ApplicationActorSystem.ActorSystem.CreateActor<EmailActor>();
            //Act
            ApplicationActorSystem.ActorSystem.LocateActor(typeof(EmailActor)).Tell(new SendEmailMessage());
            //Assert
            AwaitAssert(() => ExpectMsg<EmailSentMessage>());
        }

        public class SendEmailMessage { }

        public class EmailSentMessage { }

        public class EmailActor : ReceiveActor { }
    }
}