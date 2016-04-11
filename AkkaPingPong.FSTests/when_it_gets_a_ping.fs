namespace AkkaPingPong.FSTests

open Akka.TestKit.TestActors
open AkkaPingPong.ActorSystemLib
open AkkaPingPong.Core.Actors
open AkkaPingPong.Core.Messages
open AkkaPingPong.AkkaTestBase
open NUnit.Framework
open System

[<TestFixture>]
type public when_it_gets_a_ping() = 
  inherit AkkaTestBase() 

     [<Test>]     
     member public this.``it should do a pong``() =
        //Arrange
        this.ActorSystem.CreateActor<PingPongActor<BlackHoleActor>>()  |> ignore
        //Act
        new PingMessage() |> this.ActorSystem.LocateActor<PingPongActor<_>>().Tell  |> ignore
        //Assert
        let result = this.AwaitAssert  (fun ()->this.ExpectMsg<PingMessageCompleted>() |> ignore),TimeSpan.FromSeconds(5.0) 
        ()

     [<Test>]    
     member public this.``it should do a pong integration``() =
        //Arrange
        this.ActorSystem.CreateActor<PingPongActor<PingCoordinatorActor<PingActor, PingBlockingActor>>>() |> ignore
        //Act
        [1..10] |> List.iter (fun i->
                               System.Threading.Thread.Sleep(1000) |> ignore
                               new PingMessage() |> this.ActorSystem.LocateActor<PingPongActor<_>>().Tell 
                               )
        //Assert
        let result = this.AwaitAssert  (fun ()->this.Subscriber.ExpectMsg<PongMessage>() |> ignore),TimeSpan.FromSeconds(20.0)
        ()

     [<Test>]    
     member public this.``it should do a pong unit1``() =
        //Arrange
        this.ActorSystem.CreateActor<PingPongActor<BlackHoleActor>>() |> ignore
        //Act
        [1..10] |> List.iter (fun i->
                               System.Threading.Thread.Sleep(1000) |> ignore
                               new PingMessage() |> this.ActorSystem.LocateActor<PingPongActor<_>>().Tell
                               )
        //Assert
        let result = this.AwaitAssert  (fun m->this.ExpectMsg<PingMessageCompleted>() |> ignore),TimeSpan.FromSeconds(20.0) 
        ()

     [<Test>]    
     member public this.``it should do a pong unit2``() =
        //Arrange
        this.ActorSystem.CreateActor<PingCoordinatorActor<AkkaTestBase.BlackHoleActor1, AkkaTestBase.BlackHoleActor2>>()  |> ignore
        //Act
        new PingMessage() |> this.ActorSystem.LocateActor<PingCoordinatorActor<_,_>>().Tell  |> ignore
        //Assert
        let result = this.AwaitAssert  (fun ()->this.ExpectMsg<SorryImStashing>() |> ignore),TimeSpan.FromSeconds(5.0) 
        ()