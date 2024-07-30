using ManageApartment.UI.ViewModels.Apartment;
using ManageApartment.UI.ViewModels.Flat;
using ManageApartment.UI.ViewModels.Resident;

namespace ManageApartment.UI.ViewModels.Home
{
    public class vmHome
    {
        public IEnumerable<vmApartment> vmApartments { get; set; }
        public IEnumerable<vmFlat> vmFlats { get; set; }
        public IEnumerable<vmResident> vmResidents { get; set; }
    }
}
