using System.ComponentModel.DataAnnotations;

namespace EF.Data.Models
{
    public class Parent
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; }
    }
}
