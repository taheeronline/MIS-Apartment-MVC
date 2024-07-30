using ManageApartment.Entities;

namespace ManageApartment.Repositories.Interface
{
    public interface iFlatRepository
    {
        Task<IEnumerable<Flat>> GetAllFlatsAsync();
        Task<Flat> GetFlatByIdAsync(int id);
        Task AddFlatAsync(Flat flat);
        Task UpdateFlatAsync(Flat flat);
        Task DeleteFlatAsync(int id);

        Task<IEnumerable<Flat>> GetFlatsByApartmentIdAsync(int id);
    }
}
