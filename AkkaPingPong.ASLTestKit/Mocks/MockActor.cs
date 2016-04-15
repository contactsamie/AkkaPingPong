using Akka.Actor;
using Akka.DI.Core;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit.Messages;
using AkkaPingPong.ASLTestKit.Models;
using AkkaPingPong.ASLTestKit.State;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AkkaPingPong.ASLTestKit.Mocks
{
    public class MockMessages
    {
        public MockMessages(string actorPath, Type message)
        {
            Message = message;
            ActorPath = actorPath;
        }

        public string ActorPath { private set; get; }

        public Type Message { private set; get; }
    }

    public class MockMessagesQueryActor : ReceiveActor
    {
        private Dictionary<Guid, MockMessages> ReceivedMessages { set; get; }

        public MockMessagesQueryActor()
        {
            ReceivedMessages = new Dictionary<Guid, MockMessages>();
            Receive<GetAllPreviousMessagesReceivedByMockActor>(message =>
            {
                Console.WriteLine(GetType().Name + " Responding to request  for all received messages totaling :  " + ReceivedMessages.Count);
                Sender.Tell(ReceivedMessages);
            });
            Receive<MockMessages>((message) =>
            {
                Console.WriteLine(GetType().Name + ">>>>>>>> Receiving message from  " + message.ActorPath + " : > " + message.Message);

                ReceivedMessages.Add(Guid.NewGuid(), message);
            });
        }
    }

    public abstract class MockActorBase : StatefullReceiveActor<MockActorState>
    {
        protected Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors> InjectedActors { set; get; }
        private ActorSelection MockMessagesQueryActorSelection { set; get; }

        protected MockActorBase(IMockActorState state)
        {
            MockMessagesQueryActorSelection = Context.System.LocateActor<MockMessagesQueryActor>();

            WriteLine(GetType().Name + " HAS STARTED RUNNING ....");

            SetState(state as MockActorState);

            ReceiveAny(message =>
            {
                try
                {
                    WriteLine(GetType().Name + " Receiving message " + message);

                    //   MockMessagesQueryActorSelection.Tell(new MockMessages(Context.Self.ToActorMetaData().Path,message));
                    var myState = GetState();
                    var results =
                        myState.MockSetUpMessages.Where(s => s.WhenInComing == message.GetType() && s.Owner == GetType()).ToList();

                    foreach (var response in results)
                    {
                        if (response == null) return;

                        MockMessagesQueryActorSelection.Tell(new MockMessages(Context.Self.ToActorMetaData().Path, response.WhenInComing));

                        HandleMessage(response.RespondWith, response);
                        //  WriteLine(GetType().Name + " Receiving message .... Process " + response.RespondWith + " for message " + response);
                    }
                }
                catch (Exception e)
                {
                    WriteLine("Exception has been thrown inside Receive of  " + GetType().Name + " : " + e.Message);
                    throw e;
                }
            });
        }

        private void WriteLine(string message)
        {
            Console.WriteLine("<<<<<Message From :    " + GetType().Name);

            // Console.WriteLine("__________________________________________________________");
            Console.WriteLine(message);
        }

        protected void Initialize()
        {
            try
            {
                WriteLine(GetType().Name + " Initializing ....");
                var result = GetState().MockSetUpMessages.Where(s => s.WhenInComing == typeof(MockActorInitializationMessage) && s.Owner == GetType()).ToList();
                if (!result.Any()) return;

                foreach (var response in result)
                {
                    if (response == null) return;
                    HandleMessage(response.RespondWith, response);
                    WriteLine(GetType().Name + " Initializing ....Process " + response.RespondWith + " for message " + response);
                }
            }
            catch (Exception e)
            {
                WriteLine("Exception has been thrown during initialization of  " + GetType().Name + " : " + e.Message);
                throw e;
            }
        }

        private bool ExecuteLambda(object response, object message)
        {
            bool handled = false;
            if (response is ItShouldExecuteLambda)
            {
                handled = true;
                var forwarding = response as ItShouldExecuteLambda;
                forwarding.Operation(Context, InjectedActors);
            }
            return handled;
        }

        private void HandleMessage(object response, object message)
        {
            var handled = HandleMockThrowExceptionMessage(response);
            if (!handled)
                handled = HandleTellAnotherActorMessage(response, message);
            if (!handled)
                handled = HandleTellAnotherActorTypeMessage(response, message);
            if (!handled)
                handled = HandleTellAnotherRefActorMessage(response, message);
            if (!handled)
                handled = HandleForwardToChildActorTypeMessage(response, message);
            if (!handled)
                handled = HandleTellChildActorTypeMessage(response, message);
            if (!handled)
                handled = HandleMockCreateChildActor(response);
            if (!handled)
                handled = ExecuteLambda(response, message);

            if (!handled && Sender != null)
            {
                Sender.Tell(response);
            }
        }

        private bool HandleTellAnotherRefActorMessage(object response, object message)
        {
            bool handled = false;
            if (response is TellAnotherRefActorMessage)
            {
                handled = true;
                var forwarding = response as TellAnotherRefActorMessage;
                forwarding.ActorRef.Tell(forwarding.Message ?? message, Sender);
            }
            return handled;
        }

        private bool HandleTellAnotherActorTypeMessage(object response, object message)
        {
            if (!(response is TellAnotherActorTypeMessage)) return false;

            var forwarding = response as TellAnotherActorTypeMessage;
            Context.System.LocateActor(forwarding.ActorType, forwarding.Parent).Tell(forwarding.Message ?? message, Sender);
            return true;
        }

        private bool HandleTellAnotherActorMessage(object response, object message)
        {
            if (!(response is TellAnotherActorMessage)) return false;

            var forwarding = response as TellAnotherActorMessage;
            Context.System.LocateActor(forwarding.Actor).Tell(forwarding.Message ?? message, Sender);
            return true;
        }

        private static bool HandleMockThrowExceptionMessage(object response)
        {
            if (!(response is MockThrowExceptionMessage)) return false;

            var ex = response as MockThrowExceptionMessage;
            var exception = ex.Exception ?? new Exception();
            throw exception;
        }

        private bool HandleMockCreateChildActor(object response)
        {
            if (!(response is MockCreateChildActorMessage)) return false;

            var forwarding = response as MockCreateChildActorMessage;
            if (InjectedActors == null) return true;
            if (InjectedActors.Item1 != null && InjectedActors.Item1.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item1.ActorRef = CreateChildActor(InjectedActors.Item1.ActorType, forwarding);
            }
            if (InjectedActors.Item2 != null && InjectedActors.Item2.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item2.ActorRef = CreateChildActor(InjectedActors.Item2.ActorType, forwarding);
            }
            if (InjectedActors.Item3 != null && InjectedActors.Item3.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item3.ActorRef = CreateChildActor(InjectedActors.Item3.ActorType, forwarding);
            }
            if (InjectedActors.Item4 != null && InjectedActors.Item4.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item4.ActorRef = CreateChildActor(InjectedActors.Item4.ActorType, forwarding);
            }
            return true;
        }

        private bool HandleTellChildActorTypeMessage(object response, object message)
        {
            if (!(response is TellChildActorTypeMessage)) return false;

            var forwarding = response as TellChildActorTypeMessage;

            if (InjectedActors == null) return true;
            if (InjectedActors.Item1 != null && InjectedActors.Item1.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item1.ActorRef.Tell(forwarding.Message ?? message);
            }
            if (InjectedActors.Item2 != null && InjectedActors.Item2.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item2.ActorRef.Tell(forwarding.Message ?? message);
            }
            if (InjectedActors.Item3 != null && InjectedActors.Item3.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item3.ActorRef.Tell(forwarding.Message ?? message);
            }
            if (InjectedActors.Item4 != null && InjectedActors.Item4.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item4.ActorRef.Tell(forwarding.Message ?? message);
            }
            return true;
        }

        private bool HandleForwardToChildActorTypeMessage(object response, object message)
        {
            if (!(response is ForwardToChildActorTypeMessage)) return false;

            var forwarding = response as ForwardToChildActorTypeMessage;

            if (InjectedActors == null) return true;
            if (InjectedActors.Item1 != null && InjectedActors.Item1.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item1.ActorRef.Forward(forwarding.Message ?? message);
            }
            if (InjectedActors.Item2 != null && InjectedActors.Item2.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item2.ActorRef.Tell(forwarding.Message ?? message);
            }
            if (InjectedActors.Item3 != null && InjectedActors.Item3.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item3.ActorRef.Tell(forwarding.Message ?? message);
            }
            if (InjectedActors.Item4 != null && InjectedActors.Item4.ActorType == forwarding.ChildActorType)
            {
                InjectedActors.Item4.ActorRef.Tell(forwarding.Message ?? message);
            }
            return true;
        }

        private static IActorRef CreateChildActor(Type actorType, MockCreateChildActorMessage mockCreateChildActorMessage)
        {
            var props = Context.DI().Props(actorType);

            props = SelectableActor.PrepareProps(mockCreateChildActorMessage.Options, props);

            return Context.ActorOf(props, SelectableActor.GetActorNameByType(null, actorType));
        }
    }
}