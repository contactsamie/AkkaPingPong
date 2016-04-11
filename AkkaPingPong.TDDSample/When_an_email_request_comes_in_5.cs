using Akka.Actor;
using Akka.TestKit.NUnit;
using AkkaPingPong.ActorSystemLib;
using Autofac;
using NUnit.Framework;

namespace AkkaPingPong.TDDSample
{
    /// <summary>
    /// Expected: True But was:  False
    /// </summary>
    [TestFixture]
    public class When_an_email_request_comes_in_5 : TestKit
    {
        [Test]
        [ExpectedException]
        public void it_should_send_it_out()
        {
            //Arrange
            ApplicationActorSystem.Register(new ContainerBuilder().Build(), null, Sys);
            ApplicationActorSystem.ActorSystem.CreateActor<EmailActor>();
            var emailAddress = "test@test.com";
            //Act
            ApplicationActorSystem.ActorSystem.LocateActor(typeof(EmailActor)).Tell(new SendEmailMessage(emailAddress));
            //Assert
            AwaitAssert(() => ExpectMsg<EmailSentMessage>(message => message.EmailAddress == emailAddress));
            Assert.IsTrue(TestEmailSender.HasSentEmail);
        }

        public interface IEmailSender
        {
            void Send(string emailAddress);
        }

        public class TestEmailSender : IEmailSender
        {
            public static bool HasSentEmail { set; get; }

            public void Send(string emailAddress)
            {
                HasSentEmail = true;
            }
        }

        public class SendEmailMessage
        {
            public SendEmailMessage(string emailAddress)
            {
                EmailAddress = emailAddress;
            }

            public string EmailAddress { private set; get; }
        }

        public class EmailSentMessage
        {
            public EmailSentMessage(string emailAddress)
            {
                EmailAddress = emailAddress;
            }

            public string EmailAddress { private set; get; }
        }

        public class EmailActor : ReceiveActor
        {
            public EmailActor()
            {
                Receive<SendEmailMessage>(message => Sender.Tell(new EmailSentMessage(message.EmailAddress)));
            }
        }
    }
}