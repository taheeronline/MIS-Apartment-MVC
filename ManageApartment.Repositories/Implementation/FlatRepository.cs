using ManageApartment.Entities;
using ManageApartment.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ManageApartment.Repositories.Implementation
{
    public class FlatRepository:iFlatRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<FlatRepository> _logger;

        public FlatRepository(ApplicationDbContext dbContext, ILogger<FlatRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Flat>> GetAllFlatsAsync()
        {
            try
            {
                return await _dbContext.Flats.Include(x => x.Apartment).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting all flats.");
                throw;
            }
        }

        public async Task<Flat> GetFlatByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Flats.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting flat with ID {id}.");
                throw;
            }
        }

        public async Task AddFlatAsync(Flat flat)
        {
            try
            {
                _dbContext.Flats.Add(flat);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Added flat with ID {flat.ID}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task UpdateFlatAsync(Flat flat)
        {
            try
            {
                _dbContext.Flats.Update(flat);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Updated flat with ID {flat.ID}.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating flat.");
                throw;
            }        }

        public async Task DeleteFlatAsync(int id)
        {
            try
            {
                var flat = await _dbContext.Flats.FindAsync(id);
                if (flat != null)
                {
                    _dbContext.Flats.Remove(flat);
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation($"Deleted flat with ID {id}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting flat with ID {id}.");
                throw;
            }
        }

        public async Task<IEnumerable<Flat>>GetFlatsByApartmentIdAsync(int id)
        {
            try
            {
                return await _dbContext.Flats
                                  .Where(f => f.ApartmentId == id)
                                  .Include(f => f.Apartment)
                                  .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting flat with Apartment ID {id}.");
                throw;
            }
        }
    }
}
