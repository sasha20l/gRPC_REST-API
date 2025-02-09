using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    [Table("MatchHistory")]
    public class MatchHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "bigint")]
        public long MatchHistoryId { get; set; }

        [ForeignKey(nameof(Player1))]
        [Column(TypeName = "bigint")]
        public long fkPlayer1Id { get; set; } // Игрок 1

        [ForeignKey(nameof(Player2))]
        [Column(TypeName = "bigint")]
        public long? fkPlayer2Id { get; set; } // Игрок 2; может быть null, если ещё нет второго игрока

        public double Stake { get; set; }

        [ForeignKey(nameof(Winner))]
        [Column(TypeName = "bigint")]
        public long? fkWinnerId { get; set; } // Победитель (может быть null до завершения матча)

        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User Player1 { get; set; }
        public virtual User Player2 { get; set; }
        public virtual User Winner { get; set; }
    }
}
