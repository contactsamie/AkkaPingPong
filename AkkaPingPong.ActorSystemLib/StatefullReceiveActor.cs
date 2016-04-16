using Akka.Actor;
using Akka.Event;
using AkkaPingPong.ActorSystemLib.StateMessages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AkkaPingPong.ActorSystemLib
{
    public abstract class StatefullReceiveActor : StatefullReceiveActor<object>
    {
    }

    public abstract class StatefullReceiveActor<T> : ReceiveActor where T : new()
    {
        private Dictionary<Guid, Tuple<T, DateTime>> StateHistory { set; get; }

        private bool RecordActorStateHistory { get; set; }

        protected StatefullReceiveActor()
        {
            RecordActorStateHistory = false;
            SetState(new T());
            Receive<GetActorStateMessage>(message =>
            {
                Sender.Tell(CurrentState);
            });
            Receive<GetActorStateHistoryMessage>(message =>
            {
                Sender.Tell(StateHistory);
            });
            Receive<RecordActorStateHistoryMessage>(message =>
            {
                RecordActorStateHistory = true;
            });
            Receive<StopRecordActorStateHistoryMessage>(message =>
            {
                RecordActorStateHistory = false;
            });
            Receive<ClearRecordActorStateHistoryMessage>(message =>
            {
                StateHistory = new Dictionary<Guid, Tuple<T, DateTime>>();
            });
        }

        public bool HasStateChange()
        {
            return JsonConvert.SerializeObject(CurrentState) == JsonConvert.SerializeObject(PreviousState);
        }

        public void SetState(T newState)
        {
            PreviousState = CurrentState;
            CurrentState = newState;
            if (!RecordActorStateHistory) return;
            StateHistory = StateHistory ?? new Dictionary<Guid, Tuple<T, DateTime>>();
            StateHistory.Add(Guid.NewGuid(), new Tuple<T, DateTime>(newState, DateTime.UtcNow));
        }

        public T GetState()
        {
            return CurrentState;
        }

        private  T PreviousState { set; get; }
        private  T CurrentState { set; get; }

        public readonly ILoggingAdapter Logger = Context.GetLogger();

        protected override void PostRestart(Exception reason)
        { }

        protected override void PreRestart(Exception reason, object message)
        {
            // Keep the call to PostStop(), but no stopping of children
            PostStop();
        }
    }
}