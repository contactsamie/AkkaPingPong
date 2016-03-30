using System;
using System.Threading.Tasks;

namespace AkkaPingPong.Common
{
    public class PingPongService : IPingPongService
    {
        public void Execute()
        {
            Console.WriteLine("Pong");
        }

        public async Task ExecuteAsync()
        {
            await Task.Delay(5000);
            Console.WriteLine("Pong Async");
        }
    }
}