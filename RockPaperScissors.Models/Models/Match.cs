using System;
using System.ComponentModel.DataAnnotations;

namespace RockPaperScissors.Models
{


    public class Match
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string PlayerName { get; set; }
        [Required]
        public FS.Choice P1Choice { get; set; }
        public FS.Choice P2Choice { get; set; }
        [Range(0,2)]
        public int Winner { get; set; }
        public DateTime Timestamp { get; set; }
    }
}