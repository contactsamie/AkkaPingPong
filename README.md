# AkkaPingPong

        [Test]
        public void it_should_do_a_pong_unit1()
        {
            //Arrange
            ActorSystemfactory.ActorSystem.CreateActor<PingPongActor<BlackHoleActor>>();

            //Act
            ActorSystemfactory.ActorSystem.LocateActor<PingPongActor<BlackHoleActor>>().Tell(new PingMessage());
 
            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(20));
        }
