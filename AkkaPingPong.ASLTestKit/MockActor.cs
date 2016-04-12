using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using AkkaPingPong.ASLTestKit.State;

namespace AkkaPingPong.ASLTestKit
{
    public class MockActor : StatefullReceiveActor<MockActorState>
    {
        public Dictionary<Guid, object> ReceivedMessages { set; get; }

        public MockActor(IMockActorState state)
        {
            ReceivedMessages = new Dictionary<Guid, object>();
            SetState(state as MockActorState);
            Receive<GetAllPreviousMessagesReceivedByMockActor>(message =>
            {
                Sender.Tell(ReceivedMessages);
            });
            ReceiveAny(message =>
            {
                ReceivedMessages.Add(Guid.NewGuid(), message);
                var myState = GetState();
                var result = myState.MockSetUpMessages.Where(s => s.WhenInComing == message.GetType() && s.Owner == GetType()).ToList();
                if (!result.Any()) return;
                var response = result.First().RespondWith;
                var handled = false;
                if (response is MockThrowExceptionMessage)
                {
                    handled = true;
                    var ex = response as MockThrowExceptionMessage;
                    var exception = ex.Exception ?? new Exception();

                    throw exception;
                }
                if (response is ItTellAnotherActorMessage)
                {
                    handled = true;
                    var forwarding = response as ItTellAnotherActorMessage;

                    Context.System.LocateActor(forwarding.Actor).Tell(message, Sender);
                }
                if (response is TellAnotherRefActorMessage)
                {
                    handled = true;
                    var forwarding = response as TellAnotherRefActorMessage;

                    forwarding.ActorRef.Tell(message, Sender);
                }
                if (!handled)
                {
                    Sender.Tell(response);
                }
            });
        }
    }
}