using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit.Messages;
using Autofac;

namespace AkkaPingPong.ASLTestKit
{
    public class AkkaMockFactory
    {
        public AkkaMockFactory(IContainer container, ActorSystem actorSystem)
        {
            Container = container;
            ActorSystem = actorSystem;
        }

        public WhenActorReceives<T> WhenActorReceives<T>()
        {
            return new WhenActorReceives<T>(Container, ActorSystem);
        }

        public async Task<List<T>> GetAllReceivecMessagesOfType<T>(Type actor, ActorSelection parentActor) where T : class
        {
            var messages = await ActorSystem.LocateActor(actor, parentActor).Ask(new GetAllPreviousMessagesReceivedByMockActor()).ConfigureAwait(false);

            var m = messages as Dictionary<Guid, object>;
            return m?.Where(x => x.Value is T).Select(x => x.Value as T).ToList() ?? new List<T>();
        }

        public List<T> ExpectMockActorToReceiveMessage<T>(Type actor, ActorSelection parentActor = null, Func<T, bool> messageValidator = null) where T : class
        {
            var messages = GetAllReceivecMessagesOfType<T>(actor, parentActor).Result;
            if (messageValidator == null ? !messages.Any() : !messages.Any(messageValidator))
            {
                throw new Exception("Expected actor to receive message " + typeof(T).FullName);
            }
            return messages;
        }

        private IContainer Container { set; get; }
        private ActorSystem ActorSystem { set; get; }
    }
}