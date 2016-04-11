using Akka.Actor;
using Akka.TestKit.NUnit;
using AkkaPingPong.ActorSystemLib;
using Autofac;
using NUnit.Framework;

namespace AkkaPingPong.TDDSample
{
    /// <summary>
    /// Failed: Timeout 00:00:03 while waiting for a message of type AkkaPingPong.Tests.TDD_Sample.When_an_email_request_comes_in_9+EmailReadyToSendMessage
    /// </summary>
    [TestFixture]
    public class When_an_email_request_comes_in_9 :TestKit
    {
        [Test]
        [ExpectedException]
        public void it_should_send_it_out()
        {
            //Arrange
            ApplicationActorSystem.Register(new ContainerBuilder().Build(), (builder)=>builder.Register((r)=>new TestEmailSender()).As<IEmailSender>(), Sys);
            ApplicationActorSystem.ActorSystem.CreateActor<EmailSupervisorActor<EmailActor>>();
            var emailAddress = "test@test.com";
            //Act
            ApplicationActorSystem.ActorSystem.LocateActor(typeof(EmailSupervisorActor<>)).Tell(new SendEmailMessage(emailAddress));
            //Assert
         
            AwaitAssert(() => ExpectMsg<EmailReadyToSendMessage>(message => message.EmailAddress == emailAddress));
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

        public class EmailReadyToSendMessage
        {
            public EmailReadyToSendMessage(string emailAddress)
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
        public class EmailSupervisorActor<TEmailActor> : ReceiveActor where TEmailActor : ActorBase
        {
            public EmailSupervisorActor()
            {
                MailActor = Context.System.CreateActor<TEmailActor>();
                Receive<SendEmailMessage>(message =>
                {
                    MailActor.Tell(message,Sender);
                });
            }

            private IActorRef MailActor { get; set; }
        }
        public class EmailActor : ReceiveActor
        {
            public EmailActor(IEmailSender emailSender)
            {
                Receive<SendEmailMessage>(message =>
                {
                    emailSender.Send(message.EmailAddress);
                    Sender.Tell(new EmailSentMessage(message.EmailAddress));
                });
            }
        }
    }
}