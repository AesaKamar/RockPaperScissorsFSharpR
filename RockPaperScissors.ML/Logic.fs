namespace RockPaperScissors

open RockPaperScissors.Data
open RockPaperScissors.DataAccess
open System.Data.Entity
open System.Data
open System.Linq
open System.Collections.Generic
open System

open Accord
open Accord.Statistics.Models.Regression
open Accord.Statistics.Models.Regression.Fitting


type Logic() =
    /// Defines rules for winning and losing
    member this.Winner a b =
        match a, b with
        | a, b when a = b               -> 0   // Equality ties
        | Choice.Rock, Choice.Scissors  -> 1   // Rock beats Scissors
        | Choice.Paper, Choice.Rock     -> 1   // Paper beats Rock
        | Choice.Scissors, Choice.Paper -> 1   // Scissors beat Paper
        | _ , _                         -> 2   // Else P2 wins


    //Use IRedisClient.GetTypedClient<HistoryRecord> to speed up these 2 operations in cache
    member this.MakeMatchHistoryByFastest (rpsMatch: Match)=
        let context = new RPSContext()
        ignore

    //Constructs an array with the previous 7 matches
    member this.GetMatchHistory (rpsMatch: Match) =
        let context = new RPSContext()
        context.Matches
            .Where(fun x -> x.PlayerName = rpsMatch.PlayerName && x.Timestamp <= rpsMatch.Timestamp)
            .OrderByDescending(fun x -> x.Timestamp)
            .Take(8).ToArray()



    //Validates we conly create match frames where there are 7 valid historical games
    member this.CreateMatchHistoryArray (matchHistoryArray8 : Match array)=
        match matchHistoryArray8 with
        | [| p7;p6;p5;p4;p3;p2;p1;t0 |] -> 
                                            let learningRecord : int array=
                                                [| 
                                                    int p7.P1Choice;int p7.P2Choice;
                                                    int p6.P1Choice;int p6.P2Choice;
                                                    int p5.P1Choice;int p5.P2Choice;
                                                    int p4.P1Choice;int p4.P2Choice;
                                                    int p3.P1Choice;int p3.P2Choice;
                                                    int p2.P1Choice;int p2.P2Choice;
                                                    int p1.P1Choice;int p1.P2Choice;
                                                    int t0.P1Choice
                                                |]
                                            Some(learningRecord)
        | x -> None


    //When supplied a player name, this gets a historical view of all games with a frame of past 7 games
    member this.MyMatchHistoryAsTable (playerName : string) =
        let context = new RPSContext()
        context.Matches.Where(fun x -> x.PlayerName = playerName).OrderByDescending(fun x -> x.Timestamp).ToArray()
        |> Array.map (fun x -> x 
                                |> this.GetMatchHistory 
                                |> this.CreateMatchHistoryArray)
        |> Array.choose id


    member this.FromMatchHistoryTableGenerateWinners (table : int [][] ) = 
        table 
        |> Array.map(fun x -> x.[14])