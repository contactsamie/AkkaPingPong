using Akka.Actor;
using AkkaPingPong.Common;
using AkkaPingPong.Core.Messages;

namespace AkkaPingPong.Core.Actors
{
    public class PingBlockingActor : ReceiveActor
    {
        public PingBlockingActor(IPingPongService service)
        {
            Receive<bool>( message =>
            {
                 Sender.Tell(new PongBlockingMessage());
            });

            Receive<PingMessage>( message =>
            {
                 service.ExecuteAsync().PipeTo(Self,Sender);
            });
            /*
             Receive<PingMessage>(async message =>
            {
                var sender = Sender;
                await service.ExecuteAsync();
                sender.Tell(new PongBlockingMessage());
            });
            */
        }
    }
}