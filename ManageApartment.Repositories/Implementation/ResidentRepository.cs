using ManageApartment.Entities;
using ManageApartment.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ManageApartment.Repositories.Implementation
{
    public class ResidentRepository : iResidentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ResidentRepository> _logger;

        public ResidentRepository(ApplicationDbContext dbContext, ILogger<ResidentRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Resident>> GetAllResidentsAsync()
        {
            try
            {
                return await _dbContext.Residents.Include(x => x.Flat).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting all residents.");
                throw;
            }
        }
        public async Task<Resident> GetResidentByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Residents.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting resident with ID {id}.");
                throw;
            }
        }

        public async Task AddResidentAsync(Resident resident)
        {
            try
            {
                _dbContext.Residents.Add(resident);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Added resident with ID {resident.ID}.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while creating resident.");
                throw;
            }
        }

        public async Task UpdateResidentAsync(Resident resident)
        {
            try
            {
                _dbContext.Residents.Update(resident);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Updated resident with ID {resident.ID}.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating resident.");
                throw;
            }
        }

        public async Task DeleteResidentAsync(int id)
        {
            try
            {
                var resident = await _dbContext.Residents.FindAsync(id);
                if (resident != null)
                {
                    _dbContext.Residents.Remove(resident);
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation($"Deleted resident with ID {id}.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting resident.");
                throw;
            }
        }
    }
}
