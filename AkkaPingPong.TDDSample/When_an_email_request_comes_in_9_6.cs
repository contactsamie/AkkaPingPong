using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.AkkaTestBase;
using AkkaPingPong.ASLTestKit.Mocks;
using System;
using Xunit;

namespace AkkaPingPong.TDDSample
{
    /// <summary>
    /// Pass
    /// </summary>

    public class When_an_email_request_comes_in_9_6 : TestKitTestBase
    {
        [Fact]
        public void it_should_send_it_out()
        {
            //Arrange

            const string emailAddress = "test@test.com";
            MockFactory.WhenActorReceives<SendEmailMessage>().ItShouldTellSender(new EmailSentMessage(emailAddress)).SetUpMockActor<MockActor>();
            MockFactory.CreateActor<EmailSupervisorActor<MockActor>>();

            //Act
            MockFactory.LocateActor(typeof(EmailSupervisorActor<>)).Tell(new SendEmailMessage(emailAddress));

            //Assert
            AwaitAssert(() => ExpectMsg<EmailReadyToSendMessage>(message => message.EmailAddress == emailAddress), TimeSpan.FromMinutes(1));
        }

        [Fact]
        public void it_should_send_it_out2()
        {
            //Arrange
            const string emailAddress = "test@test.com";
            MockFactory.WhenActorReceives<SendEmailMessage>().ItShouldTellSender(new EmailSentMessage(emailAddress)).SetUpMockActor<MockActor>();
            MockFactory.CreateActor<EmailSupervisorActor<MockActor>>();

            //Act
            MockFactory.LocateActor(typeof(EmailSupervisorActor<>)).Tell(new SendEmailMessage(emailAddress));

            //Assert
            AwaitAssert(() => ExpectMsg<EmailSentMessage>(message => message.EmailAddress == emailAddress), TimeSpan.FromMinutes(1));
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
                    MailActor.Forward(message);
                    Sender.Tell(new EmailReadyToSendMessage(message.EmailAddress));
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