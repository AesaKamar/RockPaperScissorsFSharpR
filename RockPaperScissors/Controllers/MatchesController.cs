using System;
using System.Linq;
using System.Web.Http;
using RockPaperScissors.Data;
using RockPaperScissors.DataAccess;
using Newtonsoft.Json.Linq;

namespace RockPaperScissors.Controllers
{
    public class MatchesController : ApiController
    {
        private Logic Logic = new Logic();
        private Match CreateMatch(Match Match, RPSContext context)
        {
            Match.Winner = Logic.Winner(Match.P1Choice, Match.P2Choice);
            Match.Timestamp = DateTime.Now;
            var res = context.Matches.Add(Match);
            context.SaveChanges();
            return res;
        }

        //===================
        // GET api/matches
        //===================
        [HttpGet]
        [Route("api/matches")]
        public IHttpActionResult GetMatches([FromUri] string PlayerName = "")
        {
            using (var context = new RPSContext())
            {
                var res = context.Matches
                    .Where(x => x.PlayerName == PlayerName)
                    .OrderBy(x => x.Timestamp)
                    .Take(50).ToList();
                return Ok(res);
            }
        }

        //===================
        // POST api/matches
        //===================
        [HttpPost]
        [Route("api/matches/")]
        public IHttpActionResult Shoot([FromBody] object input)
        {
            var Match = (input as JObject).ToObject<Match>();
            if (Match.PlayerName == null)
                return BadRequest("PlayerName is invalid");

            //Initialize some stuff
            Choice AIChoice = new Choice();
            Match.Timestamp = DateTime.Now;

            //Set up our data for training
            var MatchHistoryTable = Logic.MyMatchHistoryAsTable(Match.PlayerName);
            var InTrain = Logic.FromMatchHistoryTableOmitWinners(MatchHistoryTable);
            var OutTrain = Logic.FromMatchHistoryTableGenerateWinners(MatchHistoryTable);
 

            //Give it a random Calue if we don't have enough training data
            if(InTrain.Length <= 7)
            {
                Array Values = Enum.GetValues(typeof(Choice));
                Random Random = new Random();
                AIChoice = (Choice)Values.GetValue(Random.Next(Values.Length));
            }
            else
            {
                //Train a new Model
                var Training = new Training();
                Training.Run(InTrain, OutTrain);

                //Get the most recent match frame and make sure we put in this match
                var ThisFrame = Logic.MostRecentMatchFrame(Match);

                //Decide on the AI choice
                var PredictedPlayerChoice = (Choice)Training.Decide(ThisFrame);
                AIChoice = Logic.WinAgainst(PredictedPlayerChoice);
            }



            Match.P2Choice = AIChoice;

            using (var context = new RPSContext())
            {
                return Ok(CreateMatch(Match, context));
            }
        }
    }
}
