using ManageApartment.Entities;

namespace ManageApartment.Repositories.Interface
{
    public interface iApartmentRepository
    {
        Task<IEnumerable<Apartment>> GetAllApartmentsAsync();
        Task<Apartment> GetApartmentByIdAsync(int id);
        Task AddApartmentAsync(Apartment apartment);
        Task UpdateApartmentAsync(Apartment apartment);
        Task DeleteApartmentAsync(int id);

        Task<int> GetNumberOfFlatsAsync(int apartmentId);  // New method to get the number of flats
    }
}
