using Akka.Actor;
using Akka.DI.Core;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit.Messages;
using AkkaPingPong.ASLTestKit.Mocks;
using AkkaPingPong.ASLTestKit.Models;
using AkkaPingPong.ASLTestKit.State;
using Autofac;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AkkaPingPong.ASLTestKit
{
    public class ActorReceives<T>
    {
        private ConcurrentDictionary<Tuple<Guid, Type>, object> Mocks { set; get; }
        private IContainer Container { set; get; }
        private ActorSystem ActorSystem { set; get; }
        private T ReceivedMessage { set; get; }
        private ConcurrentDictionary<Guid, MockMessages> MessagesReceived { get; }

        public ActorReceives(ConcurrentDictionary<Guid, MockMessages> messagesReceived, IContainer container, ActorSystem actorSystem, ConcurrentDictionary<Tuple<Guid, Type>, object> mocks = null, T receivedMessage = default(T))
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (actorSystem == null) throw new ArgumentNullException(nameof(actorSystem));
            MessagesReceived = messagesReceived;
            ActorSystem = actorSystem;
            Container = container;
            Mocks = mocks ?? new ConcurrentDictionary<Tuple<Guid, Type>, object>();
            ReceivedMessage = receivedMessage;
        }

        public ActorReceives<TT> WhenActorReceives<TT>()
        {
            return new ActorReceives<TT>(MessagesReceived, Container, ActorSystem, Mocks);
        }

        public ActorReceives<MockActorInitializationMessage> WhenActorInitializes()
        {
            return new ActorReceives<MockActorInitializationMessage>(MessagesReceived, Container, ActorSystem, Mocks);
        }

        public ActorReceives<T> ItShouldTellAnotherActor<TA>(object message, ActorMetaData parent = null)
        {
            return ItShouldDo((actorAccess) =>
            {
                actorAccess.Context.System.LocateActor(typeof(TA), parent).Tell(message);
            });
        }

        public ActorReceives<T> ItShouldTellAnotherActor(Type actorType, object message, ActorSelection parent = null)
        {
            return ItShouldDo((actorAccess) =>
            {
                actorAccess.Context.System.LocateActor(actorType, parent).Tell(message);
            });
        }

        public ActorReceives<T> ItShouldTellAnotherActor(IActorRef actorRef, object message = null)
        {
            return ItShouldDo((actorAccess) =>
            {
                actorRef.Tell(message);
            });
        }

        public ActorReceives<T> ItShouldDoNothing()
        {
            return this;
        }

        public ActorReceives<T> ItShouldCreateChildActor(Type childActorType, ActorSetUpOptions options = null)
        {
            return ItShouldDo((actorAccess) =>
            {
                HandleChildActorType(childActorType, actorAccess.ActorChildren, (actor) =>
          {
              actor.ActorRef = CreateChildActor(actorAccess.Context, actor.ActorType, options ?? new ActorSetUpOptions());
          });
            });
        }

        public object Message { get; set; }

        public ActorReceives<T> ItShouldForwardItTo<TTC>(object message, ActorSelection parent = null)
        {
            return ItShouldForwardItTo(typeof(TTC), message, parent);
        }

        public ActorReceives<T> ItShouldForwardItTo(Type actorType, object message, ActorSelection parent = null)
        {
            return ItShouldDo((actorAccess) =>
            {
                var destActor = actorAccess.Context.System.LocateActor(actorType, parent);
                destActor.Tell(message, actorAccess.Context.Sender);
            });
        }

        public ActorReceives<T> ItShouldTellItToChildActor<TTC>(object message)
        {
            return ItShouldTellItToChildActor(typeof(TTC), message);
        }

        public ActorReceives<T> ItShouldTellItToChildActor(Type actorType, object message)
        {
            return ItShouldDo((actorAccess) =>
            {
                HandleChildActorType(actorType, actorAccess.ActorChildren, (actor) =>
               {
                   actor.ActorRef.Tell(message);
               });
            });
        }

        private static IActorRef CreateChildActor(IActorContext context, Type actorType, ActorSetUpOptions options)
        {
            var props = context.DI().Props(actorType);

            props = SelectableActor.PrepareProps(options, props);

            var actorRef = context.ActorOf(props, SelectableActor.GetActorNameByType(null, actorType));
            return actorRef;
        }

        private static void HandleChildActorType(Type childActorType, Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors> injectedActors, Action<InjectedActors> operation)
        {
            if (injectedActors == null) return;
            if (injectedActors.Item1 != null && injectedActors.Item1.ActorType == childActorType)
            {
                operation(injectedActors.Item1);
            }
            if (injectedActors.Item2 != null && injectedActors.Item2.ActorType == childActorType)
            {
                operation(injectedActors.Item2);
            }
            if (injectedActors.Item3 != null && injectedActors.Item3.ActorType == childActorType)
            {
                operation(injectedActors.Item3);
            }
            if (injectedActors.Item4 != null && injectedActors.Item4.ActorType == childActorType)
            {
                operation(injectedActors.Item4);
            }
        }

        public ActorReceives<T> ItShouldForwardItToChildActor(Type actorType, object message)
        {
            return ItShouldDo((actorAccess) =>
            {
                HandleChildActorType(actorType, actorAccess.ActorChildren, (actor) =>
                {
                    actor.ActorRef.Forward(message);
                });
            });
        }

        public ActorReceives<T> ItShouldForwardItTo(IActorRef actorType, object message)
        {
            return ItShouldDo((actorAccess) =>
            {
                actorType.Forward(message);
            });
        }

        public ActorReceives<T> ItShouldTellSender<TResponse>(TResponse response)
        {
            return ItShouldDo((actorAccess) =>
            {
                actorAccess.Context.Sender.Tell(response);
            });
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

        protected Type CreateMockActor<TMockActor>(ConcurrentDictionary<Tuple<Guid, Type>, object> mocks) where TMockActor : ActorBase
        {
            ItShouldDo((actorAccess) => MessagesReceived.GetOrAdd(Guid.NewGuid(), new MockMessages(actorAccess.Context.Self.ToActorMetaData().Path, typeof(T))));

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


        public ActorReceives<T> ItShouldDo(Action operation)
        {

            Action<ActorAccess> op =(a) => { operation(); };

            Mocks.GetOrAdd(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new ItShouldExecuteLambda(op));
            return this;
        }

      
        public ActorReceives<T> ItShouldDo(Action<ActorAccess> operation)
        {

            Action<ActorAccess> op =(a) => { operation(a); };

            Mocks.GetOrAdd(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new ItShouldExecuteLambda(op));
            return this;
        }

      
    }

    public class ActorReceives : ActorReceives<object>
    {
        public ActorReceives(ConcurrentDictionary<Guid, MockMessages> messagesReceived, IContainer container, ActorSystem actorSystem, ConcurrentDictionary<Tuple<Guid, Type>, object> mocks = null) : base(messagesReceived, container, actorSystem, mocks)
        {
        }
    }
}