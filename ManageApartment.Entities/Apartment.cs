using System.ComponentModel.DataAnnotations;

namespace ManageApartment.Entities
{
    public class Apartment
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(200)]
        public string GpsLocation { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string ResidentGroupEmail { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string AssociationGroupEmail { get; set; }

        [Phone]
        [MaxLength(15)]
        public string Phone1 { get; set; }

        [Phone]
        [MaxLength(15)]
        public string Phone2 { get; set; }

        public ICollection<Flat> Flats { get; set; } = new List<Flat>();
    }
}
