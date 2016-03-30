using Akka.Actor;
using AkkaPingPong.Common;
using AkkaPingPong.Core.Messages;

namespace AkkaPingPong.Core.Actors
{
    public class PingActor : ReceiveActor
    {
        public PingActor(IPingPongService service)
        {
            Receive<PingMessage>(message =>
            {
                service.Execute();
                Sender.Tell(new PongMessage());
            });
        }
    }
}