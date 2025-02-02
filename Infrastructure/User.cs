using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    [Table("User")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "bigint")]
        public long UserId { get; set; }

        [Column, StringLength(255)]
        public string UserName { get; set; }

        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty(nameof(MatchHistory.Player1))]
        public virtual ICollection<MatchHistory> ListPlayer1 { get; set; } = new HashSet<MatchHistory>();

        [InverseProperty(nameof(MatchHistory.Player2))]
        public virtual ICollection<MatchHistory> ListPlayer2 { get; set; } = new HashSet<MatchHistory>();

        [InverseProperty(nameof(MatchHistory.Winner))]
        public virtual ICollection<MatchHistory> ListWinner { get; set; } = new HashSet<MatchHistory>();

        [InverseProperty(nameof(GameTransactions.FromUser))]
        public virtual ICollection<GameTransactions> ListFromUser { get; set; } = new HashSet<GameTransactions>();

        [InverseProperty(nameof(GameTransactions.ToUser))]
        public virtual ICollection<GameTransactions> ListToUser { get; set; } = new HashSet<GameTransactions>();
    }
}
