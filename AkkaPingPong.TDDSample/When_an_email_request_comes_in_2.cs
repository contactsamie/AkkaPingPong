using Akka.Actor;
using AkkaPingPong.AkkaTestBase;

using Xunit;

namespace AkkaPingPong.TDDSample
{
    /// <summary>
    /// Test Passes
    /// </summary>

    public class When_an_email_request_comes_in_2 : TestKitTestBase
    {
        [Fact]
        public void it_should_send_it_out()
        {
            //Arrange

            MockFactory.CreateActor<EmailActor>();
            //Act
            MockFactory.LocateActor(typeof(EmailActor)).Tell(new SendEmailMessage());
            //Assert
            AwaitAssert(() => ExpectMsg<EmailSentMessage>());
        }

        public class SendEmailMessage { }

        public class EmailSentMessage { }

        public class EmailActor : ReceiveActor
        {
            public EmailActor()
            {
                ReceiveAny(message => Sender.Tell(new EmailSentMessage()));
            }
        }
    }
}