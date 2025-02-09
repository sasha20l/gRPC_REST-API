using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    [Table("GameTransactions")]
    public class GameTransactions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "bigint")]
        public long GameTransactionsId { get; set; }

        [ForeignKey(nameof(FromUser))]
        [Column(TypeName = "bigint")]
        public long fkFromUserId { get; set; }

        [ForeignKey(nameof(ToUser))]
        [Column(TypeName = "bigint")]
        public long fkToUserId { get; set; }

        public double Amount { get; set; }

        [Column, StringLength(255)]
        public string Reason { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
    }
}
