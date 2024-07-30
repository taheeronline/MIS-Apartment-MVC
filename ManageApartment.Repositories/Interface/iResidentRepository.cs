using ManageApartment.Entities;

namespace ManageApartment.Repositories.Interface
{
    public interface iResidentRepository
    {
        Task<IEnumerable<Resident>> GetAllResidentsAsync();
        Task<Resident> GetResidentByIdAsync(int id);
        Task AddResidentAsync(Resident resident);
        Task UpdateResidentAsync(Resident resident);
        Task DeleteResidentAsync(int id);
    }
}
