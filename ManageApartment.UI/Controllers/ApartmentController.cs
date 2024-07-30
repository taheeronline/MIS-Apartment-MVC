using ManageApartment.Entities;
using ManageApartment.Repositories.Interface;
using ManageApartment.UI.ViewModels.Apartment;
using Microsoft.AspNetCore.Mvc;

namespace ManageApartment.UI.Controllers
{
    public class ApartmentController : Controller
    {
        private readonly iApartmentRepository _apartmentService;
        private readonly ILogger<ApartmentController> _logger;

        public ApartmentController(iApartmentRepository apartmentService, ILogger<ApartmentController> logger)
        {
            _apartmentService = apartmentService;
            _logger = logger;

            _logger.LogDebug(1, "NLog injected into ApartmentController");
        }

        // GET: Apartment/Index
        public async Task<IActionResult> Index()
        {
            try
            {

                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    _logger.LogDebug(1, "Index Loading...");
                    var apartments = await _apartmentService.GetAllApartmentsAsync();
                    var vm = new List<vmApartment>();
                    foreach (var apartment in apartments)
                    {
                        var numberOfFlats = await _apartmentService.GetNumberOfFlatsAsync(apartment.ID);
                        vm.Add(new vmApartment
                        {
                            ID = apartment.ID,
                            Title = apartment.Title,
                            Address = apartment.Address,
                            GpsLocation = apartment.GpsLocation,
                            ResidentGroupEmail = apartment.ResidentGroupEmail,
                            AssociationGroupEmail = apartment.AssociationGroupEmail,
                            Phone1 = apartment.Phone1,
                            Phone2 = apartment.Phone2,
                            NumberOfFlats = numberOfFlats
                        }
                        );
                    }
                    _logger.LogDebug(1, "Index Loaded...");
                    return View(vm);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // Log the error or handle it as needed
                return View(ex.Message);
            }
        }

        // GET: Apartment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var apartment = await _apartmentService.GetApartmentByIdAsync(id);
                if (apartment == null)
                {
                    return NotFound();
                }

                return View(apartment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // Log the error or handle it as needed
                return View("Error");
            }
        }

        // GET: Apartment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Apartment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(vmAddApartment model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogDebug(1, "Create Initiated..");
                    var apartment = new Apartment
                    {
                        Title = model.Title,
                        Address = model.Address,
                        GpsLocation = model.GpsLocation,
                        ResidentGroupEmail = model.ResidentGroupEmail,
                        AssociationGroupEmail = model.AssociationGroupEmail,
                        Phone1 = model.Phone1,
                        Phone2 = model.Phone2
                    };
                    await _apartmentService.AddApartmentAsync(apartment);
                    _logger.LogDebug(1, "Create Completed..");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // Log the error or handle it as needed
                ModelState.AddModelError(string.Empty, "Unable to save changes. Please try again.");
            }
            return View(model);
        }

        // GET: Apartment/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var apartment = await _apartmentService.GetApartmentByIdAsync(id);
                if (apartment == null)
                {
                    return NotFound();
                }

                return View(apartment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // Log the error or handle it as needed
                return View("Error");
            }
        }

        // PUT: Apartment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, vmUpdateApartment model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogDebug(1, "Edit Initiated..");
                    var apartment = new Apartment
                    {
                        ID = model.ID,
                        Title = model.Title,
                        Address = model.Address,
                        GpsLocation = model.GpsLocation,
                        ResidentGroupEmail = model.ResidentGroupEmail,
                        AssociationGroupEmail = model.AssociationGroupEmail,
                        Phone1 = model.Phone1,
                        Phone2 = model.Phone2
                    };
                    await _apartmentService.UpdateApartmentAsync(apartment);
                    _logger.LogDebug(1, "Edit Completed..");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // Log the error or handle it as needed
                ModelState.AddModelError(string.Empty, "Unable to update changes. Please try again.");
            }
            return View(model);
        }

        // POST: Apartment/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apartmentService.DeleteApartmentAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                // Log the error or handle it as needed
                return View("Error");
            }
        }
    }
}
