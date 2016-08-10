#I "../packages/RProvider.1.1.20"
#load "../packages/RProvider.1.1.20/RProvider.fsx"
#load "../packages/Deedle.1.2.5/Deedle.fsx"
#r @"./bin/debug/RockPaperScissors.Data.dll"
#r @"./bin/debug/RockPaperScissors.DataAccessFS.dll"
#r @"./bin/debug/RockPaperScissors.Ml.dll"
#r @"../../packages/EntityFramework.6.1.3/lib/net45/EntityFramework.dll"
#r @"./bin/debug/Deedle.dll"
#r @"./bin/debug/Deedle.RProvider.Plugin.dll"
#r @"./bin/debug/DynamicInterop.dll"
#r @"./bin/debug/RProvider.Runtime.dll"
#r @"./bin/debug/EntityFramework.dll"
#r @"./bin/debug/LinqOptimizer.Base.dll"
#r @"./bin/debug/LinqOptimizer.Core.dll"
#r @"./bin/debug/LinqOptimizer.FSharp.dll"

open RDotNet
open RProvider
open RProvider.``base``
open RProvider.datasets
open RProvider.neuralnet
open RProvider.caret
open System.Linq
open Deedle
open System.Data.Entity
open RockPaperScissors.Data
open RockPaperScissors.DataAccessFs

R.set_seed("314159") |> ignore

let context = new RockPaperScissors.DataAccess.RPSContext()
let Logic = new RockPaperScissors.Logic()


let AllUserDataFrame = Logic.MatchRecordsToRDataFrame "Aesa"

let inTrainRows = caret.R.createDataPartition(y=AllUserDataFrame.AsList().["ThisP1Choice"], p=0.70, list=true)

let testDeedleFrame : Frame<string, string> = inTrainRows.GetValue()

//let testData = R.subset(x=AllUserDataFrame, paramArray= (inTrainRows.AsList() ))
//let testDate = AllUserDataFrame.AsList().["-inTrainRows"]

//let proportion = R.nrow(inTrainRows)/(R.nrow(AllUserData))