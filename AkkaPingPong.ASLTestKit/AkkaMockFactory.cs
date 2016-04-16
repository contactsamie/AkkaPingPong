using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit.Messages;
using AkkaPingPong.ASLTestKit.Mocks;
using AkkaPingPong.Core;
using Autofac;
using System;
using System.Collections.Concurrent;

namespace AkkaPingPong.ASLTestKit
{
    public class AkkaMockFactory
    {
        public ConcurrentDictionary<Guid, MockMessages> MessagesReceived { get; }

        public AkkaMockFactory(IContainer container, ActorSystem actorSystem)
        {
            MessagesReceived = new ConcurrentDictionary<Guid, MockMessages>();
            Container = container;
            ActorSystem = actorSystem;
            var preBuilder = new ContainerBuilder();
            preBuilder.Register(x => new FakeActorSystemFactory()).As<IActorSystemFactory>().SingleInstance();
            preBuilder.Update(Container);
            ActorSystemfactory = Container.Resolve<IActorSystemFactory>();
            ActorSystemfactory.Register(Container, (builder) => { }, ActorSystem);
        }

        public IActorRef CreateActor<T>(ActorSetUpOptions option = null, ActorMetaData parentActorMetaData = null) where T : ActorBase
        {
            return ActorSystem.CreateActor<T>(option, parentActorMetaData);
        }

        public IActorSystemFactory ActorSystemfactory { set; get; }

        public ActorReceives<T> WhenActorReceives<T>(T message = default(T))
        {
            return new ActorReceives<T>(MessagesReceived, Container, ActorSystem, null, message);
        }

        public ActorReceives<MockActorInitializationMessage> WhenActorInitializes()
        {
            return new ActorReceives<MockActorInitializationMessage>(MessagesReceived, Container, ActorSystem);
        }

        public ActorSelection LocateActor<T>(ActorMetaData parentActorMetaData = null)
        {
            return SelectableActor.Select(typeof(T), parentActorMetaData, ActorSystem);
        }

        public ActorSelection LocateActor(Type type, ActorMetaData parentActorMetaData = null)
        {
            return SelectableActor.Select(type, parentActorMetaData, ActorSystem);
        }

        public ExpectMockActor ExpectMockActor(Type actor)
        {
            return new ExpectMockActor(MessagesReceived, actor, Container, ActorSystem);
        }

        public ExpectMockActor ExpectMockActor(IActorRef actorRef)
        {
            return new ExpectMockActor(MessagesReceived, actorRef, Container, ActorSystem);
        }

        private IContainer Container { set; get; }
        private ActorSystem ActorSystem { set; get; }

        public void UpdateContainer(Action<ContainerBuilder> action)
        {
            var builder = new ContainerBuilder();
            action(builder);

            builder.Update(Container);
        }

        public void Dispose()
        {
            ActorSystemfactory.ShutDownActorSystem();

            Container.Dispose();
        }

        public void JustWait(int durationMilliseconds = 600000)
        {
            var now = DateTime.Now;
            var counter = 0;
            while ((DateTime.Now - now).TotalMilliseconds < durationMilliseconds)
            {
                System.Threading.Thread.Sleep(10 * counter);
                counter++;
            }
        }
    }
}