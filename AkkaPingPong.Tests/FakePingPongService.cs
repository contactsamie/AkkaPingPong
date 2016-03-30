using System;
using System.Threading.Tasks;
using AkkaPingPong.Common;

namespace AkkaPingPong.Tests
{
    public class FakePingPongService : IPingPongService
    {
        public void Execute()
        {
            Console.WriteLine("Fake Pong");
        }
        public async Task<bool> ExecuteAsync()
        {
          await Task.Delay(5000);
            Console.WriteLine("Pong Async");
            return await Task.FromResult(true);
        }
    }
}