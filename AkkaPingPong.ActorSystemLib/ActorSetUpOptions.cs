using Akka.Actor;
using Akka.Routing;

namespace AkkaPingPong.ActorSystemLib
{
    public class ActorSetUpOptions
    {
        public RouterConfig RouterConfig { set; get; }
        public SupervisorStrategy SupervisoryStrategy { set; get; }
        public string Dispatcher { set; get; }
        public string MailBox { set; get; }
    }
}