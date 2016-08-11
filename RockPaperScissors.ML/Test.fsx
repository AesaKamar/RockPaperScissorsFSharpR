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




let context = new RockPaperScissors.DataAccess.RPSContext()
let Logic = new RockPaperScissors.Logic()

let input = 
    Logic.MyMatchHistoryAsTable "Aesa"

let output = Logic.FromMatchHistoryTableGenerateWinners input

let listOfDecisionVariables =  List.init 14 (fun x -> DecisionVariable.Discrete("", new IntRange(0, 5) ))
let decisionTree: DecisionTree = new DecisionTree(inputs= listOfDecisionVariables.ToList(), classes= 3)


//let rec teach(): unit =
//    match teacher.Run(input, output) with
