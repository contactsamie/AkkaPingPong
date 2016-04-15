using Akka.Actor;
using AkkaPingPong.AkkaTestBase;
using Autofac;
using System;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace AkkaPingPong.TDDSample
{
    /// <summary>
    /// Timeout 00:00:03 while waiting for a message of type AkkaPingPong.Tests.TDD_Sample.When_an_email_request_comes_in_7+EmailSentMessage
    /// </summary>

    public class When_an_email_request_comes_in_7 : TestKitTestBase
    {
        [Fact]
        // [ExpectedException]
        public void it_should_send_it_out()
        {
            //Arrange
            mockFactory.UpdateContainer((builder) => builder.Register((r) => new TestEmailSender()).As<IEmailSender>());
            mockFactory.CreateActor<EmailSupervisorActor<EmailActor>>();
            var emailAddress = "test@test.com";
            //Act
            mockFactory.LocateActor(typeof(EmailSupervisorActor<>)).Tell(new SendEmailMessage(emailAddress));
            //Assert
            try
            {
                AwaitAssert(() => ExpectMsg<EmailSentMessage>(message => message.EmailAddress == emailAddress));
                Assert.IsTrue(TestEmailSender.HasSentEmail);
                throw new Exception();
            }
            catch (Exception)
            {
            }
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

        public class EmailSupervisorActor<TEmailActor> : ReceiveActor
        {
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