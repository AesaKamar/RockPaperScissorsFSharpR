namespace RockPaperScissors.Data

open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open System.Data.Entity
open System.Data
open Newtonsoft.Json

/// Union of possible choices for a round of rock-paper-scissors
type Choice =
    | Rock      = 0
    | Paper     = 1
    | Scissors  = 2

//Create a category which is a record type for persistance
[<CLIMutable>]
[<JsonObject(MemberSerialization=MemberSerialization.OptIn)>]
type public Match =
    {
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>][<Required>][<Key>]
        mutable Id: int;
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>][<Required>]                
        mutable PlayerName: string;
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>][<Required>]                
        mutable P1Choice: Choice;
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>][<Required>]                
        mutable P2Choice: Choice;
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>][<Required>][<Range(0,2)>]  
        mutable Winner: int;
            [<JsonProperty(NullValueHandling = NullValueHandling.Ignore)>][<Required>]                
        mutable Timestamp: System.DateTime;
    }

//Use REDIS Cache to save these
type public LearningModel =
    {
        [<Range(0,2)>]      Prior7P1Choice: int;
        [<Range(0,2)>]      Prior7Winner:   int;
        [<Range(0,2)>]      Prior6P1Choice: int;
        [<Range(0,2)>]      Prior6Winner:   int;
        [<Range(0,2)>]      Prior5P1Choice: int;
        [<Range(0,2)>]      Prior5Winner:   int;
        [<Range(0,2)>]      Prior4P1Choice: int;
        [<Range(0,2)>]      Prior4Winner:   int;
        [<Range(0,2)>]      Prior3P1Choice: int;
        [<Range(0,2)>]      Prior3Winner:   int;
        [<Range(0,2)>]      Prior2P1Choice: int;
        [<Range(0,2)>]      Prior2Winner:   int;
        [<Range(0,2)>]      Prior1P1Choice: int;
        [<Range(0,2)>]      Prior1Winner:   int;
        [<Range(0,2)>]      ThisP1Choice:   int;
    }
module DataConstants = 
    let ColumnHeaders = 
        [|
            "Prior7P1Choice" ;
            "Prior7Winner"   ;
            "Prior6P1Choice" ;
            "Prior6Winner"   ;
            "Prior5P1Choice" ;
            "Prior5Winner"   ;
            "Prior4P1Choice" ;
            "Prior4Winner"   ;
            "Prior3P1Choice" ;
            "Prior3Winner"   ;
            "Prior2P1Choice" ;
            "Prior2Winner"   ;
            "Prior1P1Choice" ;
            "Prior1Winner"   ;
            "ThisP1Choice"   ;
        |]