using Akka.Actor;
using Akka.Event;
using AkkaPingPong.Common;
using AkkaPingPong.Core.Messages;

namespace AkkaPingPong.Core.Actors
{
    public class PingBlockingActor : ReceiveActor
    {
        private  ILoggingAdapter _logger = Context.GetLogger();
        public PingBlockingActor(IPingPongService service)
        {
            _logger.Debug(GetType().FullName + " Running ...");

            Receive<bool>( message =>
            {
                _logger.Debug(GetType().FullName + " response from async service ...");

                Sender.Tell(new PongBlockingMessage());
            });

            Receive<PingMessage>( message =>
            {
                _logger.Debug(GetType().FullName + " making async call ...");

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