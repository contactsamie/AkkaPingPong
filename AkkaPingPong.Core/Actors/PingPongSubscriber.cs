using Akka.Actor;
using AkkaPingPong.Core.Messages;
using System;
using AkkaPingPong.ActorSystemLib;

namespace AkkaPingPong.Core.Actors
{
    public class PingPongSubscriber : StatefullReceiveActor
    {
        public PingPongSubscriber()
        {
            Receive<CriticalErrorMessage>(message =>
            {
                var init = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message.Info);
                Console.ForegroundColor = init;
            });

            Receive<UnHandledMessageReceived>((message) =>
            {
                var init = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message.Info);
                Console.ForegroundColor = init;
            });

            ReceiveAny((message) =>
            {
                var init = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message.GetType().Name);
                Console.ForegroundColor = init;
            });
        }
    }
}