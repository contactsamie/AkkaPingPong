namespace AkkaPingPong.FSClient

open AkkaPingPong.ActorSystemLib
open AkkaPingPong.Core
open AkkaPingPong.Core.Actors
open AkkaPingPong.Core.Messages
open AkkaPingPong.DependencyLib
open Autofac
open System

 module Program =

   [<EntryPoint>]
   let main argv =
       let container = DependencyResolver.GetContainer()
       let  actorSystemfactory = DependencyResolver.GetContainer().Resolve<IActorSystemFactory>()
       actorSystemfactory.Register(container)
       [1..10] |> List.iter
                            (fun (i)->
                                      System.Threading.Thread.Sleep 1000
                                      new PingMessage() |> actorSystemfactory.ActorSystem.LocateActor<PingPongActor<_>>().Tell
                                      )
       Console.ReadLine |> ignore
       printfn "%s" "i'm done!"
       0