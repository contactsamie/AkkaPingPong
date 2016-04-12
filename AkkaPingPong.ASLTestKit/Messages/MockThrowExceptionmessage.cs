using System;

namespace AkkaPingPong.ASLTestKit.Messages
{
    public class MockThrowExceptionMessage
    {
        public MockThrowExceptionMessage(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { private set; get; }
    }
}