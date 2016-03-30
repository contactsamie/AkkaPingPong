using Akka.Actor;
using AkkaPingPong.Common;
using AkkaPingPong.Core.Messages;

namespace AkkaPingPong.Core.Actors
{
    public class PingBlockingActor : ReceiveActor
    {
        public PingBlockingActor(IPingPongService service)
        {
            Receive<PingMessage>(async message =>
            {
                var sender = Sender;
                await service.ExecuteAsync();
                sender.Tell(new PongBlockingMessage());
            });
        }
    }
}