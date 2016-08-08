namespace RockPaperScissors

open RockPaperScissors.Data
open RockPaperScissors.DataAccess
open System.Data.Entity
open System.Data
open System.Linq
open System.Collections.Generic
open System
open RDotNet
open RProvider
open RProvider.graphics
open RProvider.stats
open RProvider.utils
open RDotNet
open RProvider
open RProvider.``base``
open RProvider.datasets
open RProvider.caret
open Deedle

type Logic() = 
    do R.set_seed("314159") |> ignore

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


    //Converts Match History to a single record
    member this.CreateMatchHistoryRecord (matchHistoryArray8 : Match array)=
        match matchHistoryArray8 with
        //TODO make a type that is the ML learnign record
        | [| p7;p6;p5;p4;p3;p2;p1;t0 |] -> 
                                            let learningRecord : Data.LearningModel=
                                                {
                                                    Prior7P1Choice  = int p7.P1Choice; Prior7Winner    = p7.Winner;
                                                    Prior6P1Choice  = int p6.P1Choice; Prior6Winner    = p6.Winner;
                                                    Prior5P1Choice  = int p5.P1Choice; Prior5Winner    = p5.Winner;
                                                    Prior4P1Choice  = int p4.P1Choice; Prior4Winner    = p4.Winner;
                                                    Prior3P1Choice  = int p3.P1Choice; Prior3Winner    = p3.Winner;
                                                    Prior2P1Choice  = int p2.P1Choice; Prior2Winner    = p2.Winner;
                                                    Prior1P1Choice  = int p1.P1Choice; Prior1Winner    = p1.Winner;
                                                    ThisP1Choice    = int t0.P1Choice;
                                                }
                                            Some(learningRecord)
        | x -> None


    //Converts an array of matches into an R Data Frame
    member this.MatchRecordsToRDataFrame (playerName : string) =
        let context = new RPSContext()
        context.Matches.Where(fun x -> x.PlayerName = playerName).OrderByDescending(fun x -> x.Timestamp).ToArray()
        |> Seq.map (fun x -> x 
                                |> this.GetMatchHistory 
                                |> this.CreateMatchHistoryRecord)
        |> Seq.choose id
        |> Frame.ofRecords
        |> R.as_data_frame

