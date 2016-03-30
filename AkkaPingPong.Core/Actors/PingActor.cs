using Akka.Actor;
using Akka.Event;
using AkkaPingPong.Common;
using AkkaPingPong.Core.Messages;

namespace AkkaPingPong.Core.Actors
{
    public class PingActor : ReceiveActor
    {
        private  ILoggingAdapter _logger = Context.GetLogger();

        public PingActor(IPingPongService service)
        {
            _logger.Debug(GetType().FullName+" Running ...");
            Receive<PingMessage>(message =>
            {
                service.Execute();
                Sender.Tell(new PongMessage());
            });
        }
    }
}