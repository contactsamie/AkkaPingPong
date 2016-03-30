using System;

namespace AkkaPingPong.Common
{
    public class PingPongService : IPingPongService
    {
        public void Execute()
        {
            Console.WriteLine("Pong");
        }
    }
}