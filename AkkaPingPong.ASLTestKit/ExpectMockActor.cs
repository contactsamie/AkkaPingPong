using Akka.Actor;
using AkkaPingPong.ActorSystemLib;
using AkkaPingPong.ASLTestKit.Messages;
using AkkaPingPong.ASLTestKit.Mocks;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkkaPingPong.ASLTestKit
{
    public class ExpectMockActor
    {
        private Type ActorType { set; get; }
        private IContainer Container { set; get; }
        private ActorSystem ActorSystem { set; get; }
        private ActorMetaData ParentActor { set; get; }
        private ActorMetaData ActorMetaData { set; get; }

        public ExpectMockActor(Type actorType, IContainer container, ActorSystem actorSystem)
        {
            ActorType = actorType;
            Container = container;
            ActorSystem = actorSystem;
        }

        public ExpectMockActor(ActorMetaData actorMetaData, IContainer container, ActorSystem actorSystem)
        {
            ActorMetaData = actorMetaData;
            Container = container;
            ActorSystem = actorSystem;
        }

        public ExpectMockActor(IActorRef actorRef, IContainer container, ActorSystem actorSystem)
        {
            ActorMetaData = actorRef.ToActorMetaData();
            Container = container;
            ActorSystem = actorSystem;
        }

        public List<T> ToHaveReceivedMessage<T>(int maxWaitMilliseconds)
            where T : class
        {
            return ToHaveReceivedMessage<T>(null, maxWaitMilliseconds);
        }

        public List<T> ToHaveReceivedMessage<T>(Func<T, bool> messageValidator = null, int maxWaitMilliseconds = 5000) where T : class
        {
            return AssertAwait(() =>
            {
                List<T> messages;
                try
                {
                    messages = GetAllReceivecMessagesOfType<T>(ActorMetaData == null ? ActorSystem.LocateActor(ActorType, ParentActor) : ActorSystem.LocateActor(ActorMetaData)).Result;
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (PassedValidation(messageValidator, messages))
                {
                    throw new Exception("Expected actor to receive message " + typeof(T).FullName);
                }

                return messageValidator != null ? messages.Where(messageValidator).ToList() : messages;
            }, maxWaitMilliseconds);
        }

        public List<T> NotToHaveReceivedMessage<T>(int maxWaitMilliseconds)
            where T : class
        {
            return NotToHaveReceivedMessage<T>(null, maxWaitMilliseconds);
        }

        public List<T> NotToHaveReceivedMessage<T>(Func<T, bool> messageValidator = null, int maxWaitMilliseconds = 5000) where T : class
        {
            return AssertAwait(() =>
            {
                List<T> messages;
                try
                {
                    messages =
                        GetAllReceivecMessagesOfType<T>(ActorMetaData == null ? ActorSystem.LocateActor(ActorType, ParentActor) : ActorSystem.LocateActor(ActorMetaData)).Result;
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (!PassedValidation(messageValidator, messages))
                {
                    throw new Exception("Expected actor NOT to receive message " + typeof(T).FullName);
                }

                return messageValidator != null ? messages.Where(messageValidator).ToList() : messages;
            }, maxWaitMilliseconds);
        }

        public T AssertAwait<T>(Func<T> action, int durationMilliseconds)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            var now = DateTime.Now;
            var counter = 0;
            var passed = false;
            var lastException = new Exception();
            T result = default(T);
            while ((DateTime.Now - now).TotalMilliseconds < durationMilliseconds)
            {
                try
                {
                    result = action();
                    passed = true;
                    break;
                }
                catch (Exception e)
                {
                    lastException = e;
                }
                System.Threading.Thread.Sleep(10 * counter);
                counter++;
            }
            if (!passed)
            {
                throw new Exception("Could not pass in " + durationMilliseconds + " ms ", lastException);
            }
            return result;
        }

        private static bool PassedValidation<T>(Func<T, bool> messageValidator, List<T> messages) where T : class
        {
            Func<T, bool> defaultValidator = (t) => true;
            messageValidator = messageValidator ?? defaultValidator;
            return !messages.Any(messageValidator);
        }

        protected ActorSelection LocateActor(ActorSelection actor)
        {
            return ActorSystem.LocateActor(actor.ToActorMetaData());
        }

        protected async Task<List<T>> GetAllReceivecMessagesOfType<T>(ActorSelection actorSelection) where T : class
        {
            var t = typeof(T);
            var path = actorSelection.ToActorMetaData().Path;
            var mockMessagesQueryActorSelection = ActorSystem.LocateActor<MockMessagesQueryActor>();
            var messages = await mockMessagesQueryActorSelection.Ask(new GetAllPreviousMessagesReceivedByMockActor(), TimeSpan.FromSeconds(5));
            var m = messages as Dictionary<Guid, MockMessages>;
            var matches = m?.Where(x =>
             {
                 var samePath = x.Value.ActorPath == path;
                 var sameType = x.Value.Message == typeof(T);
                 return samePath && sameType;
             }) ?? new Dictionary<Guid, MockMessages>();
            var result = matches.Select(x => x.Value.Message as T).ToList();
            return result;
        }

        /// <summary>
        /// providing ActorSelection for actor in contest ignores this request, as actor selection implies parent has been provided
        /// </summary>
        public ExpectMockActor WhoseParentIs(IActorRef parentActor)
        {
            ParentActor = parentActor.ToActorMetaData();
            return this;
        }

        /// <summary>
        /// providing ActorSelection for actor in contest ignores this request, as actor selection implies parent has been provided
        /// </summary>
        public ExpectMockActor WhoseParentIs(ActorSelection parentActor)
        {
            ParentActor = parentActor.ToActorMetaData();
            return this;
        }

        /// <summary>
        /// providing ActorSelection for actor in contest ignores this request, as actor selection implies parent has been provided
        /// </summary>
        public ExpectMockActor WhoseParentIs(ActorMetaData parentActor)
        {
            ParentActor = parentActor;
            return this;
        }

        /// <summary>
        /// providing ActorSelection for actor in contest ignores this request, as actor selection implies parent has been provided
        /// </summary>
        public ExpectMockActor WhoseParentIsTheAppliocableGuardianActor()
        {
            return this;
        }
    }
}