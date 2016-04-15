using Akka.Actor;
using AkkaPingPong.AkkaTestBase;
using System;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace AkkaPingPong.TDDSample
{
    /// <summary>
    /// Expected: True But was:  False
    /// </summary>

    public class When_an_email_request_comes_in_5 : TestKitTestBase
    {
        [Fact]
        // [ExpectedException]
        public void it_should_send_it_out()
        {
            //Arrange

            mockFactory.CreateActor<EmailActor>();
            var emailAddress = "test@test.com";
            //Act
            mockFactory.LocateActor(typeof(EmailActor)).Tell(new SendEmailMessage(emailAddress));
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

        public class EmailActor : ReceiveActor
        {
            public EmailActor()
            {
                Receive<SendEmailMessage>(message => Sender.Tell(new EmailSentMessage(message.EmailAddress)));
            }
        }
    }
}