namespace RockPaperScissors

open RockPaperScissors.Data
open RockPaperScissors.DataAccess
open System.Data.Entity
open System.Data
open System.Linq
open System.Collections.Generic
open System

open Accord
open Accord.MachineLearning.DecisionTrees
open Accord.MachineLearning.DecisionTrees.Learning


type Training() =
    ///Construction
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
        ]                                                             
    let mutable decisionTree = new DecisionTree(inputs= listOfDecisionVariables.ToList(), classes= 3)
    let mutable teacher = new Accord.MachineLearning.DecisionTrees.Learning.ID3Learning(decisionTree)



    //Runs an id3 training algorithm against dataset
    member this.Run inputs outputs = teacher.Run(inputs, outputs)

    //Evaluates the error based upon the current model's training state
    member this.Error inputs outputs= teacher.ComputeError(inputs, outputs)

    //Given an array of match histories, this function will decide the result of computation
    member this.Decide (inputs : int array) = decisionTree.Decide(inputs)
