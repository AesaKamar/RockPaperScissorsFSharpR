//In F# Scripts, we have to manually link all of our DLL dependencies. 
//This is ONLY in scripts, not in managed projects
#r @"./bin/debug/RockPaperScissors.Data.dll"
#r @"./bin/debug/RockPaperScissors.DataAccessFS.dll"
#r @"./bin/debug/RockPaperScissors.Ml.dll"
#r @"./bin/debug/EntityFramework.dll"
#r @"./bin/debug/DynamicInterop.dll"
#r @"./bin/debug/EntityFramework.dll"
#r @"./bin/debug/LinqOptimizer.Base.dll"
#r @"./bin/debug/LinqOptimizer.Core.dll"
#r @"./bin/debug/LinqOptimizer.FSharp.dll"
#r @"./bin/debug/Accord.dll"
#r @"./bin/debug/Accord.Statistics.dll"
#r @"./bin/debug/Accord.Controls.dll"
#r @"./bin/debug/Accord.MachineLearning.dll"

open System.Linq
open System.Data.Entity
open RockPaperScissors.Data
open RockPaperScissors.DataAccessFs
open System
open System.Data

open Accord
open Accord.Statistics.Models.Regression
open Accord.Statistics.Models.Regression.Fitting
open Accord.Controls
open Accord.IO
open Accord.Math
open Accord.Statistics.Distributions.Univariate
open Accord.MachineLearning.Bayes
open Accord.MachineLearning.DecisionTrees
open Accord.MachineLearning.DecisionTrees.Learning




let context = new RockPaperScissors.DataAccess.RPSContext()
let Logic = new RockPaperScissors.Logic()
let Training = new RockPaperScissors.Training()

let inputs = Logic.MyMatchHistoryAsTable "Aesa"
let outputs = Logic.FromMatchHistoryTableGenerateWinners inputs



let inputsMinusLastCol = Logic.FromMatchHistoryTableOmitWinners inputs



let TrainIn = inputsMinusLastCol.Take(20).ToArray()
let TrainOut = outputs.Take(20).ToArray()
let TestIn = inputsMinusLastCol.Skip(Math.Max(0, inputsMinusLastCol.Count() - 20)).ToArray()
let TestOut = outputs.Skip(Math.Max(0, outputs.Count() - 20)).ToArray()


let runner = Training.Run TrainIn TrainOut
let error = Training.Error TestIn TestOut


