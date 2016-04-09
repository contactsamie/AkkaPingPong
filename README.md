# AkkaPingPong

        [Test]
        public void it_should_do_a_pong()
        {
            //Arrange
            ActorSystem.CreateActor<PingPongActor<BlackHoleActor>>();

            //Act
            ActorSystem.LocateActor(typeof(PingPongActor<>)).Tell(new PingMessage());

            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(5));
        }
