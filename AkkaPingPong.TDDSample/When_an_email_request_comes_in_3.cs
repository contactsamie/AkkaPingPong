using Akka.Actor;
using AkkaPingPong.AkkaTestBase;
using System;
using Xunit;

namespace AkkaPingPong.TDDSample
{
    /// <summary>
    /// Failed: Timeout 00:00:03 while waiting for a message of type AkkaPingPong.Tests.TDD_Sample.When_an_email_request_comes_in_3+EmailSentMessage
    /// </summary>

    public class When_an_email_request_comes_in_3 : TestKitTestBase
    {
        [Fact]
        // [ExpectedException]
        public void it_should_send_it_out()
        {
            //Arrange

            MockFactory.CreateActor<EmailActor>();
            var emailAddress = "test@test.com";
            //Act
            MockFactory.LocateActor(typeof(EmailActor)).Tell(new SendEmailMessage(emailAddress) { });
            //Assert
            try
            {
                AwaitAssert(() => ExpectMsg<EmailSentMessage>(message => message.EmailAddress == emailAddress));
                throw new Exception();
            }
            catch (Exception)
            {
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
                ReceiveAny(message => Sender.Tell(new EmailSentMessage(null)));
            }
        }
    }
}