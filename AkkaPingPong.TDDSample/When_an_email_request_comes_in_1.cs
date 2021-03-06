﻿using Akka.Actor;
using AkkaPingPong.AkkaTestBase;
using Xunit;
using Xunit.Sdk;

namespace AkkaPingPong.TDDSample
{
    /// <summary>
    /// Failed: Timeout 00:00:03 while waiting for a message of type AkkaPingPong.Tests.TDD_Sample.Step1.EmailSentMessage
    /// </summary>

    public class When_an_email_request_comes_in_1 : TestKitTestBase
    {
        [Fact]
        public void it_should_send_it_out()
        {
            //Arrange
            MockFactory.CreateActor<EmailActor>();

            //Act
            MockFactory.LocateActor(typeof(EmailActor)).Tell(new SendEmailMessage());

            //Assert
            Assert.Throws<TrueException>(() => AwaitAssert(() => ExpectMsg<EmailSentMessage>()));
        }

        public class SendEmailMessage { }

        public class EmailSentMessage { }

        public class EmailActor : ReceiveActor { }
    }
}