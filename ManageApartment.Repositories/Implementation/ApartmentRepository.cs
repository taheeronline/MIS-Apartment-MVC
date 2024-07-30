using ManageApartment.Entities;
using ManageApartment.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ManageApartment.Repositories.Implementation
{
    public class ApartmentRepository:iApartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ApartmentRepository> _logger;

        public ApartmentRepository(ApplicationDbContext dbContext, ILogger<ApartmentRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Apartment>> GetAllApartmentsAsync()
        {
            return await _dbContext.Apartments.ToListAsync();
        }

        public async Task<Apartment> GetApartmentByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Apartments.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting apartment with ID {id}.");
                throw;
            }
        }

        public async Task AddApartmentAsync(Apartment apartment)
        {
            try
            {
                _dbContext.Apartments.Add(apartment);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Added apartment with ID {apartment.ID}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding apartment with ID {apartment.ID}.");
                throw;
            }
        }

        public async Task UpdateApartmentAsync(Apartment apartment)
        {
            try
            {
                _dbContext.Apartments.Update(apartment);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Updated apartment with ID {apartment.ID}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating apartment with ID {apartment.ID}.");
                throw;
            }
        }

        public async Task DeleteApartmentAsync(int id)
        {
            try
            {
                var apartment = await _dbContext.Apartments.FindAsync(id);
                if (apartment != null)
                {
                    _dbContext.Apartments.Remove(apartment);
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation($"Deleted apartment with ID {id}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting apartment with ID {id}.");
                throw;
            }
        }

        public async Task<int> GetNumberOfFlatsAsync(int apartmentId)
        {
            try
            {
                var apartment = await _dbContext.Apartments.Include(a => a.Flats).FirstOrDefaultAsync(a => a.ID == apartmentId);
                return apartment?.Flats.Count ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting number of flats for apartment with ID {apartmentId}.");
                throw;
            }
        }

    }
}
