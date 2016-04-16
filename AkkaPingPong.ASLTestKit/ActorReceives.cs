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

        //public ActorReceives<T> ExceptionShouldBeThrown(Exception exception = null)
        //{
        //    Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new MockThrowExceptionMessage(exception));
        //    return this;
        //}

        public ActorReceives<T> ItShouldTellAnotherActor<TA>(object message, ActorMetaData parent = null)
        {
            return ItShouldDo((context, injectedActors) =>
            {
                context.System.LocateActor(typeof(TA), parent).Tell(message);
            });
            //Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellAnotherActorTypeMessage(typeof(TA), message, parent));
            //return this;
        }

        public ActorReceives<T> ItShouldTellAnotherActor(Type actorType, object message, ActorSelection parent = null)
        {
            return ItShouldDo((context, injectedActors) =>
            {
                context.System.LocateActor(actorType, parent).Tell(message);
            });
            //Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellAnotherActorTypeMessage(actorType, message, parent?.ToActorMetaData()));
            //return this;
        }

        public ActorReceives<T> ItShouldTellAnotherActor(IActorRef actorRef, object message = null)
        {
            return ItShouldDo((context, injectedActors) =>
            {
                actorRef.Tell(message);
            });
            //Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellAnotherRefActorMessage(actorRef, message));
            //return this;
        }

        public ActorReceives<T> ItShouldDoNothing()
        {
            return this;
        }

        public ActorReceives<T> ItShouldCreateChildActor(Type childActorType, ActorSetUpOptions options = null)
        {
            return ItShouldDo((context, injectedActors) =>
            {
                HandleChildActorType(childActorType, injectedActors, (actor) =>
          {
              actor.ActorRef = CreateChildActor(context, actor.ActorType, options ?? new ActorSetUpOptions());
          });
            });
            //Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new MockCreateChildActorMessage(childActorType, options));
            //return this;
        }

        private IActorRef CreateChildActor(IUntypedActorContext Context, Type actorType, ActorSetUpOptions Options)
        {
            var props = Context.DI().Props(actorType);

            props = SelectableActor.PrepareProps(Options, props);

            var actorRef = Context.ActorOf(props, SelectableActor.GetActorNameByType(null, actorType));
            return actorRef;
        }

        public object Message { get; set; }

        public ActorReceives<T> ItShouldForwardItTo<TTC>( object message, ActorSelection parent = null)
        {
            return ItShouldForwardItTo(typeof (TTC), parent);
        }

        public ActorReceives<T> ItShouldForwardItTo(Type actorType, object message, ActorSelection parent = null)
        {
            return ItShouldDo((context, injectedActors) =>
            {
                context.System.LocateActor(actorType, parent).Tell(message, context.Sender);
            });

            //Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellAnotherActorTypeMessage(actorType, message, parent?.ToActorMetaData()));
            //return this;
        }

        public ActorReceives<T> ItShouldTellItToChildActor<TTC>(object message)
        {
            return ItShouldTellItToChildActor(typeof(TTC), message);
        }

        public ActorReceives<T> ItShouldTellItToChildActor(Type actorType, object message)
        {
            return ItShouldDo((context, injectedActors) =>
            {
                HandleChildActorType(actorType, injectedActors, (actor) =>
               {
                   actor.ActorRef.Tell(message);
               });
            });
            //Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellChildActorTypeMessage(actorType, message));
            //return this;
        }

        private void HandleChildActorType(Type childActorType, Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors> InjectedActors, Action<InjectedActors> operation)
        {
            if (InjectedActors == null) return;
            if (InjectedActors.Item1 != null && InjectedActors.Item1.ActorType == childActorType)
            {
                operation(InjectedActors.Item1);
            }
            if (InjectedActors.Item2 != null && InjectedActors.Item2.ActorType == childActorType)
            {
                operation(InjectedActors.Item2);
            }
            if (InjectedActors.Item3 != null && InjectedActors.Item3.ActorType == childActorType)
            {
                operation(InjectedActors.Item3);
            }
            if (InjectedActors.Item4 != null && InjectedActors.Item4.ActorType == childActorType)
            {
                operation(InjectedActors.Item4);
            }
        }

        public ActorReceives<T> ItShouldForwardItToChildActor(Type actorType, object message)
        {
            return ItShouldDo((context, injectedActors) =>
            {
                HandleChildActorType(actorType, injectedActors, (actor) =>
                {
                    actor.ActorRef.Forward(message);
                });
            });
            //Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new ForwardToChildActorTypeMessage(actorType, message));
            //return this;
        }

        public ActorReceives<T> ItShouldForwardItTo(IActorRef actorType, object message)
        {
            return ItShouldDo((context, injectedActors) =>
            {
                actorType.Forward(message);
            });
            //Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new TellAnotherRefActorMessage(actorType, message));
            //return this;
        }

        public ActorReceives<T> ItShouldTellSender<TResponse>(TResponse response)
        {
            return ItShouldDo((context, injectedActors) =>
            {
                context.Sender.Tell(response);
            });
            //  Mocks.Add(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), response);
            // return this;
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
            ItShouldDo((context, injectedActors) =>
            {
                MessagesReceived.GetOrAdd(Guid.NewGuid(),
                  new MockMessages(context.Self.ToActorMetaData().Path, typeof(T)));
            });

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
            Mocks.GetOrAdd(new Tuple<Guid, Type>(Guid.NewGuid(), typeof(T)), new ItShouldExecuteLambda(operation));
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