using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagmentDAL.Entities
{
    public class HealthRecord : BaseEntity
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal Height { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Weight { get; set; }

        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
