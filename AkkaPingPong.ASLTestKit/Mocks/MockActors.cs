using AkkaPingPong.ASLTestKit.Models;
using AkkaPingPong.ASLTestKit.State;
using System;

namespace AkkaPingPong.ASLTestKit.Mocks

{
    public class MockActor : MockActorBase
    {
        public MockActor(IMockActorState state) : base(state)
        {
            //Initialize();
        }
    }

    public class MockActor<TMockActor, TMockActor2, TMockActor3, TMockActor4> : MockActorBase
    {
        public MockActor(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, new InjectedActors() { ActorType = typeof(TMockActor3) }, new InjectedActors() { ActorType = typeof(TMockActor4) });
            //Initialize();
        }
    }

    public class MockActor<TMockActor, TMockActor2, TMockActor3> : MockActorBase
    {
        public MockActor(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, new InjectedActors() { ActorType = typeof(TMockActor3) }, null);
            //Initialize();
        }
    }

    public class MockActor<TMockActor, TMockActor2> : MockActorBase
    {
        public MockActor(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, null, null);
            //Initialize();
        }
    }

    public class MockActor<TMockActor> : MockActorBase
    {
        public MockActor(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, null, null, null);
            //Initialize();
        }
    }

    public class MockActor4 : MockActorBase
    {
        public MockActor4(IMockActorState state) : base(state)
        {
            //Initialize();
        }
    }

    public class MockActor3<TMockActor, TMockActor2, TMockActor3, TMockActor4> : MockActorBase
    {
        public MockActor3(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, new InjectedActors() { ActorType = typeof(TMockActor3) }, new InjectedActors() { ActorType = typeof(TMockActor4) });
            //Initialize();
        }
    }

    public class MockActor3<TMockActor, TMockActor2, TMockActor3> : MockActorBase
    {
        public MockActor3(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, new InjectedActors() { ActorType = typeof(TMockActor3) }, null);
            //Initialize();
        }
    }

    public class MockActor3<TMockActor, TMockActor2> : MockActorBase
    {
        public MockActor3(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, null, null);
            //Initialize();
        }
    }

    public class MockActor3<TMockActor> : MockActorBase
    {
        public MockActor3(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, null, null, null);
            //Initialize();
        }
    }

    public class MockActor3 : MockActorBase
    {
        public MockActor3(IMockActorState state) : base(state)
        {
            //Initialize();
        }
    }

    public class MockActor2<TMockActor, TMockActor2, TMockActor3, TMockActor4> : MockActorBase
    {
        public MockActor2(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, new InjectedActors() { ActorType = typeof(TMockActor3) }, new InjectedActors() { ActorType = typeof(TMockActor4) });
            //Initialize();
        }
    }

    public class MockActor2<TMockActor, TMockActor2, TMockActor3> : MockActorBase
    {
        public MockActor2(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, new InjectedActors() { ActorType = typeof(TMockActor3) }, null);
            //Initialize();
        }
    }

    public class MockActor2<TMockActor, TMockActor2> : MockActorBase
    {
        public MockActor2(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, null, null);
            //Initialize();
        }
    }

    public class MockActor2<TMockActor> : MockActorBase
    {
        public MockActor2(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, null, null, null);
            //Initialize();
        }
    }

    public class MockActor2 : MockActorBase
    {
        public MockActor2(IMockActorState state) : base(state)
        {
            //Initialize();
        }
    }

    public class MockActor1<TMockActor, TMockActor2, TMockActor3, TMockActor4> : MockActorBase
    {
        public MockActor1(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, new InjectedActors() { ActorType = typeof(TMockActor3) }, new InjectedActors() { ActorType = typeof(TMockActor4) });
            //Initialize();
        }
    }

    public class MockActor1<TMockActor, TMockActor2, TMockActor3> : MockActorBase
    {
        public MockActor1(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, new InjectedActors() { ActorType = typeof(TMockActor3) }, null);
            //Initialize();
        }
    }

    public class MockActor1<TMockActor, TMockActor2> : MockActorBase
    {
        public MockActor1(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, null, null);
            //Initialize();
        }
    }

    public class MockActor1<TMockActor> : MockActorBase
    {
        public MockActor1(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, null, null, null);
            //Initialize();
        }
    }

    public class MockActor1 : MockActorBase
    {
        public MockActor1(IMockActorState state) : base(state)
        {
            //Initialize();
        }
    }

    public class MockActorBase<TMockActor, TMockActor2, TMockActor3, TMockActor4> : MockActorBase
    {
        public MockActorBase(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, new InjectedActors() { ActorType = typeof(TMockActor3) }, new InjectedActors() { ActorType = typeof(TMockActor4) });
            //Initialize();
        }
    }

    public class MockActorBase<TMockActor, TMockActor2, TMockActor3> : MockActorBase
    {
        public MockActorBase(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, new InjectedActors() { ActorType = typeof(TMockActor3) }, null);
            //Initialize();
        }
    }

    public class MockActorBase<TMockActor, TMockActor2> : MockActorBase
    {
        public MockActorBase(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, new InjectedActors() { ActorType = typeof(TMockActor2) }, null, null);
            //Initialize();
        }
    }

    public class MockActorBase<TMockActor> : MockActorBase
    {
        public MockActorBase(IMockActorState state) : base(state)
        {
            InjectedActors = new Tuple<InjectedActors, InjectedActors, InjectedActors, InjectedActors>(new InjectedActors() { ActorType = typeof(TMockActor) }, null, null, null);
            //Initialize();
        }
    }
}