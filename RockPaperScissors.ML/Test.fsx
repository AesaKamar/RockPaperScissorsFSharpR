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

let inputs = 
    Logic.MyMatchHistoryAsTable "Aesa"

let outputs = Logic.FromMatchHistoryTableGenerateWinners inputs

//Console.WriteLine(inputs)
//Console.WriteLine(outputs)

let listOfDecisionVariables =  
    [
        DecisionVariable.Discrete("Prior7P1Choice", new IntRange(0, 2));
        DecisionVariable.Discrete("Prior7Winner"  , new IntRange(0, 2));
        DecisionVariable.Discrete("Prior6P1Choice", new IntRange(0, 2));
        DecisionVariable.Discrete("Prior6Winner"  , new IntRange(0, 2));
        DecisionVariable.Discrete("Prior5P1Choice", new IntRange(0, 2));
        DecisionVariable.Discrete("Prior5Winner"  , new IntRange(0, 2));
        DecisionVariable.Discrete("Prior4P1Choice", new IntRange(0, 2));
        DecisionVariable.Discrete("Prior4Winner"  , new IntRange(0, 2));
        DecisionVariable.Discrete("Prior3P1Choice", new IntRange(0, 2));
        DecisionVariable.Discrete("Prior3Winner"  , new IntRange(0, 2));
        DecisionVariable.Discrete("Prior2P1Choice", new IntRange(0, 2));
        DecisionVariable.Discrete("Prior2Winner"  , new IntRange(0, 2));
        DecisionVariable.Discrete("Prior1P1Choice", new IntRange(0, 2));
        DecisionVariable.Discrete("Prior1Winner"  , new IntRange(0, 2));
//        DecisionVariable.Discrete("ThisP1Choice"  , new IntRange(0, 2));
    ]                                                             

let decisionTree = new DecisionTree(inputs= listOfDecisionVariables.ToList(), classes= 3)


let teacher = new Accord.MachineLearning.DecisionTrees.Learning.ID3Learning(decisionTree)

let inputsMinusLastCol = 
    inputs |> Array.map (fun x -> x.[0..(x.Length-2)])

//Console.WriteLine(inputsMinusLastCol.[0..5])
//Console.WriteLine(outputs.[0..5])

let runner = teacher.Run(inputsMinusLastCol.[0..50], outputs.[0..50])

let error = teacher.ComputeError(inputsMinusLastCol.[50..70], outputs.[50..70])


