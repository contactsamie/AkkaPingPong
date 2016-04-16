using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit.Messages;
using AkkaPingPong.ASLTestKit.Models;
using AkkaPingPong.ASLTestKit.State;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AkkaPingPong.ASLTestKit.Mocks
{
    public abstract class MockActorBase : StatefullReceiveActor<MockActorState>
    {
        protected Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors> InjectedActors { set; get; }

        protected MockActorBase(IMockActorState state)
        {
            SetState(state as MockActorState);

            ReceiveAny(message =>
            {
                var myState = GetState();
                var results = myState.MockSetUpMessages.Where(s => s.WhenInComing == message.GetType() && s.Owner == GetType()).ToList();

                ProcessMockSetUpMessage(results);
            });
        }

        protected void Initialize()
        {
            var result = GetState().MockSetUpMessages.Where(s => s.WhenInComing == typeof(MockActorInitializationMessage) && s.Owner == GetType()).ToList();

            ProcessMockSetUpMessage(result);
        }

        private void ProcessMockSetUpMessage(List<MockSetUpMessage> result)
        {
            if (!result.Any()) return;
            try
            {
                foreach (var response in result)
                {
                    if (response == null) return;
                    Execute(response.RespondWith, response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception has been thrown during initialization of  " + GetType().Name + " : " + e.Message);
                throw e;
            }
        }

        private bool Execute(object response, object message)
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
    }
}