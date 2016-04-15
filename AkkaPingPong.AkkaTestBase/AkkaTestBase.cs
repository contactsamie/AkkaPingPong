using Akka.TestKit.Xunit2;
using AkkaPingPong.ASLTestKit;
using Autofac;
using System;

namespace AkkaPingPong.AkkaTestBase
{
    public abstract class TestKitTestBase : TestKit, IDisposable
    {
        public AkkaMockFactory mockFactory { set; get; }

        public TestKitTestBase()
        {
            mockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
        }

        protected override void Dispose(bool disposing)
        {
            mockFactory.Dispose();
            base.Dispose(disposing);
        }
    }
}