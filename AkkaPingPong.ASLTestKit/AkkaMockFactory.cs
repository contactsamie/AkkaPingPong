using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.AkkaTestBase;
using AkkaPingPong.ASLTestKit.Messages;
using AkkaPingPong.Core;
using Autofac;
using System;

namespace AkkaPingPong.ASLTestKit
{
    public class AkkaMockFactory : IDisposable
    {
        public AkkaMockFactory(IContainer container, ActorSystem actorSystem)
        {
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
            return new ActorReceives<T>(Container, ActorSystem, null, message);
        }

        public ActorReceives<MockActorInitializationMessage> WhenActorInitializes()
        {
            return new ActorReceives<MockActorInitializationMessage>(Container, ActorSystem);
        }

        public ActorSelection LocateActor(Type type, ActorMetaData parentActorMetaData = null)
        {
            return SelectableActor.Select(type, parentActorMetaData, ActorSystem);
        }

        public ExpectMockActor ExpectMockActor(Type actor)
        {
            return new ExpectMockActor(actor, Container, ActorSystem);
        }

        public ExpectMockActor ExpectMockActor(IActorRef actorRef)
        {
            return new ExpectMockActor(actorRef, Container, ActorSystem);
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
            var name = ActorSystem.Name;
            ActorSystem.Terminate();
            ActorSystem.WhenTerminated.Wait();
            Container.Dispose();
            Console.WriteLine("ActorSystem terminated at " + DateTime.UtcNow + " : " + name);
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