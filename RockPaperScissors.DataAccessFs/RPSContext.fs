namespace RockPaperScissors.DataAccess

open ServiceStack.Redis
open ServiceStack.Redis.Generic
open System.Data.Entity
open RockPaperScissors.Data
open System
open System.Configuration

type RPSContext() = 
    inherit DbContext("RPSContext")
    [<DefaultValue>] 
    val mutable matches : DbSet<Match>
    member this.Matches 
        with get() = this.matches 
        and set v = this.matches <- v

type RedisCache()=
    let redisClient = 
        (new RedisClient() :> IRedisClient)
            .As<RockPaperScissors.Data.LearningModel>()
    member this.get     (x: Match) = redisClient.GetById(x.Id)
    member this.store   (x: LearningModel) = redisClient.Store(x)
        

