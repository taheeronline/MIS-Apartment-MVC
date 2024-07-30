using System.ComponentModel.DataAnnotations;

namespace ManageApartment.Entities
{
    public class Resident
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(200)]
        public string PermanentAddress { get; set; }

        [MaxLength(200)]
        public string CurrentAddress { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Phone]
        [MaxLength(15)]
        public string Phone1 { get; set; }

        [Phone]
        [MaxLength(15)]
        public string Phone2 { get; set; }

        [Required]
        public DateTime EntryDate { get; set; }

        public DateTime? ExitDate { get; set; }

        public bool IsOwner { get; set; }
        public bool MaintenancePayee { get; set; }
        public int FlatId { get; set; }
        public Flat Flat { get; set; }
        
    }
}
