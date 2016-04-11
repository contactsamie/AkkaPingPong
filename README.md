# AkkaPingPong

F#
     [<Test>]     
     member public this.``it should do a pong``() =
        //Arrange
        this.ActorSystem.CreateActor<PingPongActor<BlackHoleActor>>()  |> ignore
        //Act
        new PingMessage() |> this.ActorSystem.LocateActor(typedefof<PingPongActor<_>>).Tell  |> ignore
        //Assert
        let result = this.AwaitAssert  (fun ()->this.ExpectMsg<PingMessageCompleted>() |> ignore),TimeSpan.FromSeconds(5.0) 
        ()

        
C#       
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
