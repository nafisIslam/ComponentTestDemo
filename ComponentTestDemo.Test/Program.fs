namespace ComponentTestDemo.Test

module Main =
  open Expecto
  open System
  open FSharp.Data

  let allModuleTests = 
    [
       AccountTests.test;
    ]     

  [<EntryPoint>]
  let main argv =
    let printSpacer () = printfn "%s%s" Environment.NewLine Environment.NewLine
    let testConfig = { defaultConfig with parallel=false; verbosity=Expecto.Logging.LogLevel.Debug;  } 

    let result =
      match (allModuleTests |> Seq.map (fun t -> printSpacer (); runTests testConfig t) |> Seq.sum) with
      | 0 -> 0
      | n ->
          printfn "Tests failed with code %d" n
          n

    printfn "Hit enter to exit..."
    Console.ReadLine() |> ignore
    result
