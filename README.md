# AkkaPingPong
    
     [<Test>]     
      member public  this.it_should_do_a_pong() =
        //Arrange
        this.ActorSystem.CreateActor<PingPongActor<BlackHoleActor>>()  |> ignore
        //Act
        new PingMessage() |> this.ActorSystem.LocateActor(typedefof<PingPongActor<_>>).Tell  |> ignore
        //Assert
        this.AwaitAssert  (fun m->this.ExpectMsg<PingMessageCompleted>()|> ignore),TimeSpan.FromSeconds(5.0) |> ignore


        [Test]
        public void it_should_do_a_pong()
        {
            //Arrange
            ActorSystemfactory.ActorSystem.CreateActor<PingPongActor<BlackHoleActor>>();

            //Act
            ActorSystemfactory.ActorSystem.LocateActor<PingPongActor<BlackHoleActor>>().Tell(new PingMessage());
 
            //Assert
            AwaitAssert(() => ExpectMsg<PingMessageCompleted>(), TimeSpan.FromSeconds(20));
        }
