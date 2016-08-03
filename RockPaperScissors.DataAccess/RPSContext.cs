using System.Data.Entity;


namespace RockPaperScissors.DataAccess
{
    public class RPSContext : DbContext
    {
        public RPSContext() : base("_Default")
        {

        }
        public DbSet<Data.Match> Matches { get; set; }
    }
}