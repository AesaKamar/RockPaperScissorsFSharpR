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


            var Confidence = .33;
            Choice AIChoice = Choice.Rock;

            if(Confidence <= .33)
            {
                Array Values = Enum.GetValues(typeof(Choice));
                Random Random = new Random();
                AIChoice = (Choice)Values.GetValue(Random.Next(Values.Length));
            }
            Match.P2Choice = AIChoice;

            using (var context = new RPSContext())
            {
                return Ok(CreateMatch(Match, context));
            }
        }
    }
}
