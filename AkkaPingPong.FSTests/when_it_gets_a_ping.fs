namespace AkkaPingPong.FSTests

open Akka.TestKit
open Akka.TestKit.Xunit2
open Akka.TestKit.TestActors
open AkkaPingPong.ActorSystemLib
open AkkaPingPong.Core.Actors
open AkkaPingPong.Core.Messages
open AkkaPingPong.AkkaTestBase
open AkkaPingPong.ASLTestKit
open AkkaPingPong.ASLTestKit.Mocks
open Xunit;
open System
open Autofac

type public when_it_gets_a_ping() =
  inherit TestKitTestBase()

     [<Fact>]
     member public this.``it should do a pong``() =
        //Arrange

        this.MockFactory.CreateActor<PingPongActor<MockActor>>()  |> ignore
        //Act
        new PingMessage() |> this.MockFactory.LocateActor<PingPongActor<_>>().Tell  |> ignore
        //Assert
        let result = this.AwaitAssert  (fun ()->this.ExpectMsg<PingMessageCompleted>() |> ignore),TimeSpan.FromSeconds(5.0)
        ()

     [<Fact>]
     member public this.``it should do a pong integration``() =
        //Arrange

        this.MockFactory.CreateActor<PingPongActor<PingCoordinatorActor<PingActor, PingBlockingActor>>>() |> ignore
        //Act
        [1..10] |> List.iter (fun i->
                               System.Threading.Thread.Sleep(1000) |> ignore
                               new PingMessage() |> this.MockFactory.LocateActor<PingPongActor<_>>().Tell
                               )
        //Assert
        let result = this.AwaitAssert  (fun ()->this.ExpectMsg<PongMessage>() |> ignore),TimeSpan.FromSeconds(20.0)
        ()

     [<Fact>]
     member public this.``it should do a pong unit1``() =
        //Arrange

        this.MockFactory.CreateActor<PingPongActor<MockActor>>() |> ignore
        //Act
        [1..10] |> List.iter (fun i->
                               System.Threading.Thread.Sleep(1000) |> ignore
                               new PingMessage() |> this.MockFactory.LocateActor<PingPongActor<_>>().Tell
                               )
        //Assert
        let result = this.AwaitAssert  (fun m->this.ExpectMsg<PingMessageCompleted>() |> ignore),TimeSpan.FromSeconds(20.0)
        ()

     [<Fact>]
     member public this.``it should do a pong unit2``() =
        //Arrange

        this.MockFactory.CreateActor<PingCoordinatorActor<MockActor1, MockActor2>>()  |> ignore
        //Act
        new PingMessage() |> this.MockFactory.LocateActor<PingCoordinatorActor<_,_>>().Tell  |> ignore
        //Assert
        let result = this.AwaitAssert  (fun ()->this.ExpectMsg<SorryImStashing>() |> ignore),TimeSpan.FromSeconds(5.0)
        ()