using QM.Models.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QM.DAL.Models
{
    [Table("Registrations")]
    public class RegistrationModelDB : IRegistrationModel
    {
        [Key]        
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [RegularExpression(@"\S")]
        public string UserName { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}