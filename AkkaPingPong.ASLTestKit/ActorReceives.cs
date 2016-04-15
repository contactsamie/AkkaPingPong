using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit.Messages;
using AkkaPingPong.ASLTestKit.Models;
using AkkaPingPong.ASLTestKit.State;
using Autofac;
using System;
using System.Collections.Generic;

namespace AkkaPingPong.ASLTestKit
{
    public class ActorReceives<T>
    {
        private Dictionary<Tuple<Guid, Type>, object> Mocks { set; get; }
        private IContainer Container { set; get; }
        private ActorSystem ActorSystem { set; get; }
        private T ReceivedMessage { set; get; }

        public ActorReceives(IContainer container, ActorSystem actorSystem, Dictionary<Tuple<Guid, Type>, object> mocks = null, T receivedMessage = default(T))
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (actorSystem == null) throw new ArgumentNullException(nameof(actorSystem));
            ActorSystem = actorSystem;
            Container = container;
            Mocks = mocks ?? new Dictionary<Tuple<Guid, Type>, object>();
            ReceivedMessage = receivedMessage;
        }

        public ActorReceives<TT> WhenActorReceive<TT>()
        {
            return new ActorReceives<TT>(Container, ActorSystem, Mocks);
        }

        public ActorReceives<MockActorInitializationMessage> WhenActorInitializes()
        {
            return new ActorReceives<MockActorInitializationMessage>(Container, ActorSystem, Mocks);
        }

        public ActorReceives<T> ExceptionShouldBeThrown(Exception exception = null)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new MockThrowExceptionMessage(exception));
            return this;
        }

        public ActorReceives<T> ItShouldTellAnotherActor<TA>(object message, ActorMetaData parent = null)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellAnotherActorTypeMessage(typeof(TA), message, parent));
            return this;
        }

        public ActorReceives<T> ItShouldTellAnotherActor(Type actorType, object message, ActorSelection parent = null)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellAnotherActorTypeMessage(actorType, message, parent?.ToActorMetaData()));
            return this;
        }

        public ActorReceives<T> ItShouldTellAnotherActor(IActorRef actorRef, object message = null)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellAnotherRefActorMessage(actorRef, message));
            return this;
        }

        public ActorReceives<T> ItShouldDoNothing()
        {
            return this;
        }

        public ActorReceives<T> ItShouldCreateChildActor(Type childActorType, ActorSetUpOptions options = null)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new MockCreateChildActorMessage(childActorType, options));
            return this;
        }

        public object Message { get; set; }

        public ActorReceives<T> ItShouldForwardItTo(Type actorType, object message, ActorSelection parent = null)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellAnotherActorTypeMessage(actorType, message, parent?.ToActorMetaData()));
            return this;
        }

        public ActorReceives<T> ItShouldTellItToChildActor(Type actorType, object message)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellChildActorTypeMessage(actorType, message));
            return this;
        }

        public ActorReceives<T> ItShouldForwardItToChildActor(Type actorType, object message)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new ForwardToChildActorTypeMessage(actorType, message));
            return this;
        }

        public ActorReceives<T> ItShouldForwardItTo(IActorRef actorType, object message)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellAnotherRefActorMessage(actorType, message));
            return this;
        }

        public ActorReceives<T> ItShouldTellSender<TResponse>(TResponse response)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), response);
            return this;
        }

        public IActorRef CreateMockActorRef<TActor>() where TActor : ActorBase
        {
            var actor = SetUpMockActor<TActor>();
            var actorref = ActorSystem.CreateActor<TActor>();

            return actorref;
        }

        public Type SetUpMockActor<TActor>() where TActor : ActorBase
        {
            var actor = CreateMockActor<TActor>(Mocks);
            return actor;
        }

        public Type CreateMockActor<TActor>() where TActor : ActorBase
        {
            var actor = SetUpMockActor<TActor>();
            var actorref = ActorSystem.CreateActor<TActor>();
            return actor;
        }

        public ActorSelection CreateMockActorSelection<TActor>() where TActor : ActorBase
        {
            var actor = CreateMockActor<TActor>(Mocks);
            ActorSystem.CreateActor<TActor>();
            var actorSelection = ActorSystem.LocateActor<TActor>();

            return actorSelection;
        }

        protected Type CreateMockActor<TMockActor>(Dictionary<Tuple<Guid, Type>, object> mocks) where TMockActor : ActorBase
        {
            Console.WriteLine("Setting Up Actor " + typeof(TMockActor).Name + " with " + mocks.Count + " items ....");
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
                mockActorState.MockSetUpMessages.Add(new MockSetUpMessage(typeof(TMockActor), mock.Key.Item2, mock.Value));
                Console.WriteLine("When Received is  " + mock.Key.Item2 + " The response will be " + mock.Value);

                builder.RegisterType<IMockActorState>();
                builder.Register<IMockActorState>(c => mockActorState);
                builder.Update(Container);
            }
            var state1 = (MockActorState)Container.Resolve<IMockActorState>();
            Console.WriteLine("State now has " + state1.MockSetUpMessages.Count + " items ....");

            return typeof(TMockActor);
        }

        public ActorReceives<T> ItShouldDo(Action<IUntypedActorContext, Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>> operation)
        {
            Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new ItShouldExecuteLambda(operation));
            return this;
        }
    }

    public class ActorReceives : ActorReceives<object>
    {
        public ActorReceives(IContainer container, ActorSystem actorSystem, Dictionary<Tuple<Guid, Type>, object> mocks = null) : base(container, actorSystem, mocks)
        {
        }
    }
}