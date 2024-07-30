using System.ComponentModel.DataAnnotations;

namespace ManageApartment.Entities
{
    public class Flat
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Area must be greater than zero.")]
        public int Area { get; set; }
        public bool IsVacant { get; set; }

        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
        public ICollection<Resident> Residents { get; set; } = new List<Resident>();

    }
}
