using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Data.Models
{
    public class Child
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
        public int? Parent1Id { get; set; }
        [ForeignKey("Parent1Id")]
        public virtual Parent Parent1 { get; set; }
        public int? Parent2Id { get; set; }
        [ForeignKey("Parent2Id")]
        public virtual Parent Parent2 { get; set; }
    }
}
