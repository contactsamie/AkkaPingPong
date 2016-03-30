using System;
using AkkaPingPong.Common;

namespace AkkaPingPong.Tests
{
    public class FakePingPongService : IPingPongService
    {
        public void Execute()
        {
            Console.WriteLine("Fake Pong");
        }
    }
}