using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marc2.Domain.Entities
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RoleId")]
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
        public int RolePriority { get; set; }
        public List<User> Users { get; set; } = null!;
    }
}
