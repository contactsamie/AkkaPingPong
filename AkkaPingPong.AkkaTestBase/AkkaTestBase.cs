using Akka.TestKit.Xunit2;
using AkkaPingPong.ASLTestKit;
using Autofac;
using System;

namespace AkkaPingPong.AkkaTestBase
{
    public abstract class TestKitTestBase : TestKit, IDisposable
    {
        public AkkaMockFactory MockFactory { set; get; }

        protected TestKitTestBase()
        {
            MockFactory = new AkkaMockFactory(new ContainerBuilder().Build(), Sys);
        }

        protected override void Dispose(bool disposing)
        {
            MockFactory.Dispose();
            base.Dispose(disposing);
        }
    }
}