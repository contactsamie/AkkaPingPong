using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit.Messages;
using Autofac;
using System;
using System.Collections.Generic;
using AkkaPingPong.ASLTestKit.State;

namespace AkkaPingPong.ASLTestKit
{
    public class WhenActorReceives<T>
    {
        private Dictionary<Type, object> Mocks { set; get; }
        private IContainer Container { set; get; }
        private ActorSystem ActorSystem { set; get; }

        public WhenActorReceives(IContainer container, ActorSystem actorSystem, Dictionary<Type, object> mocks = null)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (actorSystem == null) throw new ArgumentNullException(nameof(actorSystem));
            ActorSystem = actorSystem;
            Container = container;
            Mocks = mocks ?? new Dictionary<Type, object>();
        }

        public WhenActorReceives<TT> AndWhenActorReceive<TT>()
        {
            return new WhenActorReceives<TT>(Container, ActorSystem, Mocks);
        }

        public WhenActorReceives<T> ExceptionShouldBeThrown(Exception exception = null)
        {
            Mocks.Add(typeof(T), new MockThrowExceptionMessage(exception));
            return this;
        }

        public WhenActorReceives<T> ItShouldTellAnotherActor<TA>()
        {
            Mocks.Add(typeof(T), new ItTellAnotherActorMessage(typeof(TA)));
            return this;
        }

        public object Message { get; set; }

        public WhenActorReceives<T> ItShouldTellAnotherActor(Type actorType)
        {
            Mocks.Add(typeof(T), new ItTellAnotherActorMessage(actorType));
            return this;
        }

        public WhenActorReceives<T> ItShouldTellSender<TResponse>(TResponse response = default(TResponse))
        {
            Mocks.Add(typeof(T), response);
            return this;
        }

        public IActorRef CreateMockActorRef<TActor>() where TActor : ActorBase
        {
            var actor = CreateMockActor<TActor>(Mocks);
            var actorref = ActorSystem.CreateActor<TActor>();

            return actorref;
        }

        public Type CreateMockActor<TActor>() where TActor : ActorBase
        {
            var actor = CreateMockActor<TActor>(Mocks);
            ActorSystem.CreateActor<TActor>();

            return actor;
        }

        public ActorSelection CreateMockActorSelection<TActor>() where TActor : ActorBase
        {
            var actor = CreateMockActor<TActor>(Mocks);
            ActorSystem.CreateActor<TActor>();
            var actorSelection = ActorSystem.LocateActor<TActor>();

            return actorSelection;
        }

        protected Type CreateMockActor<TMockActor>(Dictionary<Type, object> mocks) where TMockActor : ActorBase
        {
            foreach (var mock in mocks)
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<TMockActor>();
                var mockActorState = new MockActorState();
                if (Container.IsRegistered<IMockActorState>())
                {
                    var state = (MockActorState)Container.Resolve<IMockActorState>();
                    mockActorState = state ?? mockActorState;
                }
                mockActorState.MockSetUpMessages = mockActorState.MockSetUpMessages ?? new List<MockSetUpMessage>();
                mockActorState.MockSetUpMessages.Add(new MockSetUpMessage(typeof(TMockActor), mock.Key, mock.Value));
                builder.RegisterType<IMockActorState>();
                builder.Register<IMockActorState>(c => mockActorState);
                builder.Update(Container);
            }

            return typeof(TMockActor);
        }
    }

    public class WhenActorReceives : WhenActorReceives<object>
    {
        public WhenActorReceives(IContainer container, ActorSystem actorSystem, Dictionary<Type, object> mocks = null) : base(container, actorSystem, mocks)
        {
        }
    }
}